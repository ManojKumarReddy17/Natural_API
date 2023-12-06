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
            catch (Exception ex)
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

        public async Task UpdateCategory(Category updatecategory)
        {
            //var response = new CategoryResponse();

            //try
            //{
            //    var existingCategory = await _unitOfWork.CategoryRepo.GetCategoryById(categoryId);

            //    if (existingCategory == null)
            //    {
            //        response.Message = "Category not found";
            //        response.StatusCode = 404;
            //    }
            //    else
            //    {
            //        existingCategory.CategoryName = category.CategoryName;
            //        // Update other properties as needed

            //        _unitOfWork.CategoryRepo.Update(categorytoUpdate);
            //        await _unitOfWork.CommitAsync();

            //        response.Message = "Update Successful";
            //        response.StatusCode = 200;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    response.Message = "Update Failed";
            //    response.StatusCode = 500;
            //}
            _unitOfWork.CategoryRepo.Update(updatecategory);
            await _unitOfWork.CommitAsync();
        }

        public async Task<CategoryResponse> DeleteCategory(string categoryId)
        {
            var response = new CategoryResponse();

            try
            {
                var existingCategory = await _unitOfWork.CategoryRepo.GetByIdAsync(categoryId);

                _unitOfWork.CategoryRepo.Remove(existingCategory);
                await _unitOfWork.CommitAsync();


                if (existingCategory == null)
                {
                    response.Message = "Category not found";
                    response.StatusCode = 404;
                }
                else
                {
                    response.Message = "Delete Successful";
                    response.StatusCode = 200;
                }
            }
            catch (Exception)
            {
                response.Message = "Delete Failed";
                response.StatusCode = 500;
            }

            return response;
        }



    }

}


