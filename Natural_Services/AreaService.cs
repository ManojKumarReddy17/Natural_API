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
    public class AreaService : IAreaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AreaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Area>> GetAreasAsync()
        {
            return await _unitOfWork.AreaRepo.GetAllAsync();

        }

        public async Task<IEnumerable<Area>> GetAreasWithCityID(string CityId)
        {
            return await _unitOfWork.AreaRepo.GetAreasWithCityID(CityId);
        }


    }




}
