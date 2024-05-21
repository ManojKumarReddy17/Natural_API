  using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Natural_Core.S3Models;

#nullable disable

namespace Natural_Services
{
    public class CityService : ICityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<City>> GetCitiesAsync(string? StateId)
        {
            var result = await _unitOfWork.CityRepo.GetAllAsync();
            var presentcity = result.Where(c => c.IsDeleted == false).ToList();
            if(StateId != null)
            {
                presentcity = presentcity.Where(c =>c.StateId == StateId).ToList();
            }
            return presentcity;

        }

        public async Task<City> GetCityWithId(string CityId)
        {
            return await _unitOfWork.CityRepo.GetCityWithId(CityId);
        }

        public async Task<ProductResponse> InsertWithCity(City city)
        {
            var response = new ProductResponse();
            try
            {

                city.Id = "ctn" + new Random().Next(10000, 99999).ToString();
                await _unitOfWork.CityRepo.AddAsync(city);
                var Created = await _unitOfWork.CommitAsync();
                if (Created != 0)
                {
                    response.Message = "Insertion Successful";
                    response.StatusCode = 200;

                    response.Id = city.Id;
                }



            }
            catch (Exception)
            {
                response.Message = "Insertion Failed";
                response.StatusCode = 401;
            }
            return response;

        }
        public async Task<ProductResponse> UpdateWithCity(City UpdateWithCity)
        {
            var response = new ProductResponse();
            try
            {
                _unitOfWork.CityRepo.Update(UpdateWithCity);
                var updated = await _unitOfWork.CommitAsync();
                if (updated != 0)
                {
                    response.Message = "Update Successful";
                    response.StatusCode = 200;


                }
            }
            catch (Exception)
            {
                response.Message = "Update Failed";
                response.StatusCode = 401;
            }
            return response;



        }


        public async Task<ResultResponse> DeleteCity(string CityId)
        {
            var response = new ResultResponse();

            try
            {
                var existingCity = await _unitOfWork.CityRepo.GetByIdAsync(CityId);
                existingCity.IsDeleted = true;
                _unitOfWork.CityRepo.Update(existingCity);
                await _unitOfWork.CommitAsync();

                if (existingCity == null)
                {
                    response.Message = "City not found";
                    response.StatusCode = 404;
                }
                else
                {
                    response.Message = "Delete Successful";
                    response.StatusCode = 200;
                }
            }
            catch (Exception)
            {
                response.Message = "Delete Failed";
                response.StatusCode = 500;
            }

            return response;
        }
    }
}