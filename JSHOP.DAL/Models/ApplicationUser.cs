using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSHOP.DAL.Models
{
   public class ApplicationUser : IdentityUser


    {
        public string FullName { get; set; }
        public string ? City { get; set; }
        public string? Country { get; set; }
    }
}
