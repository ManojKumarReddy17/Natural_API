using System;
using Natural_Core.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Natural_Core.IServices
{
	public interface IDsrdetailService
	{
        Task<ResultResponse> CreateDsrdetail(List<Dsrdetail> dsr,string id);

    }
}

