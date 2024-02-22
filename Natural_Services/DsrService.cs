using Natural_Core;
using Natural_Core.IServices;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Natural_Services
{

    public class DsrService:IDsrService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAssignDistributorToExecutiveService _distributorToExecutiveService;
        private readonly IDsrdetailService _DsrdetailService;
        public DsrService(IUnitOfWork unitOfWork, IAssignDistributorToExecutiveService distributorToExecutiveService, IDsrdetailService DsrdetailService)
        {
            _unitOfWork = unitOfWork;
            _distributorToExecutiveService = distributorToExecutiveService;
            _DsrdetailService = DsrdetailService;
        }

        public async Task<ResultResponse> CreateDsrWithAssociationsAsync(Dsr dsr, List<Dsrdetail> dsrdetails)
        {
            var response = new ResultResponse();
            try
            {
                dsr.Id = "DSR" + new Random().Next(10000, 99999).ToString();


                await _unitOfWork.dSRRepo.AddAsync(dsr);



                var created = await _unitOfWork.CommitAsync();

                if (created != 0)
                {
                    var dsrdetailinertion = await _DsrdetailService.CreateDsrdetail(dsrdetails, dsr.Id);

                    if (dsrdetailinertion.StatusCode == 200)
                    {

                        response.Message = " Dsr and DsrdetailInsertion Successful";
                        response.StatusCode = 200;
                    }
                    else
                    {
                        response.Message = "DsrdetailInsertion Failed";
                        response.StatusCode = 401;

                    }

                }
                else
                {

                    response.Message = " Dsr Insertion Successful";
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

        public async Task<IEnumerable<Dsr>> GetAllDsr()
        {
            var result = await _unitOfWork.dSRRepo.GetAllDsrAsync();
            return result;
        }


       
        public async Task<Dsr> GetDsrDetailsById(string DsrId)
        {
            return await _unitOfWork.dSRRepo.GetDetails(DsrId);
        }

        public async Task<DsrResponse> DeleteDsr(string dsrId)
        {
            var response= new DsrResponse();

            try
            {
                var dsr = await _unitOfWork.dSRRepo.GetByIdAsync(dsrId);
                if (dsr != null)
                {
                    _unitOfWork.dSRRepo.Remove(dsr);
                    await _unitOfWork.CommitAsync();
                    response.Message = "SUCCESSFULLY DELETED";
                    response.StatusCode = 200;
                }
                else
                {
                    response.Message = "FAILED";
                    response.StatusCode = 400;
                }
            }
            catch(Exception ) 
            {
                response.Message = "NOT FOUND";
            }

            return response;
             
        }



        public async Task<IEnumerable<DsrDistributor>> AssignedDistributorDetailsByExecutiveId(string ExecutiveId)
        {

            var result = await _unitOfWork.dSRRepo.GetAssignedDistributorDetailsByExecutiveId(ExecutiveId);
            
            return result;


        }

        public async Task<IEnumerable<DsrRetailor>> GetAssignedRetailorDetailsByDistributorId(string DistributorId)
        {

            var result = await _unitOfWork.dSRRepo.GetAssignedRetailorDetailsByDistributorId(DistributorId);

            return result;


        }


        public async Task<IEnumerable<Product>> GetProductAsync()
        {
            return await _unitOfWork.ProductRepository.GetProducttAsync();
        }

        public async Task<IEnumerable<Dsr>> SearchDsr(Dsr search)
        {
          var searchedDsr =  await _unitOfWork.dSRRepo.SearchDsr(search);

            return searchedDsr;
        }



    }
}
