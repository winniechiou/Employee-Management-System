using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eHR.Model;

namespace eHR.Service
{
    public class EmployeeService : IEmployeeService
    {
        private eHR.Dao.IEmployeeDao employeeDao { get; set; }

        public List<Employees> GetEmployeeByCondtioin(EmployeeSearchArg arg)
        {
            return employeeDao.GetEmployeeByCondtioin(arg);
        }

        public void InsertEmployee(Employees employee)
        {
            employeeDao.InsertEmployee(employee);
        }

        

        public string findEmployee(int employeeID)
        {
            return employeeDao.findEmployee(employeeID);
        }

        public void UpdateById(eHR.Model.Employees arg) {
            employeeDao.UpdateById(arg);
        }

        public void DeleteEmployeeById(int employeeId)
        {
            employeeDao.DeleteEmployeeById(employeeId);
        }
    }
 }