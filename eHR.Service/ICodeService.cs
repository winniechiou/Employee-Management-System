using System.Collections.Generic;
using System.Web.Mvc;

namespace eHR.Service
{
    public interface ICodeService
    {
        List<SelectListItem> GetCodeTable(string type);
        List<SelectListItem> GetEmployee(string employeeId);
    }
}