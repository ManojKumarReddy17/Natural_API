using Microsoft.Extensions.Logging;
using Natural_Core;
using Natural_Core.IServices;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Services
{
    public class ExecutiveGpsService : IExecutiveGpsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ExecutiveGpsService> _logger;


        public ExecutiveGpsService(IUnitOfWork unitOfWork, ILogger<ExecutiveGpsService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;


        }
     
        public async Task<IEnumerable<ExecutiveGp>> GetAllLatLung()
        {
            try
            {
                var result = await _unitOfWork.executiveGpsRepo.GetAllAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError("ExecutiveGpsService- GetAllLatLung", ex.Message);
                return null;

            }
        }
        public async Task<ExecutiveGp> GetExeId(string executiveId)
        {
            try
            {

            

            var notification = await _unitOfWork.executiveGpsRepo.GetByExeId(executiveId);
            return notification;
             }
              catch (Exception ex)
            {
                _logger.LogError("ExecutiveGpsService-  GetExeId{executiveId}", executiveId, ex.Message);
                return null;

            }
        }
        public async Task<ResultResponse> CreateOrUpdate(ExecutiveGp executive)
        {
            var response = new ResultResponse();
            try
            {



                try
                {

                    var existingExecutive = await _unitOfWork.executiveGpsRepo.GetByExeId(executive.ExecutiveId);

                    //  if (existingExecutive.ExecutiveId == executive.ExecutiveId)
                    if (existingExecutive != null)
                    {

                        existingExecutive.Latitude = executive.Latitude;
                        existingExecutive.Longitude = executive.Longitude;
                        _unitOfWork.executiveGpsRepo.Update(existingExecutive);
                    }
                    else
                    {
                        await _unitOfWork.executiveGpsRepo.AddAsync(executive);
                    }

                    var result = await _unitOfWork.CommitAsync();

                    if (result > 0)
                    {
                        response.Message = " Successful";
                        response.StatusCode = 200;
                    }
                }
                catch (Exception ex)
                {
                    response.Message = "An unexpected error occurred: " + ex.Message;
                    response.StatusCode = 500;
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("ExecutiveGpsService-  GetByExeId", ex.Message);
                return null;

            }
        }


    }
}
        
    



