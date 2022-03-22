using InterviewTest.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InterviewTest.Controllers
{
    public class EmployeeController : Controller
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=InterviewTest;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public ActionResult Index()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM dbo.Employee", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    EmployeeModel employee = new EmployeeModel();
                    employee.Name = reader["Name"].ToString();
                    employee.DOB = reader["DOB"].ToString();
                    employee.Gender = reader["Gender"].ToString();
                    employee.Nationality = reader["Nationality"].ToString();
                    employee.IC = reader["IC"].ToString();
                    employee.EmpNo = reader["EmpNo"].ToString();
                    employee.HiredDate = reader["HiredDate"].ToString();
                    employee.Department = reader["Department"].ToString();
                    employee.Occupation = reader["Occupation"].ToString();
                    employee.Category = reader["Category"].ToString();
                    employeeList.Add(employee);
                }
                connection.Close();
            }
            return View(employeeList);
        }

        public ActionResult Create()
        {
            List<string> allCodes = new List<string>();

            string query = "SELECT * FROM dbo.Code";

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(query, con);

            using (con)
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allCodes.Add(Convert.ToString(reader["Code"]));
                    }
                }
                con.Close();
            }

            ViewBag.Category = new SelectList(allCodes);

            return View();
        }

        [HttpPost]
        public string CreateEmployee(EmployeeModel employee)
        {
            string success;

            // check for duplications in database using IC or EmpNo
            string query = "SELECT * FROM dbo.Employee WHERE IC = @IC OR EmpNo = @EmpNo";

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@IC", employee.IC);
            cmd.Parameters.AddWithValue("@EmpNo", employee.EmpNo);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                success = "Duplicated data";
            }
            else
            {
                dr.Close();
                // insert new employee
                query = "insert into dbo.Employee (Name, DOB, Gender, Nationality, IC, EmpNo, HiredDate, Department, Occupation, Category) values (@Name, @DOB, @Gender, @Nationality, @IC, @EmpNo, @HiredDate, @Department, @Occupation, @Category)";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", employee.Name);
                cmd.Parameters.AddWithValue("@DOB", employee.DOB);
                cmd.Parameters.AddWithValue("@Gender", employee.Gender);
                cmd.Parameters.AddWithValue("@Nationality", employee.Nationality);
                cmd.Parameters.AddWithValue("@IC", employee.IC);
                cmd.Parameters.AddWithValue("@EmpNo", employee.EmpNo);
                cmd.Parameters.AddWithValue("@HiredDate", employee.HiredDate);
                cmd.Parameters.AddWithValue("@Department", employee.Department);
                cmd.Parameters.AddWithValue("@Occupation", employee.Occupation);
                cmd.Parameters.AddWithValue("@Category", employee.Category);
                cmd.ExecuteNonQuery();
                success = "Added";
            }
            con.Close();
            return success;
        }

        public ActionResult Edit(String IC)
        {
            // if IC is null, return to index
            if (IC == null)
            {
                return RedirectToAction("Index");
            }
            
            // find employee by ic
            string query = "SELECT * FROM dbo.Employee WHERE IC = @IC";
            EmployeeModel employee = new EmployeeModel();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@IC", IC);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    employee.Name = reader["Name"].ToString();
                    employee.DOB = reader["DOB"].ToString().Trim();
                    employee.Gender = reader["Gender"].ToString();
                    employee.Nationality = reader["Nationality"].ToString();
                    employee.IC = reader["IC"].ToString();
                    employee.EmpNo = reader["EmpNo"].ToString();
                    employee.HiredDate = reader["HiredDate"].ToString().Trim();
                    employee.Department = reader["Department"].ToString();
                    employee.Occupation = reader["Occupation"].ToString();
                    employee.Category = reader["Category"].ToString();
                }

                connection.Close();
            }

            // get department list
            List<string> allCodes = new List<string>();

            query = "SELECT * FROM dbo.Code";

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(query, con);

            using (con)
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allCodes.Add(Convert.ToString(reader["Code"]));
                    }
                }
                con.Close();
            }

            ViewBag.Category = new SelectList(allCodes);
            
            return View(employee);
        }

        [HttpPost]
        public string EditEmployee(EmployeeModel employee)
        {
            string success;

            // update the employee in database using employee IC
            string query = "SELECT * FROM dbo.Employee WHERE IC = @IC";

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@IC", employee.IC.Trim());

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Close();
                // update the employee
                query = "UPDATE dbo.Employee SET Name = @Name, DOB = @DOB, Gender = @Gender, Nationality = @Nationality, IC = @IC, EmpNo = @EmpNo, HiredDate = @HiredDate, Department = @Department, Occupation = @Occupation, Category = @Category WHERE IC = @IC";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Name", employee.Name.Trim());
                cmd.Parameters.AddWithValue("@DOB", employee.DOB.Trim());
                cmd.Parameters.AddWithValue("@Gender", employee.Gender.Trim());
                cmd.Parameters.AddWithValue("@Nationality", employee.Nationality.Trim());
                cmd.Parameters.AddWithValue("@IC", employee.IC.Trim());
                cmd.Parameters.AddWithValue("@EmpNo", employee.EmpNo.Trim());
                cmd.Parameters.AddWithValue("@HiredDate", employee.HiredDate.Trim());
                cmd.Parameters.AddWithValue("@Department", employee.Department.Trim());
                cmd.Parameters.AddWithValue("@Occupation", employee.Occupation.Trim());
                cmd.Parameters.AddWithValue("@Category", employee.Category.Trim());
                cmd.ExecuteNonQuery();
                success = "Updated";
            }
            else
            {
                success = "No data to update";
            }
            con.Close();
            return success;
        }

        public ActionResult DeleteEmployee(String IC)
        {
            // if IC is null, return to index
            if (IC == null)
            {
                return RedirectToAction("Index");
            }

            // delete the employee using IC
            string query = "DELETE FROM dbo.Employee WHERE IC = @IC";

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@IC", IC);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            return RedirectToAction("Index");
        }
    }
}