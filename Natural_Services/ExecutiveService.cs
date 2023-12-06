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
            var result = await _unitOfWork.ExecutiveRepo.GetAllExectivesAsync();
            return result;
        }

        public  async Task<Executive> GetDetailsById(string ExecutiveId)
            {
            return await _unitOfWork.ExecutiveRepo.GetWithExectiveByIdAsync(ExecutiveId);
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
                _unitOfWork.ExecutiveRepo.Update(existing);

                // Commit changes
                var created = await _unitOfWork.CommitAsync();

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
    
    }
}

  




