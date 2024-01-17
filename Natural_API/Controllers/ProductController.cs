using Amazon.Runtime;
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

        // to test bucket connection
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllBucketAsync()
        {
            var buckets = await _ProductService.GetAllBucketAsync();
            return Ok(buckets);
        }


        [HttpGet]  //get products with category name and presignred url//

        public async Task<ActionResult<IEnumerable<GetProduct>>> GetAllPrtoductDetails(string? prefix)
        {
            var productresoursze = await _ProductService.GetAllPrtoductDetails(prefix);
            //var productresoursze1 = _mapper.Map<IEnumerable<GetProduct>, IEnumerable<ProductResource>>(productresoursze);
            //return Ok(productresoursze1);
            return Ok(productresoursze);

        }


        [HttpGet("Details/{ProductId}")]
        //get product by id -category name -presigned url
        public async Task<ActionResult<GetProduct>> GetProductDetailsById(string ProductId)
        {
            var productresult = await _ProductService.GetProductDetailsByIdAsync(ProductId);
            //var exec = _mapper.Map<Product, GetProduct>(productresult);

            return Ok(productresult);
        }

        [HttpGet("{ProductId}")] //get product by id as in tabel and presigned url
        public async Task<ActionResult<GetProduct>> GetProductById(string ProductId)

        {
            var productresult = await _ProductService.GetProductpresignedurlByIdAsync(ProductId);
            ////var exec = _mapper.Map<Product, GetProduct>(productresult);

            return Ok(productresult);

            //string bucketName = "lokesh-s3-demo";
            //string prefix = productresult.Image;
            //var PresignedUrl =      await _ProductService.GetAllFilesAsync(bucketName, prefix);
            ////var PresignedUrl = await GetAllFilesAsync(bucketName, prefix);
            //var isd = PresignedUrl.FirstOrDefault();
            //var productresoursze1 = _mapper.Map<Product, GetProduct>(productresult);
            //productresoursze1.PresignedUrl = isd.PresignedUrl;
            //return productresoursze1;
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

        //[HttpPut]
        [HttpPut]
        //first it will check weather image is being upadesd or not  if images is being update then if block will execute or else else will execute
        //public async Task<ActionResult<ProductResource>> UpdateProduct(string id, [FromForm] ProductResource productResource, string? prefix)
        //{
        //    var Existingproduct = await _ProductService.GetProductByIdAsync(id);

        //    var file = productResource.UploadImage;
        //    if (file != null && file.Length > 0)
        //    {
        //        var result = await _ProductService.UploadFileAsync(file, prefix); //change uploadfile to image
        //        var mappedexecutive = _mapper.Map(productResource, Existingproduct);
        //        mappedexecutive.Image = result.Message;
        //        var Updateresponse = await _ProductService.UpadateProduct(mappedexecutive);
        //        return StatusCode(Updateresponse.StatusCode, Updateresponse);
        //    }

        //    var mappedexecutive1 = _mapper.Map(productResource, Existingproduct);
        //    var Updateresponse1 = await _ProductService.UpadateProduct(mappedexecutive1);
        //    return StatusCode(Updateresponse1.StatusCode, Updateresponse1);
        //}

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

        //[HttpGet("get-key")]
        //// //get all files //all images with presignedurl
        //public async Task<IActionResult> GetAllFilesAsync(string bucketName, string? prefix)
        //{
        //    var result = await _ProductService.GetAllFilesAsync(bucketName, prefix);
        //    return Ok(result);
        //}



        //[HttpGet("get-by-key")]
        //// display image by key or path in db
        //public async Task<IActionResult> GetFileByKeyAsync(string bucketName, string key)
        //{
        //    var bucketExists = await _s3Client.DoesS3BucketExistAsync(bucketName);
        //    if (!bucketExists) return NotFound($"Bucket {bucketName} does not exist.");
        //    var s3Object = await _s3Client.GetObjectAsync(bucketName, key);
        //    return File(s3Object.ResponseStream, s3Object.Headers.ContentType);
        //}





    }
}

