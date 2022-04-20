using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Presistence.Context;
public abstract class BaseDbContext:IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, ApplicationRoleClaim, IdentityUserToken<string>>
{
    public IDbConnection Connection => Database.GetDbConnection();
    public BaseDbContext(DbContextOptions options) 
        :base(options) 
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // TODO: We want this only for development probably... maybe better make it configurable in logger.json config?
        optionsBuilder.EnableSensitiveDataLogging();
    }

}

