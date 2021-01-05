using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Vitamin.Core;
using Vitamin.Web.Models;

namespace Vitamin.Web.Controllers
{
    [Authorize]
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly UserAccountService _userAccountService;
        private readonly ILogger<AdminController> _logger;
        public AdminController(ILogger<AdminController> logger,
            UserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
            _logger = logger;
        }
        [Route("")]
        public async Task<IActionResult> Index()
        {
            return RedirectToAction("Index");
        }

        [HttpGet("signin")]
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
                            new (ClaimTypes.Name,model.Username),
                            new (ClaimTypes.Role,"Administrator"),
                            new ("uid",uid.ToString())
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
    }
}
