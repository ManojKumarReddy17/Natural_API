using Natural_Core;
using Natural_Core.IServices;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Services
{
    public class ExecutiveGpsService : IExecutiveGpsService
    {
        private readonly IUnitOfWork _unitOfWork;


        public ExecutiveGpsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

 
        
        public async Task<ResultResponse> CreateOrUpdate(ExecutiveGp executive)
        {
            var response = new ResultResponse();

            try
            {
               
                var existingExecutive = await _unitOfWork.executiveGpsRepo.GetByExeId( executive.ExecutiveId);

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
                //else
                //{
                //    response.Message = "No changes were made";
                //    response.StatusCode = 204; 
                //}
            }
            catch (Exception ex)
            {
                response.Message = "An unexpected error occurred: " + ex.Message;
                response.StatusCode = 500; 
            }

            return response;
        }


    }
}
        
    



