using Natural_Core;
using Natural_Core.IRepositories;
using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Data.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Services
{
    public class Retailor_To_Distributor_Service : IRetailor_To_Distributor_Service
    {
        private readonly IUnitOfWork _unitOfWork;

        public Retailor_To_Distributor_Service(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<RetailorToDistributor>> GetRetailorsIdByDistributorId(string distributorId)
        {
            return await _unitOfWork.Retailor_To_Distributor_RepositoryRepo.GetRetailorsIdByDistributorIdAsync(distributorId);
        }
        public async Task<IEnumerable<RetailorToDistributor>> GetRetailorsDetailsByDistributorId(string distributorId)
        {
            return await _unitOfWork.Retailor_To_Distributor_RepositoryRepo.GetAssignedRetailorDetailsByDistributorIdAsync(distributorId);
        }

        public async Task<ResultResponse> AssignRetailorsToDistributor(RetailorToDistributor retailorToDistributorlist)
        {
            var Response = new ResultResponse();

            try
            {
                var retailorTodistributor = new RetailorToDistributor()
                {
                    Id = "ARTD" + new Random().Next(1000, 9999).ToString(),
                    DistributorId = retailorToDistributorlist.DistributorId,
                    RetailorId = retailorToDistributorlist.RetailorId
                };

                await _unitOfWork.Retailor_To_Distributor_RepositoryRepo.AddAsync(retailorTodistributor);


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
            catch (Exception)
            {
                Response.Message = "Failed Assigning Distributors to Executive";
                Response.StatusCode = 404;
            }

            return Response;
        }
    }
 }




