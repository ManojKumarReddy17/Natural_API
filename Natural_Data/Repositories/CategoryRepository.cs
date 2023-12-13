using Microsoft.EntityFrameworkCore;

using Natural_Core;
using Natural_Core.IRepositories;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

#nullable disable
namespace Natural_Data.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(NaturalsContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Category>> GetCatogeriesAsync()
        {
            return await NaturalDbContext.Categories.ToListAsync();
        }
        public async ValueTask<Category> GetWithCategoryByIdAsync(string CategoryId)

        {
            return await NaturalDbContext.Categories.FindAsync(CategoryId);
        }

   
        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }



    }
}


