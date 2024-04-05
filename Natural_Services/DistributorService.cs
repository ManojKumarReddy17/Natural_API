using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;


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
            var presentDistributors = result.Where(d => d.IsDeleted != true ).ToList();
            return presentDistributors;
        }


        public async Task<IEnumerable<Distributor>> GetNonAssignedDistributors()
        {
            var result = await _unitOfWork.DistributorRepo.GetNonAssignedDistributorsAsync();
            return result;
        }




        public async Task<Distributor> GetDistributorById(string distributorId)
        {
            var result = await _unitOfWork.DistributorRepo.GetByIdAsync(distributorId);
            if(result.IsDeleted == false)
            {
                return result;
            }
            return null;
        }

        public async Task<Distributor> GetDistributorDetailsById(string distributorId)
        {
            return await _unitOfWork.DistributorRepo.GetDistributorDetailsByIdAsync(distributorId);
        }

        public async Task<ResultResponse> CreateDistributorWithAssociationsAsync(Distributor distributor)
        {
            var response = new ResultResponse();

            try
            {
                distributor.Id = "NDIS" + new Random().Next(10000, 99999).ToString();

                await _unitOfWork.DistributorRepo.AddAsync(distributor);

                var created = await _unitOfWork.CommitAsync();

                if (created != 0)
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

        public async Task<ResultResponse> UpdateDistributor(Distributor distributor)

        {
            var response = new ResultResponse();
            try
            {
                _unitOfWork.DistributorRepo.Update(distributor);
                var created = await _unitOfWork.CommitAsync();
                if (created != 0)
                {
                    response.Message = "update Successful";
                    response.StatusCode = 200;
                }
            }
            catch (Exception)
            {

                response.Message = "update Failed";
                response.StatusCode = 401;
            }

            return response;
        }
        public async Task<ResultResponse> DeleteDistributor(string distributorId)
        {
            var response = new ResultResponse();

            try
            {
                var distributor = await _unitOfWork.DistributorRepo.GetByIdAsync(distributorId);

                if (distributor != null)
                {
                    _unitOfWork.DistributorRepo.Update(distributor);
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

        public async Task<IEnumerable<Distributor>> SearchDistributors(SearchModel search)
        {
            var distributors = await _unitOfWork.DistributorRepo.SearchDistributorAsync(search);
            return distributors;
        }

        public async Task<IEnumerable<Distributor>> SearchNonAssignedDistributors(SearchModel search)
        {
            var searchdistributors = await _unitOfWork.DistributorRepo.SearchNonAssignedDistributorsAsync(search);
            return searchdistributors;
        }
        public async Task<AngularLoginResponse> LoginAsync(Distributor credentials)
        {
            AngularLoginResponse response = new AngularLoginResponse();
            try
            {
                var user = await _unitOfWork.DistributorRepo.GetAllAsync();

                var authenticatedUser = user.FirstOrDefault(u => u.UserName == credentials.UserName && u.Password == credentials.Password);


                if (authenticatedUser != null)
                {
                    response.Id = authenticatedUser.Id;
                    response.FirstName = authenticatedUser.FirstName;
                    response.LastName = authenticatedUser.LastName;
                    response.Email = authenticatedUser.Email;
                    response.Address = authenticatedUser.Address;
                    response.MobileNumber = authenticatedUser.MobileNumber;
                  //  response.Executive = authenticatedUser.DistributorToExecutives.ToList();
                    
                    response.Statuscode = 200;
                    response.Message = "LOGIN SUCCESSFUL";
                    return response;
                }
                else
                {
                    response.Statuscode = 401;
                    response.Message = "INVALID CREDENTIALS";
                    return response;

                }


            }

            catch (Exception)
            {
                response.Message = "INTERNAL SERVER ERROR";
                response.Statuscode = 500;
                return response;
            }



        }



        public async Task<ResultResponse> SoftDelete(string distributorId)
        {
            var response = new ResultResponse();

            try
            {
                var distributor = await _unitOfWork.DistributorRepo.GetByIdAsync(distributorId);
                
                
                if (distributor != null)
                {
                    
                    distributor.IsDeleted = true;
                    _unitOfWork.DistributorRepo.Update(distributor);
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