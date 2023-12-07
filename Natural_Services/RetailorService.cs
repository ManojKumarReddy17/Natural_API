using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Natural_Core.IRepositories;
using Natural_Data.Repositories;

#nullable disable

namespace Natural_Services
{
    public class RetailorService : IRetailorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRetailorRepository _repository;


        public RetailorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<IEnumerable<Retailor>> GetAllRetailors()
        {
            var result = await _unitOfWork.RetailorRepo.GetAllRetailorsAsync();
            return result;
        }
        public async Task<Retailor> GetRetailorById(string retailorId)
        {
            return await _unitOfWork.RetailorRepo.GetWithRetailorsByIdAsync(retailorId);
        }
        public async Task<Retailor> GetRetailorsById(string retailorId)
        {
            return await _unitOfWork.RetailorRepo.GetByIdAsync(retailorId);
        }

        public async Task<RetailorResponce> CreateRetailorWithAssociationsAsync(Retailor retailor, string areaId, string cityId, string stateId)
        {
            var response = new RetailorResponce();

            try
            {

                retailor.AreaNavigation = await _unitOfWork.AreaRepo.GetByIdAsync(areaId);
                retailor.CityNavigation = await _unitOfWork.CityRepo.GetByIdAsync(cityId);
                retailor.StateNavigation = await _unitOfWork.StateRepo.GetByIdAsync(stateId);


                await _unitOfWork.RetailorRepo.AddAsync(retailor);

                var created = await _unitOfWork.CommitAsync();

                if (created != null)
                {
                    response.Message = "Insertion Successful";
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



        public async Task<RetailorResponce> DeleteRetailor(string retailorId)
        {
            var response = new RetailorResponce();

            try
            {
                var retailor = await _unitOfWork.RetailorRepo.GetByIdAsync(retailorId);

                if (retailor!= null)
                {
                    _unitOfWork.RetailorRepo.Remove(retailor);
                    await _unitOfWork.CommitAsync();
                    response.Message = "SUCCESSFULLY DELETED";
                }
                else
                {
                    response.Message = "RETAILER NOT FOUND";
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
 
