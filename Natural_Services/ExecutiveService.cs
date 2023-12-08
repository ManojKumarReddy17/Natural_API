using Natural_Core;
using Natural_Core.IRepositories;
using Natural_Core.IServices;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<ExecutiveResponse> UpadateExecutive(Executive existing,Executive executive)
        {
            var response = new ExecutiveResponse();
            try
            {
                existing.FirstName = executive.FirstName;
                existing.LastName = executive.LastName;
                existing.MobileNumber = executive.MobileNumber;
                existing.Address = executive.Address;
                existing.Area = executive.Area;
                existing.City = executive.City;
                existing.State = executive.State;
                existing.Email = executive.Email;
                existing.UserName= executive.UserName;
                existing.Password= executive.Password;
                _unitOfWork.ExecutiveRepo.Update(existing);

                // Commit changes

                await _unitOfWork.CommitAsync();
                response.Message = "updatesuceesfull";
                    response.StatusCode = 200;
                }
            catch  (Exception ex)
            {
                response.Message = "Failed";
                response.StatusCode = 500;
        }

            return (response);
        }

        public async Task<ExecutiveResponse> CreateExecutiveWithAssociationsAsync(Executive executive)
        {
            {
                var response = new ExecutiveResponse();

                try
                {
                    
                    await _unitOfWork.ExecutiveRepo.AddAsync(executive);

               
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
        }

        public async Task<ExecutiveResponse> DeleteExecutive(string executiveId)
        {
            var response = new ExecutiveResponse();

            try
            {
                var exec = await _unitOfWork.ExecutiveRepo.GetByIdAsync(executiveId);

                if (exec != null)
                {
                    _unitOfWork.ExecutiveRepo.Remove(exec);
                    await _unitOfWork.CommitAsync();
                    response.Message = "SUCCESSFULLY DELETED";
                }
                else
                {
                    response.Message = "RETAILER NOT FOUND";
                }
            }
            catch (Exception)
            {
                response.Message = "Internal Server Error";
            }

            return response;
        } 
    }
}

  




