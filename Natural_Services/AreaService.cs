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
    public class AreaService : IAreaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AreaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Area>> GetAreasAsync(string? CityId)
        {
            var result = await _unitOfWork.AreaRepo.GetAllAsync();
            var presentArea = result.Where(c => c.IsDeleted == false).ToList();
            if(CityId != null)
            {
                presentArea = presentArea.Where(c => c.CityId == CityId).ToList();
            }
            return presentArea;



        }
        public async Task<ProductResponse> Insert(Area area)
        {
            var response = new ProductResponse();
            try
            {

                area.Id = "arn" + new Random().Next(10000, 99999).ToString();
                await _unitOfWork.AreaRepo.AddAsync(area);
                var created = await _unitOfWork.CommitAsync();
                if (created != 0)
                {
                    response.Message = "Insertion Successful";
                    response.StatusCode = 200;
                    response.Id = area.Id;
                    
                }
                
            }
            catch (Exception)
            {

                response.Message = "Insertion Failed";
                response.StatusCode = 401;
            }

            return response;
        }

       

        public async Task<ProductResponse> updateArea(Area updateArea)
        {
            var response = new ProductResponse();
            try
            {
                _unitOfWork.AreaRepo.Update(updateArea);
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
        public async Task<ProductResponse> DeleteArea(String Id)
        {
            var response = new ProductResponse();
            try
            {
                var existingarea = await _unitOfWork.AreaRepo.GetByIdAsync(Id);
                existingarea.IsDeleted = true;
                _unitOfWork.AreaRepo.Update(existingarea);
                await _unitOfWork.CommitAsync();
                if (existingarea == null)
                {
                    response.Message = "Area not Found";
                    response.StatusCode = 404;
                }
                else
                {
                    response.Message = "Delete Successfull";
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

        public async Task<Area> GetAreasWithId(string AreaId)
        {
            return await _unitOfWork.AreaRepo.GetAreasId(AreaId);
        }

    }
}
