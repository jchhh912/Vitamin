using Microsoft.EntityFrameworkCore;
using Vitamin.Data.Configurations;

namespace Vitamin.Data
{
    public class VitaminDbContext : DbContext
    {
        public VitaminDbContext()
        {
        }

        public VitaminDbContext(DbContextOptions options)
            : base(options)
        {
        }
        //创建数据表
        public virtual DbSet<UserAccountEntity> UserAccount { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserAccountConfiguration());
        }
    }
}
