using System.Collections.Generic;
using eHR.Model;

namespace eHR.Service
{
    public interface IEmployeeService
    {
        void DeleteEmployeeById(int employeeId);
        List<Employees> GetEmployeeByCondtioin(EmployeeSearchArg arg);
        string findEmployee(int employeeID);
        void UpdateById(eHR.Model.Employees arg);
        void InsertEmployee(Employees employee);
    }
}