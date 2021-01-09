using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Vitamin.Data.Configurations
{
    internal class UserAccountConfiguration : IEntityTypeConfiguration<UserAccountEntity>
    {
        //用户信息 通用
        public void Configure(EntityTypeBuilder<UserAccountEntity> builder)
        {
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.Username).HasMaxLength(32);
            builder.Property(e => e.PasswordHash).HasMaxLength(64);
            builder.Property(e => e.LastLoginIp).HasMaxLength(64);
            builder.Property(e => e.CreateOnUtc).HasColumnType("datetime");
            builder.Property(e => e.LastLoginTimeUtc).HasColumnType("datetime");
        }
    }
}
