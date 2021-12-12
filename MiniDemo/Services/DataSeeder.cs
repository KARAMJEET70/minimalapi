using MiniDemo.Database;
using MiniDemo.Model;

namespace MiniDemo.Services
{
    public class DataSeeder
    {
        public readonly EmployeesDbContext _db;
        public DataSeeder(EmployeesDbContext db)
        {
            _db = db;
           
        }
        public void Seed()
        {
            if(!_db.Employee.Any())
            {
                var emp = new List<Employee>()
                    {
                      new Employee()
                {
                     Name = "karmjeet",
             Citizenship = "india",
             Employeeid = "1"

                },
                 new Employee()
                {
                     Name = "karan",
             Citizenship = "india",
             Employeeid = "2"

                },
                };
                _db.Employee.AddRange(emp);
                _db.SaveChanges();
            }
        }
    }
}
