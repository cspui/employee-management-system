using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewTest.Models
{
    public class CodeModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Active { get; set; }
        public string Category { get; set; } // department or occupation 
        public string LinkToCode { get; set; } 
        // bond to a Department Code if Category is occupation 
        
        public CodeModel()
        {

        }
    }
}
