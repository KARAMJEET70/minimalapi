using MiniDemo.Model;

namespace MiniDemo.Services
{
    public interface IEmployees
    {
        Employee AddEmp(Employee data);
        Employee EmpId(string id);
        List<Employee> getEmp();
        Employee PutEmp(Employee data);
        public tbllogin login(tbllogin data);
    }
}