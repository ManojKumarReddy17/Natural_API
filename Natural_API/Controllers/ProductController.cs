﻿using Amazon.Runtime;
using Amazon.S3;

using Amazon.S3.Model;
using Amazon.S3.Model.Internal.MarshallTransformations;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using Natural_API.Resources;
using Natural_Core.IServices;
using Natural_Core.Models;
using Natural_Services;
using System.Reflection.Metadata.Ecma335;
using static NuGet.Packaging.PackagingConstants;


namespace Natural_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _ProductService;
        private readonly IMapper _mapper;
        private readonly IAmazonS3 _s3Client;


        public ProductController(IProductService ProductService, IMapper mapper, IAmazonS3 s3Client)
        {

            _ProductService = ProductService;
            _mapper = mapper;
            _s3Client = s3Client;
        }

        //// to test bucket connection
        //[HttpGet("get-all")]
        //public async Task<IActionResult> GetAllBucketAsync()
        //{
        //    var buckets = await _ProductService.GetAllBucketAsync();
        //    return Ok(buckets);
        //}


        [HttpGet]  //get products with category name and presignred url//

        public async Task<ActionResult<IEnumerable<GetProduct>>> GetAllPrtoductDetails(string? prefix)
        {
            var productresoursze = await _ProductService.GetAllPrtoductDetails(prefix);
            
            return Ok(productresoursze);

        }


        [HttpGet("Details/{ProductId}")]
        //get product by id -category name -presigned url
        public async Task<ActionResult<GetProduct>> GetProductDetailsById(string ProductId)
        {
            var productresult = await _ProductService.GetProductDetailsByIdAsync(ProductId);
           

            return Ok(productresult);
        }

        [HttpGet("{ProductId}")] //get product by id as in tabel and presigned url
        public async Task<ActionResult<GetProduct>> GetProductById(string ProductId)
        {
            var productresult = await _ProductService.GetProductpresignedurlByIdAsync(ProductId);         

            return Ok(productresult);
            
        }

        // POST: ProductController/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]

        // first image is being uploaded to s3bucket and later product data to mysql
        public async Task<ActionResult<ProductResource>> Insertproduct([FromForm] ProductResource productResource, string? prefix)
        {
            var file = productResource.UploadImage;
            var result = await _ProductService.UploadFileAsync(file, prefix);
            var createexecu = _mapper.Map<ProductResource, Product>(productResource);
            createexecu.Image = result.Message;
            var exe = await _ProductService.CreateProduct(createexecu);
            return StatusCode(exe.StatusCode, exe);
        }

       
        [HttpPut]
       
        public async Task<ActionResult<ProductResource>> UpdateProduct([FromForm] ProductResource productResource, string? prefix)
        {
            var id = productResource.Id;
            var Existingproduct = await _ProductService.GetProductByIdAsync(id);

            var file = productResource.UploadImage;
            if (file != null && file.Length > 0)
            {
                var result = await _ProductService.UploadFileAsync(file, prefix); //change uploadfile to image
                var mappedexecutive = _mapper.Map(productResource, Existingproduct);
                mappedexecutive.Image = result.Message;
                var Updateresponse = await _ProductService.UpadateProduct(mappedexecutive);
                return StatusCode(Updateresponse.StatusCode, Updateresponse);
            }

            var mappedexecutive1 = _mapper.Map(productResource, Existingproduct);
            var Updateresponse1 = await _ProductService.UpadateProduct(mappedexecutive1);
            return StatusCode(Updateresponse1.StatusCode, Updateresponse1);
        }

        [HttpDelete("Delete/{ProductById}")]
        //delete complete product data
        //deleting image from s3 buckect and  productdata from db 
        public async Task<IActionResult> DeleteProduct(string ProductById)
        {
            var produ = await _ProductService.DeleteProduct(ProductById);
            return Ok(produ);
        }


        [HttpDelete("{ProductId}")]
        //if while updating i want to delete image
        public async Task<IActionResult> DeleteImage(string ProductId)
        {
            var produ = await _ProductService.DeleteImage(ProductId);
            return Ok(produ);
        }


    }
}
