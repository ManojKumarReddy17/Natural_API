using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace Natural_Services
{
    public class RetailorService : IRetailorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RetailorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Retailor>> GetAllRetailors()
        {
            var result = await _unitOfWork.RetailorRepo.GetAllRetailorsAsync();
            return result;
        }
        public async Task<Retailor> GetRetailorById(string distributorId)
        {
            return await _unitOfWork.RetailorRepo.GetWithRetailorsByIdAsync(distributorId);
        }

        public async Task<RetailorResponce> CreateRetailorWithAssociationsAsync(Retailor retailor)
        {
            var response = new RetailorResponce();

            try
            {
              
                await _unitOfWork.RetailorRepo.AddAsync(retailor);

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
}