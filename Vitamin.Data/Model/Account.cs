using System;
using System.Collections.Generic;
using System.Text;

namespace Vitamin.Data.Model
{
    /// <summary>
    /// 用户模型
    /// </summary>
    public class Account
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public DateTime? LastLoginTimeUtc { get; set; }
        public string LastLoginIp { get; set; }
        public DateTime CreateOnUtc { get; set; }
    }
}
