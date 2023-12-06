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
    public class DistributorService : IDistributorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DistributorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Distributor>> GetAllDistributors()
        {
            var result = await _unitOfWork.DistributorRepo.GetAllDistributorstAsync();
            return result;
        }


        // Get Distributor by Id
        public async Task<Distributor> GetDistributorById(string distributorId)
        {
            return await _unitOfWork.DistributorRepo.GetWithDistributorsByIdAsync(distributorId);
        }

        //Create Distributor
        public async Task<DistributorResponse> CreateDistributorWithAssociationsAsync(Distributor distributor)
        {
            var response = new DistributorResponse();

            try
            {

                await _unitOfWork.DistributorRepo.AddAsync(distributor);

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
        public async Task<DistributorResponse> DeleteDistributor(string distributorId)
        {
            var response = new DistributorResponse();

            try
            {
                var distributor = await _unitOfWork.DistributorRepo.GetByIdAsync(distributorId);

                if (distributor != null)
                {
                    _unitOfWork.DistributorRepo.Remove(distributor);
                    await _unitOfWork.CommitAsync();
                    response.Message = "SUCCESSFULLY DELETED";
                }
                else
                {
                    response.Message = "DISTRIBUTOR NOT FOUND";
                }
            }
            catch (Exception ex)
            {
                response.Message = "Internal Server Error";
            }

            return response;
        }
    }
}
