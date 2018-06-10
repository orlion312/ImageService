using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    /// <summary>
    /// the class is to present the students detailes
    /// </summary>
    public class Student
    {

        //the ID of the student
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ID")]
        public string ID { get; set; }

        //the first name of the student
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        //the last name of the student
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "LastName")]
        public string LastName { get; set; }
    }
}