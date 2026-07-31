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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContexet _context;
        public CategoryRepository(ApplicationDbContexet context)
        {
            _context = context;
        }
        async Task<Category> ICategoryRepository.CreateAsync(Category category)
        {
           await _context.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;
        }

        async Task<List<Category>> ICategoryRepository.GetAllAsync()
        {
            var categories =await  _context.Categories.Include(c => c.Translations).ToListAsync();
            return categories;
        }
    }
}
