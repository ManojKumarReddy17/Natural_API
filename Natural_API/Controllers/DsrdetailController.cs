using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Natural_API.Resources;
using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Services;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Natural_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DsrdetailController : ControllerBase
    {

        private readonly IDsrdetailService _DsrdetailService;
        private readonly IMapper _mapper;
        public DsrdetailController(IDsrdetailService DsrdetailService, IMapper mapper)
        {
            _DsrdetailService = DsrdetailService;
            _mapper = mapper;

        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dsrdetail>>> GetDsrDetailsByDsrIdAsync(string dsrId)
           {

               var result = await _DsrdetailService.GetDsrDetailsByDsrIdAsync(dsrId);

               return Ok(result);
           }



    [HttpPost]
        //[ValidateAntiForgeryToken]

        // first image is being uploaded to s3bucket and later product data to mysql
        public async Task<ActionResult<ResultResponse>> InsertDsrdetails([FromBody] List<DsrdetailProduct> Dsrdetail, string id)
        {
            var detaiils = _mapper.Map<List<DsrdetailProduct>, List<Dsrdetail>>(Dsrdetail);
            var createDistributorResponse = await _DsrdetailService.CreateDsrdetail(detaiils, id);

            return StatusCode(createDistributorResponse.StatusCode, createDistributorResponse);
        }


        [HttpPut]
        //[ValidateAntiForgeryToken]

        // first image is being uploaded to s3bucket and later product data to mysql
        //public async Task<ActionResult<ResultResponse>> updateDsrdetails([FromBody] List<DsrdetailProduct> Dsrdetail, string id)

        //{
        //    var existingProductList = await _DsrdetailService.GetDsrDetailsByDsrIdAsync(id);

        //    var detaiils = _mapper.Map<List<DsrdetailProduct>, List<Dsrdetail>>(Dsrdetail);

        //        var updatedprodctList = (from s1 in existingProductList
        //                                 join s2 in detaiils on s1.Product equals s2.Product into gj
        //                                 from s2 in gj.DefaultIfEmpty()
        //                                 where s1.Product == s2.Product
        //                                 //where s1.Dsr == s2.Dsr
        //                                 select new Dsrdetail
        //                                 {
        //                                     Id = s1.Id,
        //                                     Product = s1.ProductNavigation.Id,
        //                                     Quantity = s2 != null ? s2.Quantity : 0, // If s2 is null, set Quantity to 0 or handle null case accordingly
        //                                     Price = s2 != null ? s2.Price : 0, // If s2 is null, set Price to 0 or handle null case accordingly
        //                                     Dsr = s1.Dsr
        //                                 }).ToList();



        //    var createDistributorResponse = await _DsrdetailService.UpadateDsrdetail(updatedprodctList);

        //    return StatusCode(createDistributorResponse.StatusCode, createDistributorResponse);
        //}
        public async Task<ActionResult<ResultResponse>> updateDsrdetails([FromBody] List<DsrdetailProduct> Dsrdetail, string id)
        {
            try
            {
                var existingProductList = await _DsrdetailService.GetDsrDetailsByDsrIdAsync(id);

                var detaiils = _mapper.Map<List<DsrdetailProduct>, List<Dsrdetail>>(Dsrdetail);

                // Wrap the LINQ query in a try-catch block
                try
                {
                    var updatedprodctList = (from s1 in existingProductList
                                             join s2 in detaiils on s1.Product equals s2.Product into gj
                                             from s2 in gj.DefaultIfEmpty()
                                             where s1.Product == s2.Product
                                             //where s1.Dsr == s2.Dsr
                                             select new Dsrdetail
                                             {
                                                 Id = s1.Id,
                                                 Product = s1.ProductNavigation.Id,
                                                 Quantity = s2 != null ? s2.Quantity : 0, // If s2 is null, set Quantity to 0 or handle null case accordingly
                                                 Price = s2 != null ? s2.Price : 0, // If s2 is null, set Price to 0 or handle null case accordingly
                                                 Dsr = s1.Dsr
                                             }).ToList();

                    var createDistributorResponse = await _DsrdetailService.UpadateDsrdetail(updatedprodctList);

                    return StatusCode(createDistributorResponse.StatusCode, createDistributorResponse);
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it as needed
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing the request.");
            }
        }


        //[HttpPut]
        ////[ValidateAntiForgeryToken]

        //// first image is being uploaded to s3bucket and later product data to mysql
        //public async Task<ActionResult<ResultResponse>> updateDsrdetails([FromBody] List<DsrdetailProduct> Dsrdetail, string id)
        //{
        //    var existingProductList = await _DsrdetailService.GetDsrDetailsByDsrIdAsync(id);

        //    var detaiils = _mapper.Map<List<DsrdetailProduct>, List<Dsrdetail>>(Dsrdetail);

        //    //var updatedprodctList = (from s1 in existingProductList
        //    //                         join s2 in detaiils on s1.Product equals s2.Product into gj
        //    //                         from s2 in gj.DefaultIfEmpty()
        //    //                         select s2 != null ? s2 : s1).ToList();

        //    //var updatedprodctList =  (from s1 in existingProductList
        //    //                         join s2 in detaiils on s1.Dsr equals s2.Dsr
        //    //                         where s1.Product == s1.Product
        //    //                         select new Dsrdetail
        //    //                         {
        //    //                             Id = s1.Id,
        //    //                             Product = s1.Product,
        //    //                             Quantity = s2.Quantity,
        //    //                             Price = s2.Price
        //    //                         }).ToList();

        //    //    List<Dsrdetail> Products = new List<Dsrdetail>
        //    //{
        //    //    new Dsrdetail {Id =1,Dsr="DSR2225" , Quantity = 9, Price = 540 },
        //    //    new Dsrdetail { Id =7,Dsr="DSR2225" , Quantity = 8, Price = 100 }
        //    //};

        //    //var resultList = (from s1 in existingProductList
        //    //                  join s2 in detaiils on s1.Product equals s2.Product into gj
        //    //                  from s2 in gj.DefaultIfEmpty()
        //    //                  select new Dsrdetail
        //    //                  {
        //    //                      Id = s1.Id,
        //    //                      Product = s1.ProductNavigation.Id,
        //    //                      Quantity =s2.Quantity,
        //    //                      Price=s2.Price,
        //    //                      Dsr= s1.Dsr
        //    //                      // Add other fields from details if needed
        //    //                  }).ToList();

        //    var resultList = (from s1 in existingProductList

        //                      join s2 in detaiils on s1.Product equals s2.Product into gj
        //                      from s2 in gj.DefaultIfEmpty()
        //                      select new Dsrdetail
        //                      {
        //                          Id = s1.Id,
        //                          Product = s1.ProductNavigation != null ? s1.ProductNavigation.Id : null,
        //                          Quantity = s2 != null ? s2.Quantity : s1.Quantity, // Assuming default value for Quantity if s2 is null
        //                          Price = s2 != null ? s2.Price : s1.Price, // Assuming default value for Price if s2 is null
        //                          Dsr = s1.Dsr
        //                          // Add other fields from details if needed
        //                      }).ToList();

        //    var createDistributorResponse = await _DsrdetailService.UpadateDsrdetail(resultList);

        //    return StatusCode(createDistributorResponse.StatusCode, createDistributorResponse);
        //}

    }
}

