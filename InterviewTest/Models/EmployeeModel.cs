using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewTest.Models
{
    public class EmployeeModel
    {
        // Name, DOB (date picker), Gender (Radio), Nationality (dropdown), IC No
        [Required]
        public string Name { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string Nationality { get; set; }
        public string IC { get; set; }

        // Emp No, Hired Date(date picker), Department(dropdown), Occupation(dropdown), Category
        public string EmpNo { get; set; }
        public string HiredDate { get; set; }
        public string Department { get; set; }
        public string Occupation { get; set; }
        public string Category { get; set; }

        public EmployeeModel()
        {

        }
    }
}
