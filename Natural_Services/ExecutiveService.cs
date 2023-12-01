using Natural_Core;
<<<<<<< HEAD
=======
using Natural_Core.IRepositories;
>>>>>>> c58358f3903f29e537fa003d6294fb2aae3176fa
using Natural_Core.IServices;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Services
{
<<<<<<< HEAD
    public class ExecutiveService :IExecutiveService
=======
    public class ExecutiveService : IExecutiveService
>>>>>>> c58358f3903f29e537fa003d6294fb2aae3176fa
    {
        private readonly IUnitOfWork _unitOfWork;
        public ExecutiveService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

<<<<<<< HEAD
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
    
=======
        public  async Task<ExecutiveResponse> CreateExecutiveWithAssociationsAsync(Executive executive, string areaId, string cityId, string stateId)
        {
            {
                var response = new ExecutiveResponse();

                try
                {
                    // setting associated models (or) entities 

                    executive.AreaNavigation = await _unitOfWork.AreaRepo.GetByIdAsync(areaId);
                    executive.CityNavigation = await _unitOfWork.CityRepo.GetByIdAsync(cityId);
                    executive.StateNavigation = await _unitOfWork.StateRepo.GetByIdAsync(stateId);

                    // Adding distributor to the repository

                    await _unitOfWork.ExecutiveRepo.AddAsync(executive);

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
        }

        public  async Task<IEnumerable<Executive>> GetAllExecutives()
        {
            var result = await _unitOfWork.ExecutiveRepo.GetAllExectivesAsync();
            return result;
        }

        public  async Task<Executive> GetExecutiveById(string ExecutiveId)
        {
            return await _unitOfWork.ExecutiveRepo.GetWithExectiveByIdAsync(ExecutiveId);
        }
>>>>>>> c58358f3903f29e537fa003d6294fb2aae3176fa
    }
}
