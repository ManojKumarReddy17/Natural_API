using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Core.IServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategories();
        Task<ResultRepsonse> CreateCategory(Category category);
        Task<Category> GetCategoryById(string CategoryId);
        Task<ResultRepsonse> UpdateCategory(Category updatecategory);
        Task<ResultRepsonse> DeleteCategory(string categoryId);


    }
}
