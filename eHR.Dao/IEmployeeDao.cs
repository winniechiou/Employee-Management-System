using System.Collections.Generic;
using eHR.Model;

namespace eHR.Dao
{
    public interface IEmployeeDao
    {
        void DeleteEmployeeById(int employeeId);
        string findEmployee(int employeeID);
        List<Employees> GetEmployeeByCondtioin(EmployeeSearchArg arg);
        void UpdateById(eHR.Model.Employees arg);
        void InsertEmployee(Employees employee);
    }
}