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



        public async Task<IEnumerable<DsrProduct>> GetDsrDetailsByDsrIdAsync(string dsrId)

        {

            var dsrdetails = await _unitOfWork.DsrdetailRepository.GetDsrDetailsByDsrIdAsync(dsrId);

            return dsrdetails;

        }


       

       public async Task<ResultResponse> UpadateDsrdetail(List<Dsrdetail> updatedrecord,  string dsrId)
        {
            var response = new ResultResponse();

            try
            {
                var   existingrecord = await _unitOfWork.DsrdetailRepository.GetDetailTableByDsrIdAsync(dsrId);

                existingrecord = existingrecord
                    .Join(updatedrecord,
                        od => od.Id,
                        pd => pd.Id,
                        (od, pd) => {
                            od.Quantity = pd != null ? pd.Quantity : 0; // Update Quantity
                            od.Price = pd != null ? pd.Price : 0;       // Update Price
                            return od;
                        })
                    .ToList();
         
                _unitOfWork.DsrdetailRepository.UpdateRange(existingrecord);
               
                var updated = await _unitOfWork.CommitAsync();
                if (updated != 0)
                {

                    response.Message = "updatesuceesfull";
                    response.StatusCode = 200;
                }
            }
            catch (Exception ex)
            {
                response.Message = "Failed";
                response.StatusCode = 500;
            }

            return (response);

        }


        public async Task<IEnumerable<GetProduct>> GetDetailTableAsync(string dsrId)
        {

          var dsrdetailstable = await  _unitOfWork.DsrdetailRepository.GetDetailTableAsync(dsrId);
            return dsrdetailstable;

        }






    }
}

