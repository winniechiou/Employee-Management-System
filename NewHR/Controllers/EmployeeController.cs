using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eHR.Model;
using eHR.Service;

namespace NewHR.Controllers
{
    public class EmployeeController : Controller
    {

        private ICodeService codeService { get; set; }
        private IEmployeeService employeeService { get; set; }
        

        public ActionResult Index()
        {
            try
            {
              
                return View();
            }
            catch (Exception ex)
            {
                eHR.Common.Logger.Write(eHR.Common.Logger.LogCategoryEnum.Error, ex.ToString());
                return View("Error");
            } 
        }




        /// <summary>
        /// 生成DropDownListDataForPosition
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetDropDownListDataForPosition(string type)
        {

            List<SelectListItem> dropdownlistlist = codeService.GetCodeTable(type);
            return Json(dropdownlistlist);
        }

        /// <summary>
        /// 生成DropDownListDataForPosition
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetDropDownListDataForEmployee(string employeeId)
        {

            List<SelectListItem> dropdownlistlist = codeService.GetEmployee("0");
            return Json(dropdownlistlist);
        }






        /// <summary>
        /// 查詢員工
        /// </summary>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult SearchEmployee(EmployeeSearchArg arg)
        {
            List<Employees> searchResult = employeeService.GetEmployeeByCondtioin(arg);
            return Json(searchResult);
        }

        /// <summary>
        /// 取得欲修改書籍資料or明細
        /// </summary>
        [HttpPost()]
        public JsonResult GetEmployeeData(EmployeeSearchArg ee)
        {
            List<Employees> employeeData = new List<Employees>();
            int eid = int.Parse(ee.EmployeeId);
            if (employeeService.findEmployee(eid) != "")
            {
                employeeData = employeeService.GetEmployeeByCondtioin(ee);
                return Json(employeeData);
            }
            else
            {
                string alert = "EmployeeNotExist";
                return Json(alert);
            }
        }

        

        /// <summary>
        /// 修改書籍
        /// </summary>
        [HttpPost()]
        public JsonResult UpdateEmployee(eHR.Model.Employees employees)
        {
            employeeService.UpdateById(employees);
            return Json(true);
        }


        /// <summary>
        /// 刪除員工
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult DeleteEmployee(int employeeid)
        {
            try
            {
                string checkStatus = "";
                if (employeeService.findEmployee(employeeid) != "")
                {
                    //書存在 可以刪除
                        checkStatus = "canBeDelete";
                    employeeService.DeleteEmployeeById(employeeid);
                        return this.Json(checkStatus);
                    
                }
                else
                {
                    //書不存在 不可刪除
                    checkStatus = "employeeNotExist";
                    return this.Json(checkStatus);
                }
            }

            catch (Exception ex)
            {
                return this.Json(false);
            }
        }

        ///// <summary>
        ///// 員工資料查詢(查詢)
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost()]
        //public ActionResult Index(EmployeeSearchArg arg)
        //{
        //    if (arg.HireDateEnd == null)
        //        arg.HireDateEnd = DateTime.Now.ToShortDateString();
        //    ViewBag.SearchResult = employeeService.GetEmployeeByCondtioin(arg);
        //    ViewBag.JobTitleCodeData = codeService.GetCodeTable("TITLE");
        //    return View("Index");
        //}

        ///// <summary>
        ///// 新增員工畫面
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet()]
        //public ActionResult InsertEmployee()
        //{
        //    ViewBag.JobTitleCodeData = this.codeService.GetCodeTable("TITLE");
        //    ViewBag.CountryCodeData = this.codeService.GetCodeTable("COUNTRY");
        //    ViewBag.CityCodeData = this.codeService.GetCodeTable("CITY");
        //    ViewBag.GenderCodeData = this.codeService.GetCodeTable("GENDER");
        //    ViewBag.EmpCodeData = this.codeService.GetEmployee("0");
        //    return View(new Employees());
        //}

        /// <summary>
        /// 新增員工
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult InsertEmployee(Employees employee)
        {
            employeeService.InsertEmployee(employee);
            return Json(true);
        }


        ///// <summary>
        ///// 刪除員工
        ///// </summary>
        ///// <param name="employeeId"></param>
        ///// <returns></returns>
        //[HttpPost()]
        //public JsonResult DeleteEmployee(string employeeId)
        //{
        //    try
        //    {
        //        employeeService.DeleteEmployeeById(employeeId);
        //        return this.Json(true);
        //    }

        //    catch (Exception ex)
        //    {
        //        return this.Json(false);
        //    }
        //}

        ///// <summary>
        ///// 修改員工畫面
        ///// </summary>
        ///// <param name="employeeId"></param>
        ///// <returns></returns>
        //public ActionResult UpdateEmployee(string employeeId)
        //{
        //    return View();
        //}

    }
}