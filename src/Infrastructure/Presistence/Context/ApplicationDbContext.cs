using Domain.Blog;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Presistence.Context;

public class ApplicationDbContext :BaseDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Tags> Tags => Set<Tags>();
    public DbSet<Categorys> Categorys => Set<Categorys>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
