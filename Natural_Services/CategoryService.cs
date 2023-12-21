﻿using Natural_Core;
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

        public async Task<ResultRepsonse> CreateCategory(Category category)
        {
            var response = new ResultRepsonse();
            try
            {
                category.Id = "NCAT" +new Random().Next(10000,99999).ToString();

                await _unitOfWork.CategoryRepo.AddAsync(category);
                var created = await _unitOfWork.CommitAsync();

                if (created != 0)
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

        public async Task<ResultRepsonse> UpdateCategory(Category updatecategory)
        {
            var response = new ResultRepsonse();
            try
            {
                _unitOfWork.CategoryRepo.Update(updatecategory);
                var updated = await _unitOfWork.CommitAsync();

                if (updated != 0)
                {
                    response.Message = "Update Successful";
                    response.StatusCode = 200;

                }
            }
            catch (Exception)
            {

                response.Message = "Update Failed";
                response.StatusCode = 401;
            }

            return response;
        }

        public async Task<ResultRepsonse> DeleteCategory(string categoryId)
        {
            var response = new ResultRepsonse();

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


