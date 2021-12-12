using Microsoft.EntityFrameworkCore;
using MiniDemo.Model;

namespace MiniDemo.Database
{
    public class EmployeesDbContext : DbContext
    {
        public EmployeesDbContext(DbContextOptions<EmployeesDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employee { get; set; }
        public DbSet<tbllogin> tbllogin { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var con = new  ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var consting = con.GetConnectionString("AppDB");
            optionsBuilder.UseSqlServer(consting);
                
           
               
            


        }
    }
}
