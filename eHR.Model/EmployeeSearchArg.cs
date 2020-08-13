using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eHR.Model
{
    public class EmployeeSearchArg
    {
        [DisplayName("員工編號")]
        public string EmployeeId { get; set; }
        [DisplayName("員工姓名")]
        public string EmployeeName { get; set; }
        [DisplayName("職稱")]
        public string JobTitleId { get; set; }
        [DisplayName("任職起日")]
        public string HireDateStart { get; set; }
        [DisplayName("任職迄日")]
        public string HireDateEnd { get; set; }
    }
}
