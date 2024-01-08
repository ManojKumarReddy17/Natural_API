using Microsoft.EntityFrameworkCore;
using Natural_Core;
using Natural_Core.IServices;
using Natural_Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Linq;
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

        public async Task<ResultResponse> CreateDsrWithAssociationsAsync(Dsr dsr)
        {
            var response = new ResultResponse();
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

       
        public async Task<Dsr> GetDsrDetailsById(string DsrId)
        {
            return await _unitOfWork.dSRRepo.GetDetails(DsrId);
        }
        public async Task<DsrResponse> DeleteDsr(string dsrId)
        {
            var response= new DsrResponse();

            try
            {
                var dsr = await _unitOfWork.dSRRepo.GetByIdAsync(dsrId);
                if (dsr != null)
                {
                    _unitOfWork.dSRRepo.Remove(dsr);
                    await _unitOfWork.CommitAsync();
                    response.Message = "SUCCESSFULLY DELETED";
                    response.StatusCode = 200;
                }
                else
                {
                    response.Message = "FAILED";
                    response.StatusCode = 400;
                }
            }
            catch(Exception ) 
            {
                response.Message = "NOT FOUND";
            }

            return response;
             
        }

    }
}
