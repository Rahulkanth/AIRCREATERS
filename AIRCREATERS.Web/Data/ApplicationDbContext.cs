using AIRCREATERS.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace AIRCREATERS.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {

        }

        public DbSet<Brand> Brand { get; set; }
    }
}
