using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IRepositories
{
    public interface ICategoryRepository :IRepository<Category>
    {
        Task<IEnumerable<Category>> GetCatogeriesAsync();
        ValueTask<Category> GetWithCategoryByIdAsync(string Categoryid);

        Task<IEnumerable<Category>> UpdateCategoryById(string Id);
       // Task<Category> DeleteCategoryById(string Id);



    }
}
