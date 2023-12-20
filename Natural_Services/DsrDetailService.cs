using Natural_Core;
using Natural_Core.IServices;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Natural_Services
{
    public class DsrDetailService : IDSRdetailsService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DsrDetailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
       public async Task<RetailorResponce> CreateDsrDetailsWithAssociationsAsync(Dsrdetail dsr)
        {
            var response = new RetailorResponce();
            try
            {

                dsr.Id = "DSRD" + new Random().Next(10000, 99999).ToString();


                await _unitOfWork.DSRDetailsRepo.AddAsync(dsr);
                var created = await _unitOfWork.CommitAsync();
                if(created !=0)
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

        public async Task<IEnumerable<Dsrdetail>> GetAllDsrdetail()
        {
            var result = await _unitOfWork.DSRDetailsRepo.GetAllDsrdetailsAsync();
            return result;
            
        }
    }
}
