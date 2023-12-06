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

        public async Task<Distributor> GetById(string distributorId)
        {
            return await _unitOfWork.DistributorRepo.GetByIdAsync(distributorId);
        }
       

        //Create Distributor
        public async Task<DistributorResponse> CreateDistributorWithAssociationsAsync(Distributor distributor, string areaId, string cityId, string stateId)
        {
            var response = new DistributorResponse();

            try
            {
                // setting associated models (or) entities 

                distributor.AreaNavigation = await _unitOfWork.AreaRepo.GetByIdAsync(areaId);
                distributor.CityNavigation = await _unitOfWork.CityRepo.GetByIdAsync(cityId);
                distributor.StateNavigation = await _unitOfWork.StateRepo.GetByIdAsync(stateId);

                // Adding distributor to the repository

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

        //public async Task<Distributor> GetById(update)
        //{
        //  var distributer=  await _unitOfWork.DistributorRepo.update(distributorId);
        //    return distributer;
        //}

       

        public async Task UpdateDistributor(Distributor distributor)
        {
            _unitOfWork.DistributorRepo.Update(distributor);
            await _unitOfWork.CommitAsync();
        }

        //public Task UpdateDistributor(Distributor DistributorToBeUpdates, Distributor distributor)
        //{
        //    throw new NotImplementedException();
        //}
    }
    }
