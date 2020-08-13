using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace eHR.Service
{

    public class CodeService : ICodeService
    {
        private eHR.Dao.ICodeDao codeDao { get; set; }

        public List<SelectListItem> GetEmployee(string employeeId)
        {
            return codeDao.GetEmployee(employeeId);
        }
        public List<SelectListItem> GetCodeTable(string type)
        {
            return codeDao.GetCodeTable(type);
        }

    }   
}
