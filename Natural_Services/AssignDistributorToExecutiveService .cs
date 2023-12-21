using Natural_Core;
using Natural_Core.IServices;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Services
{
    public class AssignDistributorToExecutiveService : IAssignDistributorToExecutiveService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AssignDistributorToExecutiveService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<DistributorToExecutive>> AssignedDistributorDetailsByExecutiveId(string ExecutiveId)
        {
            return await _unitOfWork.distributorToExecutiveRepo.GetAssignedDistributorDetailsByExecutiveId(ExecutiveId);
        }

        public async Task<IEnumerable<DistributorToExecutive>> AssignedDistributorsByExecutiveId(string ExecutiveId)
        {
            return await _unitOfWork.distributorToExecutiveRepo.GetAssignedDistributorByExecutiveId(ExecutiveId);
        }


        public async Task<ResultResponse> AssignDistributorsToExecutive(DistributorToExecutive model)
        {
            var Response = new ResultResponse();

            try
            {
                var IsAssignedDistributor = await _unitOfWork.distributorToExecutiveRepo.
                  IsExecutiveAssignedToDistributor(new List<string> { model.DistributorId });

                if (!IsAssignedDistributor)
                {

                    var distributorToExecutive = new DistributorToExecutive
                    {
                        Id = "ADTE" + new Random().Next(1000, 9999).ToString(),
                        ExecutiveId = model.ExecutiveId,
                        DistributorId = model.DistributorId
                    };

                    await _unitOfWork.distributorToExecutiveRepo.AddAsync(distributorToExecutive);


                    var assigned = await _unitOfWork.CommitAsync();

                    if (assigned != 0)
                    {
                        Response.Message = "Successfully Assigned Distributors to Executive";
                        Response.StatusCode = 200;
                    }
                    else
                    {
                        Response.Message = "Failed Assigning Distributors to Executive";
                        Response.StatusCode = 404;
                    }
                }
                else
                {
                    Response.Message = "Executive is already assigned to the distributor";
                    Response.StatusCode = 400;
                }
            }

            catch (Exception)
            {
                Response.Message = "Failed Assigning Distributors to Executive";
                Response.StatusCode = 404;
            }

            return Response;
        }
    }
}