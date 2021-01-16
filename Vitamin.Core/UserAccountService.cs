using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Vitamin.Data;
using Vitamin.Data.Infrastructure;
using Vitamin.Data.Model;

namespace Vitamin.Core
{
    /// <summary>
    /// 用户操作
    /// </summary>
    public class UserAccountService : VitaminService
    {
        private readonly IRepository<UserAccountEntity> _accountRepo;
        public UserAccountService(IRepository<UserAccountEntity> accountRepo)
        {
            _accountRepo = accountRepo;
        }
        //总数
        public int Count()
        {
            return _accountRepo.Count(p => true);
        }
        //异步获取单个（ID）
        public async Task<Account> GetAsync(Guid id)
        {
            var entity = await _accountRepo.GetAsync(id);
            var item = EntityToAccountModel(entity);
            return item;
        }
        //异步获取所有
        public Task<IReadOnlyList<Account>> GetAllAsync()
        {
            var list = _accountRepo.SelectAsync(p => new Account
            {
                Id = p.Id,
                CreateOnUtc = p.CreateOnUtc,
                LastLoginIp = p.LastLoginIp,
                LastLoginTimeUtc = p.LastLoginTimeUtc,
                Username = p.Username
            });

            return list;
        }
        //验证账号密码是否为空
        public async Task<Guid> ValidateAsync(string username, string inputPassword)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException(nameof(username), "value must not be empty.");
            }

            if (string.IsNullOrWhiteSpace(inputPassword))
            {
                throw new ArgumentNullException(nameof(inputPassword), "value must not be empty.");
            }

            var account = await _accountRepo.GetAsync(p => p.Username == username);
            if (account==null)
            {
                throw new ArgumentNullException(nameof(username), "Authentication failed for local account");
            }
            var valid = account.PasswordHash == HashPassword(inputPassword.Trim());
                return valid ? account.Id : Guid.Empty;

            
        }
        /// <summary>
        ///  获取登录成功ip
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public async Task LogSuccessLoginAsync(Guid id, string ipAddress)
        {
            var entity = await _accountRepo.GetAsync(id);
            if (entity is not null)
            {
                entity.LastLoginIp = ipAddress.Trim();
                entity.LastLoginTimeUtc = DateTime.UtcNow;
            }

            await _accountRepo.UpdateAsync(entity);
        }
        /// <summary>
        /// 用户是否存在
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool Exist(string username)
        {
            var exist = _accountRepo.Any(p => p.Username == username.ToLower());
            return exist;
        }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="username"></param>
        /// <param name="clearPassword"></param>
        /// <returns></returns>
        public async Task<Guid> CreateAsync(string username, string clearPassword)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException(nameof(username), "value must not be empty.");
            }

            if (string.IsNullOrWhiteSpace(clearPassword))
            {
                throw new ArgumentNullException(nameof(clearPassword), "value must not be empty.");
            }

            var uid = Guid.NewGuid();
            var account = new UserAccountEntity
            {
                Id = uid,
                CreateOnUtc = DateTime.UtcNow,
                Username = username.ToLower().Trim(),
                PasswordHash = HashPassword(clearPassword.Trim())
            };

            await _accountRepo.AddAsync(account);

            return uid;
        }
        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="clearPassword"></param>
        /// <returns></returns>
        public async Task UpdatePasswordAsync(Guid id, string clearPassword)
        {
            if (string.IsNullOrWhiteSpace(clearPassword))
            {
                throw new ArgumentNullException(nameof(clearPassword), "value must not be empty.");
            }

            var account = await _accountRepo.GetAsync(id);
            if (account is null)
            {
                throw new InvalidOperationException($"LocalAccountEntity with Id '{id}' not found.");
            }

            account.PasswordHash = HashPassword(clearPassword);
            await _accountRepo.UpdateAsync(account);

            }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Guid id)
        {
            var account = await _accountRepo.GetAsync(id);
            if (account is null)
            {
                throw new InvalidOperationException($"LocalAccountEntity with Id '{id}' not found.");
            }

            _accountRepo.Delete(id);
         }
        /// <summary>
        /// 密码加密方式
        /// </summary>
        /// <param name="plainMessage"></param>
        /// <returns></returns>
        public static string HashPassword(string plainMessage)
        {
            if (string.IsNullOrWhiteSpace(plainMessage)) return string.Empty;

            var data = Encoding.UTF8.GetBytes(plainMessage);
            using HashAlgorithm sha = new SHA256Managed();
            sha.TransformFinalBlock(data, 0, data.Length);
            return Convert.ToBase64String(sha.Hash);
        }

        /// <summary>
        /// 实体到用户模型
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static Account EntityToAccountModel(UserAccountEntity entity)
        {
            if (entity is null) return null;

            return new Account
            {
                Id = entity.Id,
                CreateOnUtc = entity.CreateOnUtc,
                LastLoginIp = entity.LastLoginIp.Trim(),
                LastLoginTimeUtc = entity.LastLoginTimeUtc.GetValueOrDefault(),
                Username = entity.Username.Trim()
            };
        }
    }
}
