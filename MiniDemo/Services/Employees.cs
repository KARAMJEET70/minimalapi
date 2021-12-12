using MiniDemo.Database;
using MiniDemo.Model;

namespace MiniDemo.Services
{
    public class Employees : IEmployees
    {

        public readonly EmployeesDbContext db;
        public Employees(EmployeesDbContext _db)
        {
            db = _db;
        }

        public tbllogin login(tbllogin data)
        {
            tbllogin user = db.tbllogin.FirstOrDefault(id => id.EmailId.Equals(data.EmailId) && id.pass.Equals(data.pass));
            return user;
        }

        public List<Employee> getEmp()
        {
            return db.Employee.ToList();
        }

        public Employee PutEmp(Employee data)
        {
            db.Employee.Update(data);
            db.SaveChanges();
            return db.Employee.Where(x => x.Employeeid == data.Employeeid).FirstOrDefault();
        }
        public Employee AddEmp(Employee data)
        {
            db.Employee.Add(data);
            db.SaveChanges();
            return db.Employee.Where(x => x.Employeeid == data.Employeeid).FirstOrDefault();
        }

        public Employee EmpId(string id)
        {
            return db.Employee.Where(x => x.Employeeid == id).FirstOrDefault();
        }
    }
}
