using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Vitamin.Authentication;
using Vitamin.Core;
using Vitamin.Web.Models;

namespace Vitamin.Web.Controllers
{
    [Authorize]
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly UserAccountService _userAccountService;
        private readonly ILogger<AdminController> _logger;
        public AdminController(ILogger<AdminController> logger,
            UserAccountService userAccountService,
            IOptions<AuthenticationSettings> authSettings)
        {
            _authenticationSettings = authSettings.Value;
            _userAccountService = userAccountService;
            _logger = logger;
        }
        [Route("")]
        public  IActionResult Index()
        {
            return Content("Hello World");
        }

        [HttpGet("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn()
        {
            //多种登录方式 目前只使用本地登录
            switch (_authenticationSettings.Provider)
            {
                case AuthenticationProvider.AzureAD:
                    break;
                case AuthenticationProvider.Local:
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    break;
                case AuthenticationProvider.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return View();
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var uid = await _userAccountService.ValidateAsync(model.Username, model.Password);
                    if (uid != Guid.Empty)
                    {
                        var claims = new List<Claim>
                        {
                            new (ClaimTypes.Name, model.Username),
                            new (ClaimTypes.Role, "Administrator"),
                            new ("uid", uid.ToString())
                        };
                        var ci = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var p = new ClaimsPrincipal(ci);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, p);
                        await _userAccountService.LogSuccessLoginAsync(uid,
                            HttpContext.Connection.RemoteIpAddress?.ToString());

                        var successMessage = $@"Authentication success for local account ""{model.Username}""";

                        _logger.LogInformation(successMessage);
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt.");
                    return View(model);
                }

                var failMessage = $@"Authentication failed for local account ""{model.Username}""";

                _logger.LogWarning(failMessage);
                Response.StatusCode = StatusCodes.Status400BadRequest;
                ModelState.AddModelError(string.Empty, "Bad Request.");
                return View(model);
            }
            catch (Exception e)
            {
                _logger.LogWarning($@"Authentication failed for local account ""{model.Username}""");

                ModelState.AddModelError(string.Empty, e.Message);
                return View(model);
            }
        }
        /// <summary>
        /// 403 拒绝访问后跳转至登录页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("accessdenied")]
        public IActionResult AccessDenied()
        {
            return Forbid();
        }
    }
}
