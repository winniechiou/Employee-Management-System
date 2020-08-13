using System.Collections.Generic;
using System.Web.Mvc;

namespace eHR.Dao
{
    public interface ICodeDao
    {

        List<SelectListItem> GetCodeTable(string type);
        List<SelectListItem> GetEmployee(string employeeId);
    }
}