using Natural_Core;
using Natural_Core.IServices;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Natural_Services
{
    public class AssignRetailorToDistributorService : IAssignRetailorToDistributorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AssignRetailorToDistributorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<RetailorToDistributor>> GetRetailorsIdByDistributorId(string distributorId)
        {
            return await _unitOfWork.RetailorToDistributorRepositoryRepo.GetRetailorsIdByDistributorIdAsync(distributorId);
        }
        public async Task<IEnumerable<RetailorToDistributor>> GetRetailorsDetailsByDistributorId(string distributorId)
        {
            return await _unitOfWork.RetailorToDistributorRepositoryRepo.GetAssignedRetailorDetailsByDistributorIdAsync(distributorId);
        }

       
        public async Task<ResultResponse> AssignRetailorsToDistributor(RetailorToDistributor retailorToDistributorlist)
        {
            var Response = new ResultResponse();

            try
            {
                var AssignedRetailor = await _unitOfWork.RetailorToDistributorRepositoryRepo.
                  DistributorAssignedToRetailor(new List<string> { retailorToDistributorlist.RetailorId });

                if (!AssignedRetailor)
                {

                    var retailorToDistributor = new RetailorToDistributor
                    {
                        Id = "ARTD" + new Random().Next(1000, 9999).ToString(),
                        DistributorId = retailorToDistributorlist.DistributorId,
                        RetailorId = retailorToDistributorlist.RetailorId
                    };

                    await _unitOfWork.RetailorToDistributorRepositoryRepo.AddAsync(retailorToDistributor);


                    var assigned = await _unitOfWork.CommitAsync();

                    if (assigned != 0)
                    {
                        Response.Message = "Successfully Assigned Retailor To Distributor";
                        Response.StatusCode = 200;
                    }
                    else
                    {
                        Response.Message = "Failed Assigning Retailor To Distributor";
                        Response.StatusCode = 404;
                    }
                }
                else
                {
                    Response.Message = "Retailor is already assigned to the distributor";
                    Response.StatusCode = 400;
                }
            }

            catch (Exception)
            {
                Response.Message = "Failed Assigning Retailor To Distributor";
                Response.StatusCode = 404;
            }

            return Response;
        }
    }
 }




