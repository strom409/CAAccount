using Microsoft.EntityFrameworkCore;

namespace BlazorDemo.Services
{
    public class SqlDbContext : DbContext
    {
        public SqlDbContext(DbContextOptions<SqlDbContext> options)
           : base(options)
        {
        }
       // public DbSet<Employee> Employees { get; set; }
    }
}
