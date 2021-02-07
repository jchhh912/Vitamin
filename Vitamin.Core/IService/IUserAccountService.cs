using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vitamin.Data.Model;

namespace Vitamin.Core.IService
{
    public interface IUserAccountService
    {
        int Count();
        Task<Account> GetAsync(Guid id);
        Task<IReadOnlyList<Account>> GetAllAsync();
        Task<Guid> ValidateAsync(string username, string inputPassword);
        Task LogSuccessLoginAsync(Guid id, string ipAddress);
        bool Exist(string username);
        Task<Guid> CreateAsync(string username, string clearPassword);
        Task UpdatePasswordAsync(Guid id, string clearPassword);
        Task DeleteAsync(Guid id);
    }
}
