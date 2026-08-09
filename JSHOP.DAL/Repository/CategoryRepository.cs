using JSHOP.DAL.Data;
using JSHOP.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSHOP.DAL.Repository
{
    public class CategoryRepository :GenericRepository<Category>,ICategoryRepository
    {

        public CategoryRepository(ApplicationDbContexet context) : base(context)
        {

        }
     
    }
}
