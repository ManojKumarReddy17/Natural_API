using Natural_Core;
using Natural_Core.IServices;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Services
{
    public class DsrService:IDsrService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DsrService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<RetailorResponce> CreateDsrWithAssociationsAsync(Dsr dsr)
        {
            var response = new RetailorResponce();
            try
            {
                dsr.Id = "DSR" + new Random().Next(10000, 99999).ToString();

                await _unitOfWork.dSRRepo.AddAsync(dsr);

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
    

        public async Task<IEnumerable<Dsr>> GetAllDsr()
        {
            var result = await _unitOfWork.dSRRepo.GetAllDsrAsync();
            return result;
        }

        
    }
}
