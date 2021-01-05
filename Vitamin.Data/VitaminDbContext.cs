using Microsoft.EntityFrameworkCore;


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

    }
}
