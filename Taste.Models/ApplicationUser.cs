using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.ExceptionServices;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Taste.Models
{
    public class ApplicationUser : IdentityUser
    {

        [Display(Name="Full Name")]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }
}
