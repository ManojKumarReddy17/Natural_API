using System;
using Natural_Core.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Natural_Core.IServices
{
	public interface IDsrdetailService
	{
        Task<ResultResponse> CreateDsrdetail(List<Dsrdetail> dsr,string id);
        //Task<IEnumerable<Dsrdetail>> GetDsrDetailsByDsrIdAsync(string dsrId);
        //Task<ResultResponse> UpadateDsrdetail(List<Dsrdetail> updatingRecord);
        Task<IEnumerable<DsrProduct>> GetDsrDetailsByDsrIdAsync(string dsrId);
        Task<ResultResponse> UpadateDsrdetail(List<Dsrdetail> updatedrecord,  string dsrId);
        Task<IEnumerable<GetProduct>> GetDetailTableAsync(string dsrId);



    }
}

