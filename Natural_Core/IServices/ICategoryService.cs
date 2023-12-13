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
        Task<CategoryResponse> CreateCategory(Category category);
        Task<Category> GetCategoryById(string CategoryId);
        Task<CategoryResponse> UpdateCategory(Category updatecategory);
        Task<CategoryResponse> DeleteCategory(string categoryId);


    }
}
