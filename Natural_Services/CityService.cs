﻿  using Natural_Core.IServices;
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
    public class CityService : ICityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CityService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //public async Task<IEnumerable<City>> GetCitiesAsync()
        //{

        //    return await _unitOfWork.CityRepo.GetAllAsync();

        //}
        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            var result = await _unitOfWork.CityRepo.GetAllAsync();
            var presentcity = result.Where(c => c.IsDeleted == false).ToList();
            return presentcity;

        }

        //public async Task<City> GetCityWithId(string CityId)
        //{
        //    return await _unitOfWork.CityRepo.GetCityWithStateId(CityId);  
        //}

       

        public async Task<City> GetCityWithId(string CityId)
        {
            return await _unitOfWork.CityRepo.GetCityWithId(CityId);
        }

        public async Task<IEnumerable<City>> GetCitywithStateId(string StateId)
        {
            return await _unitOfWork.CityRepo.GetCitywithStateId(StateId);
        }
    }
}