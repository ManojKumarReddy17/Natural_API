using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Natural_Core;
using Natural_Core.IServices;
using Natural_Core.Models;

namespace Natural_Services
{
	public class DsrdetailService : IDsrdetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public DsrdetailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
           
        }

        public async Task<ResultResponse> CreateDsrdetail(List<Dsrdetail> dsr, string id)
        {
            var response = new ResultResponse();
            try
            {

        
                string dsrid = id;
            
                var create = dsr.Select(c => new Dsrdetail
                {
                 
                    Product = c.Product,
                    Quantity = c.Quantity,
                    Price = c.Price,
                    Dsr = dsrid


                }).ToList();

                await _unitOfWork.DsrdetailRepository.AddRangeAsync(create);

                var created = await _unitOfWork.CommitAsync();

                if (created != 0)
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
    }
}

