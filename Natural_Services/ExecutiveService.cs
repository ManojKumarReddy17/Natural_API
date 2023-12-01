using Natural_Core;
using Natural_Core.IServices;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Services
{
    public class ExecutiveService :IExecutiveService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ExecutiveService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public  async Task<ExecutiveResponse> CreateExecutiveWithAssociationsAsync(Executive exec, string areaId, string cityId, string stateId)
        {
            var response = new ExecutiveResponse();

            try
            {
                // setting associated models (or) entities 

                exec.AreaNavigation = await _unitOfWork.AreaRepo.GetByIdAsync(areaId);
                exec.CityNavigation = await _unitOfWork.CityRepo.GetByIdAsync(cityId);
                exec.StateNavigation = await _unitOfWork.StateRepo.GetByIdAsync(stateId);

                // Adding distributor to the repository

                await _unitOfWork.ExecutiveRepo.AddAsync(exec);

                // Commit changes
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

        public async Task<IEnumerable<Executive>> GetAllExecutive()
        {
            var result = await _unitOfWork.ExecutiveRepo.GetAllExecutiveAsync();
            return result;
        }
    
    }
}
