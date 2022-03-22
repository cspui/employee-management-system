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
    public class CodeController : Controller
    {
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=InterviewTest;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public ActionResult Index()
        {
            List<CodeModel> codeList = new List<CodeModel>();


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM dbo.Code", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CodeModel code = new CodeModel();
                    code.Name = reader["Code"].ToString();
                    code.Description = reader["Description"].ToString();
                    code.Active = reader["Active"].ToString();
                    code.Category = reader["Category"].ToString();
                    code.LinkToCode = reader["LinkToCode"].ToString();
                    codeList.Add(code);
                }
                connection.Close();
            }
            return View(codeList);
        }

        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public string CreateCode(CodeModel code)
        {
            string success;

            // check for duplications in database using Code name
            string query = "SELECT * FROM dbo.Code WHERE Code = @Code";

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@Code", code.Name);

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
                query = "insert into dbo.Code(Code, Description, Active, Category, LinkToCode) values(@Code, @Description, @Active, @Category, @LinkToCode)";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Code", code.Name);
                cmd.Parameters.AddWithValue("@Description", code.Description);
                cmd.Parameters.AddWithValue("@Active", code.Active);
                cmd.Parameters.AddWithValue("@Category", code.Category);
                if (code.LinkToCode == null)
                {
                    cmd.Parameters.AddWithValue("@LinkToCode", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@LinkToCode", code.LinkToCode);
                }
                cmd.ExecuteNonQuery();
                success = "Added";
            }
            con.Close();
            return success;
        }

        public ActionResult Edit(String name)
        {
            // if name is null, return to index
            if (name == null)
            {
                return RedirectToAction("Index");
            }

            // find code by name
            string query = "SELECT * FROM dbo.Code WHERE Code = @Code";
            CodeModel code = new CodeModel();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Code", name);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                
                while (reader.Read())
                {
                    code.Name = reader["Code"].ToString();
                    code.Description = reader["Description"].ToString();
                    code.Active = reader["Active"].ToString();
                    code.Category = reader["Category"].ToString();
                    code.LinkToCode = reader["LinkToCode"].ToString();
                }
                
                connection.Close();
                return View(code);
            }
        }

        [HttpPost]
        public string EditCode(CodeModel code)
        {
            string success;

            // update the code in database using Code name
            string query = "SELECT * FROM dbo.Code WHERE Code = @Code";

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@Code", code.Name);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Close();
                // update the code
                query = "UPDATE dbo.Code SET Code = @Code, Description = @Description, Active = @Active, Category = @Category, LinkToCode = @LinkToCode WHERE Code = @Code";
                cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Code", code.Name);
                cmd.Parameters.AddWithValue("@Description", code.Description);
                cmd.Parameters.AddWithValue("@Active", code.Active);
                cmd.Parameters.AddWithValue("@Category", code.Category);
                if (code.LinkToCode == null)
                {
                    cmd.Parameters.AddWithValue("@LinkToCode", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@LinkToCode", code.LinkToCode);
                }
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

        public ActionResult DeleteCode(String name)
        {
            // if name is null, return to index
            if (name == null)
            {
                return RedirectToAction("Index");
            }
            
            // delete the code using name
            string query = "DELETE FROM dbo.Code WHERE Code = @Code";

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@Code", name);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult GetAllCodes()
        {
            List<CodeModel> codeList = new List<CodeModel>();


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM dbo.Code", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    CodeModel code = new CodeModel();
                    code.Name = reader["Code"].ToString();
                    code.Description = reader["Description"].ToString();
                    code.Active = reader["Active"].ToString();
                    code.Category = reader["Category"].ToString();
                    code.LinkToCode = reader["LinkToCode"].ToString();
                    codeList.Add(code);
                }
                connection.Close();
            }
            return Json(codeList, JsonRequestBehavior.AllowGet);
        }
    }
}