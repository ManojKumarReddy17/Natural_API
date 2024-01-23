using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.S3;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Natural_API.Resources;
using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Natural_API.Controllers
{
    public class GuestProductController : Controller
    {
        private readonly IProductService _ProductService;
        private readonly IMapper _mapper;

        public GuestProductController(IProductService ProductService, IMapper mapper)
        {

            _ProductService = ProductService;
            _mapper = mapper;
           
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost("Search")]
        //public async Task<IEnumerable<GetProduct>> SearchDistributor([FromBody] SearchProductResource search)
        //{
        //    var exe = await _DistributorService.SearcDistributors(search);
        //    var execget = _mapper.Map<IEnumerable<Distributor>, IEnumerable<DistributorGetResource>>(exe);
        //    return execget;
        //}
    }
}

