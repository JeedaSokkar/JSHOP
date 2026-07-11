using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSHOP.DAL
{
   public  class ApplicationDbContexet:DbContext
    {
        public ApplicationDbContexet(DbContextOptions<ApplicationDbContexet> options) : base(options)
        {
        }
    }
}
