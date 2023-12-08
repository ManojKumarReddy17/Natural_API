using Natural_Core;
using Natural_Core.IServices;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


#nullable disable
namespace Natural_Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryResponse> CreateCategory(Category category)
        {
            var response = new CategoryResponse();
            try
            {

                await _unitOfWork.CategoryRepo.AddAsync(category);
                var created = await _unitOfWork.CommitAsync();

                if (created != null)
                {
                    response.Message = "Insertion Successful";
                    response.StatusCode = 200;
                }
            }
            catch (Exception)
            {

                response.Message = "Insertion Failed";
                response.StatusCode = 401;
            }

            return response;
        }


        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            var result = await _unitOfWork.CategoryRepo.GetAllAsync();
            return result;
        }

        public async Task<Category> GetCategoryById(string CategoryId)
        {
            return await _unitOfWork.CategoryRepo.GetByIdAsync(CategoryId);

        }

    }
}

