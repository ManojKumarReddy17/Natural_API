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
        public async Task<IEnumerable<Distributor>> AssignedDistributorDetailsByExecutiveId(string ExecutiveId)
        {
            return await _unitOfWork.distributorToExecutiveRepo.GetAssignedDistributorDetailsByExecutiveIdAsync(ExecutiveId);
        }

        public async Task<IEnumerable<DistributorToExecutive>> AssignedDistributorsByExecutiveId(string ExecutiveId)
        {
            return await _unitOfWork.distributorToExecutiveRepo.GetAssignedDistributorByExecutiveIdAsync(ExecutiveId);
        }


        public async Task<ResultResponse> AssignDistributorsToExecutive(DistributorToExecutive model)
        {
            var Response = new ResultResponse();

            try
            {
                var IsAssignedDistributor = await _unitOfWork.distributorToExecutiveRepo.
                  IsDistirbutorAssignedToExecutiveAsync(new List<string> { model.DistributorId });



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

        public async Task<ResultResponse> DeleteAssignedDistributorByid(string distributorId,string ExecutiveId)
        {
            var response = new ResultResponse();
            try
            {
                var distributor = await _unitOfWork.distributorToExecutiveRepo.DeleteDistributorAsync(distributorId,ExecutiveId);

                if (distributor != null)
                {
                    _unitOfWork.distributorToExecutiveRepo.Remove(distributor);
                    await _unitOfWork.CommitAsync();
                    response.Message = "SUCCESSFULLY DELETED";
                    response.StatusCode = 200;
                }
                else
                {
                    response.Message = "DISTRIBUTOR NOT FOUND";
                    response.StatusCode = 404;
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
