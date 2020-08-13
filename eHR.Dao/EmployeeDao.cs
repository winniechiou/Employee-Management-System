using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eHR.Model;
using System.Data.SqlClient;
using System.Data;

namespace eHR.Dao
{
	public class EmployeeDao : IEmployeeDao
    {

		/// <summary>
		/// 取得DB連線字串
		/// </summary>
		/// <returns></returns>
		private string GetDBConnectionString()
		{
			return eHR.Common.ConfigTool.GetDBConnectionString("DBConn");
		}

		/// <summary>
		/// 新增員工
		/// </summary>
		/// <param name="employee"></param>
		/// <returns>員工編號</returns>
		public void InsertEmployee(Employees employee)
		{
			string sql = @" INSERT INTO HR.Employees
						 (
							 FirstName, LastName, Title, TitleOfCourtesy, Gender, ManagerID, 
							 HireDate, BirthDate, 
							 Address, City, Country, Phone, 
							 MonthlyPayment, YearlyPayment
						 )
						VALUES
						(
							 @EmployeeFirstName,@EmployeeLastName, @JobTitle, @TitleOfCourtesy, @Gender, @ManagerId, 
							 @HireDate, @BirthDate,
							 @Address, @City, @Country, @Phone, 
							 @MonthlyPayment, @YearlyPayment
						)";
			using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
			{
				conn.Open();
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.Add(new SqlParameter("@EmployeeFirstName", employee.EmployeeFirstName));
				cmd.Parameters.Add(new SqlParameter("@EmployeeLastName", employee.EmployeeLastName));
				cmd.Parameters.Add(new SqlParameter("@JobTitle", employee.JobTitleId));
				cmd.Parameters.Add(new SqlParameter("@TitleOfCourtesy", employee.TitleOfCourtesy == null ? string.Empty : employee.TitleOfCourtesy));
				cmd.Parameters.Add(new SqlParameter("@HireDate", employee.HireDate == null ? string.Empty : employee.HireDate));
				cmd.Parameters.Add(new SqlParameter("@BirthDate", employee.BirthDate == null ? string.Empty : employee.BirthDate));
				cmd.Parameters.Add(new SqlParameter("@Address", employee.Address == null ? string.Empty : employee.Address));
				cmd.Parameters.Add(new SqlParameter("@City", employee.City == null ? string.Empty : employee.City));
				cmd.Parameters.Add(new SqlParameter("@Gender", employee.GenderId == null ? string.Empty : employee.GenderId));
				cmd.Parameters.Add(new SqlParameter("@Country", employee.Country == null ? string.Empty : employee.Country));
                cmd.Parameters.Add(new SqlParameter("@ManagerId", employee.ManagerId == null ? string.Empty : employee.ManagerId));
				cmd.Parameters.Add(new SqlParameter("@Phone", employee.Phone == null ? string.Empty : employee.Phone));
				cmd.Parameters.Add(new SqlParameter("@MonthlyPayment", employee.MonthlyPayment == null ? string.Empty : employee.MonthlyPayment));
				cmd.Parameters.Add(new SqlParameter("@YearlyPayment", employee.YearlyPayment == null ? string.Empty : employee.YearlyPayment));

                SqlTransaction tran = conn.BeginTransaction();
                cmd.Transaction = tran;
                try
                {
                    cmd.ExecuteNonQuery();//可以知道影響幾筆(int)
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
			
		}


		/// <summary>
		/// 依照條件取得員工資料
		/// </summary>
		/// <returns></returns>
		public List<Employees> GetEmployeeByCondtioin(EmployeeSearchArg arg)
		{

			DataTable dt = new DataTable();
			string sql = @"SELECT e.EmployeeID, 
                                    e.FirstName,
                                    e.LastName, 
                                    e.Title AS JobTitle, 
                                    ctj.CodeVal AS JobTitleId, 
                                    e.TitleOfCourtesy,
                                    CONVERT( varchar(10), HireDate, 111) AS HireDate, 
								    CONVERT( varchar(10), BirthDate, 111) AS BirthDate, 
								    DATEPART(yyyy, GETDATE()) - YEAR(e.BirthDate) AS Age, 
                                    e.Address, e.City, e.Country,
								    e.Gender AS GenderId, 
                                    ctg.CodeVal AS Gender,
                                    e.ManagerID, 
								    e.Phone,
                                    e.MonthlyPayment, 
                                    e.YearlyPayment
						   FROM HR.Employees as e 
						   LEFT JOIN dbo.CodeTable as ctj
							   ON (e.Title = ctj.CodeId AND ctj.CodeType = 'TITLE')
						   LEFT JOIN dbo.CodeTable as ctg
							   ON (e.Gender = ctg.CodeId)
						   Where (e.EmployeeID = @EmployeeId OR @EmployeeId='') AND
								 (UPPER(e.FirstName + ' ' + e.LastName) LIKE UPPER('%' + @EmployeeName + '%')or @EmployeeName='') AND
								 (e.Title = @JobTitleId OR @JobTitleId='')AND
								 ((e.HireDate BETWEEN @HireDateStart AND @HireDateEnd))";

			using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
			{
				conn.Open();
				SqlCommand cmd = new SqlCommand(sql, conn);
				cmd.Parameters.Add(new SqlParameter("@EmployeeId", arg.EmployeeId == null ? string.Empty : arg.EmployeeId));
				cmd.Parameters.Add(new SqlParameter("@EmployeeName", arg.EmployeeName == null ? string.Empty : arg.EmployeeName));
				cmd.Parameters.Add(new SqlParameter("@JobTitleId", arg.JobTitleId == null ? string.Empty : arg.JobTitleId));
				cmd.Parameters.Add(new SqlParameter("@HireDateStart", arg.HireDateStart == null ? "1900/01/01" : arg.HireDateStart));
				cmd.Parameters.Add(new SqlParameter("@HireDateEnd", arg.HireDateEnd == null ? "2500/12/31" : arg.HireDateEnd));
				SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
				sqlAdapter.Fill(dt);
				conn.Close();
			}
			return this.MapEmployeeDataToList(dt);
		}



        /// <summary>
        /// 確認員工是否存在
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public string findEmployee(int employeeID)
        {
            string employeeid = "";//跟傳入值命名不能一樣  select EmployeeID from HR.Employees
            string sql = @"  select EmployeeID from HR.Employees where EmployeeID=@employeeID";
          
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@employeeID", employeeID));
                employeeid = Convert.ToString(cmd.ExecuteScalar());
            }
            return employeeid;
        }


        /// <summary>
        /// 修改書籍資料
        /// </summary>
        public void UpdateById(eHR.Model.Employees arg)
        {
            string sql = @"UPDATE HR.Employees
                            SET
                                LastName =@EmployeeLastName,
                                FirstName =@EmployeeFirstName,
                                Title =@JobTitleId,
                                TitleOfCourtesy =@TitleOfCourtesy,
                                BirthDate=CAST(@BirthDate AS DATETIME),
                                HireDate=CAST(@HireDate AS DATETIME),
                                Address=@Address,
                                City=@City,
                                Country=@Country,
                                Phone=@Phone,
                                ManagerID=@ManagerId,
                                Gender=@GenderId,
                                MonthlyPayment=@MonthlyPayment,
                                YearlyPayment=@YearlyPayment
                            WHERE
                                EmployeeID =@EmployeeId;";


          
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@EmployeeId", arg.EmployeeId));
                cmd.Parameters.Add(new SqlParameter("@EmployeeLastName", arg.EmployeeLastName));
                cmd.Parameters.Add(new SqlParameter("@EmployeeFirstName", arg.EmployeeFirstName));
                cmd.Parameters.Add(new SqlParameter("@JobTitleId", arg.JobTitleId));
                cmd.Parameters.Add(new SqlParameter("@TitleOfCourtesy", arg.TitleOfCourtesy == null ? string.Empty : arg.TitleOfCourtesy));
                cmd.Parameters.Add(new SqlParameter("@BirthDate", arg.BirthDate == null ? string.Empty : arg.BirthDate));
                cmd.Parameters.Add(new SqlParameter("@HireDate", arg.HireDate == null ? string.Empty : arg.HireDate));
                cmd.Parameters.Add(new SqlParameter("@Address", arg.Address == null ? string.Empty : arg.Address));
                cmd.Parameters.Add(new SqlParameter("@City", arg.City == null ? string.Empty : arg.City));
                cmd.Parameters.Add(new SqlParameter("@Country", arg.Country == null ? string.Empty : arg.Country));
                cmd.Parameters.Add(new SqlParameter("@Phone", arg.Phone == null ? string.Empty : arg.Phone));
                cmd.Parameters.Add(new SqlParameter("@ManagerId", arg.ManagerId == null ? string.Empty : arg.ManagerId));
                cmd.Parameters.Add(new SqlParameter("@GenderId", arg.GenderId == null ? string.Empty : arg.GenderId));
                cmd.Parameters.Add(new SqlParameter("@MonthlyPayment", arg.MonthlyPayment == null ? string.Empty : arg.MonthlyPayment));
                cmd.Parameters.Add(new SqlParameter("@YearlyPayment", arg.YearlyPayment == null ? string.Empty : arg.YearlyPayment));
                SqlTransaction tran = conn.BeginTransaction();
                cmd.Transaction = tran;
                try
                {
                    cmd.ExecuteNonQuery();//可以知道影響幾筆(int)
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
                finally
                {
                    conn.Close();
                }
            }
        }


        /// <summary>
        /// 刪除員工
        /// </summary>
        public void DeleteEmployeeById(int employeeId)
		{
			try
			{
				string sql = "Delete FROM HR.Employees Where EmployeeID=@EmployeeId";
				using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
				{
					conn.Open();
					SqlCommand cmd = new SqlCommand(sql, conn);
					cmd.Parameters.Add(new SqlParameter("@EmployeeId", employeeId));
					cmd.ExecuteNonQuery();
					conn.Close();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}


		/// <summary>
		/// Map資料進List
		/// </summary>
		/// <param name="employeeData"></param>
		/// <returns></returns>

		private List<Employees> MapEmployeeDataToList(DataTable employeeData)
		{
			List<Employees> result = new List<Employees>();
			foreach (DataRow row in employeeData.Rows)
			{
				result.Add(new Employees()
				{
					EmployeeId = (int)row["EmployeeID"],
					EmployeeFirstName = row["FirstName"].ToString(),
					EmployeeLastName = row["LastName"].ToString(),
					JobTitleId = row["JobTitle"].ToString(),
					JobTitle = row["JobTitleId"].ToString(),
					TitleOfCourtesy = row["TitleOfCourtesy"].ToString(),
					HireDate = row["HireDate"].ToString(),
					BirthDate = row["BirthDate"].ToString(),
					Age = (int)row["Age"],
					Address = row["Address"].ToString(),
					City = row["City"].ToString(),
					Country = row["Country"].ToString(),
					GenderId = row["GenderId"].ToString(),
					Gender = row["Gender"].ToString(),
					ManagerId = row["ManagerID"].ToString(),
					Phone = row["Phone"].ToString(),
					MonthlyPayment = row["MonthlyPayment"].ToString(),
					YearlyPayment = row["YearlyPayment"].ToString()
				});
			}
			return result;
		}

	}
}
