using Natural_Core;
using Natural_Core.IRepositories;
using Natural_Core.IServices;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Natural_Services
{
    public class ExecutiveService : IExecutiveService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ExecutiveService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public  async Task<IEnumerable<Executive>> GetAllExecutives()
        {
            var result = await _unitOfWork.ExecutiveRepo.GetAllExecutiveAsync();
            return result;
        }

        public  async Task<Executive> GetExecutiveDetailsById(string DetailsId)
            {
            return await _unitOfWork.ExecutiveRepo.GetWithExectiveByIdAsync(DetailsId);
        }

        public async Task<Executive> GetExecutiveById(string ExecutiveId)
        {
            return await _unitOfWork.ExecutiveRepo.GetByIdAsync(ExecutiveId);
        }

        public async Task<ResultResponse> UpadateExecutive(Executive executive)
        {
            var response = new ResultResponse();
            try
            {
                _unitOfWork.ExecutiveRepo.Update(executive);                             
               var updated = await _unitOfWork.CommitAsync();
                if (updated != 0)
                {

                    response.Message = "updatesuceesfull";
                    response.StatusCode = 200;
                }
            }
            catch  (Exception)
            {
                response.Message = "Failed";
                response.StatusCode = 500;
        }

            return (response);
        }

        public async Task<ResultResponse> CreateExecutiveWithAssociationsAsync(Executive executive)
        {
            {
                var response = new ResultResponse();

                try
                {

                    executive.Id = "NEXE" + new Random().Next(10000, 99999).ToString();


                    await _unitOfWork.ExecutiveRepo.AddAsync(executive);

               
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
        }

        public async Task<ResultResponse> DeleteExecutive(string executiveId)
        {
            var response = new ResultResponse();

            try
            {
                var exec = await _unitOfWork.ExecutiveRepo.GetByIdAsync(executiveId);

                if (exec != null)
                {
                    _unitOfWork.ExecutiveRepo.Remove(exec);
                    await _unitOfWork.CommitAsync();
                    response.Message = "Successfully Deleted";
                    response.StatusCode = 200;
                }
                else
                {
                    response.Message = "Executive Not Found";
                    response.StatusCode = 404;
                }
            }
            catch (Exception)
            {
                response.Message = "Internal Server Error";
            }

            return response;
        }

        public async Task<IEnumerable<Executive>> SearchExecutives(SearchModel search)
        {
            var exec = await _unitOfWork.ExecutiveRepo.SearchExecutiveAsync(search);
            return exec;
        }
    }
}

  




