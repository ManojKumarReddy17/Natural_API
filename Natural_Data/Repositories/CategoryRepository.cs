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
        private NaturalsContext NaturalDbContext
        {
            get { return Context as NaturalsContext; }
        }
       
        

        public async ValueTask<Category> GetWithCategoryByIdAsync(string Categoryid)

        {
            return await NaturalDbContext.Categories.FindAsync(Categoryid);
        }

        public Task<IEnumerable<Category>> UpdateCategoryById(string Id)
        {
            throw new NotImplementedException();
        }
    }

}


