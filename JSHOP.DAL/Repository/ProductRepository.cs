using JSHOP.DAL.Data;
using JSHOP.DAL.Models;

namespace JSHOP.DAL.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        

            public ProductRepository(ApplicationDbContexet context) : base(context)
            {

            }

        
    }

}

