using System;

namespace Vitamin.Data
{
    //用户实体
    public class UserAccountEntity
    {
       
        public Guid Id { get; set; }
        //用户名
        public string Username { get; set; }
        //用户密码
        public string PasswordHash { get; set; }
        //用户上次登录时间
        public DateTime? LastLoginTimeUtc { get; set; }
        //用户最后一次IP
        public string LastLoginIp { get; set; }
        //创建时间
        public DateTime CreateOnUtc { get; set; }
    }
}
