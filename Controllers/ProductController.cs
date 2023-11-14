using AutoMapper;
using KerryCoAdmin.Api.Entities.Dtos.Requests;
using KerryCoAdmin.Api.Entities.Dtos.Responses;
using KerryCoAdmin.Api.Entities.Models;
using KerryCoAdmin.Api.Interfaces;
using KerryCoAdmin.Api.Modules;
using KerryCoAdmin.Api.Repositories;
using KerryCoAdmin.Configurations;
using KerryCoAdmin.Entities.Dtos.Request;
using KerryCoAdmin.Entities.Dtos.Response;
using KerryCoAdmin.Interfaces;
using KerryCoAdmin.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace BusinessManagement.Controllers
{


    
    [Route("admin")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IMapper _mapper;
        private readonly ISecretService _secretService;
        private readonly List<VaultSecret> _secrets;
        private readonly string _cloudName;
        private readonly string _apiKey;
        private readonly string _apiSecret;



        public ProductController(IProductRepository productRepository,IAdminRepository adminRepository, IMapper mapper, ISecretService secretService)
        {
            _adminRepository = adminRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _secretService = secretService;
            _secrets = _secretService.GetSecrets();
            _cloudName = GetInfo.GetASecret("Cloudinary-cloudname", _secrets);
            _apiKey = GetInfo.GetASecret("Cloudinary-apiKey", _secrets);
            _apiSecret = GetInfo.GetASecret("Cloudinary-apiSecret", _secrets);

        }







        [HttpGet("products")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Staff")]
        public async Task<IActionResult> GetProducts()
        {
            var result = await _productRepository.GetProducts();


            if (result == null)
                return NotFound("No product found");

            var products = _mapper.Map<List<ProductResponse>>(result);

            return Ok(products);

            

        }




        [HttpGet("products/product/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Staff")]
        public async Task<IActionResult> GetProduct(string id)
        {
            var result = await _productRepository.GetProductById(id);

            if (result == null)
                return NotFound("No product found");

            var product = _mapper.Map<ProductResponse>(result);

            return Ok(product);

        }




        [HttpPost("{Id}/products/add-product")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Staff")]
        public async Task<IActionResult> AddProduct(string Id, [FromBody] ProductRequest req)
        {



            // check if project exists
            var productExists = await _productRepository.ProductExistsSlug(req.ProductSlug);

            if (productExists)
            {
                return BadRequest(new InitialAuthResponse()
                {
                    IsAuth = true,
                    StatusType = "error",
                    Message = "Product already exists",

                });
            }
            else
            {

                var admin = await _adminRepository.GetAdminByUser(Id);

                

                if (admin == null)
                    return BadRequest(new InitialAuthResponse()
                    {
                        IsAuth = true,
                        StatusType = "error",
                        Message = "Product not successfully saved",

                    });




                var new_product = new Product()
                {
                    ProductName = req.ProductName,
                    ProductDescription = req.Description,
                    ProductSlug = req.ProductSlug,
                    ModifiedDate = DateTime.Now,
                    Admin = admin,
                    Variations = new List<ProductVariation>()
                    {
                        new ProductVariation()
                        {
                            Size = req.Size,
                            Color = req.Color,
                            Quantity = req.Quantity,
                            UnitPrice = req.UnitPrice,
                            ImageUrl = req.ImageUrl,
                            NumberInPack = req.NumberInPack


                        }
                    },


                };



                var product = await _productRepository.CreateProduct(new_product);
                Console.WriteLine(product);

                //var dims = System.Text.Json.JsonSerializer.Deserialize<List<DimensionRequest>>((System.Text.Json.Nodes.JsonNode?)req.Dimensions);


                if (product != null)
                {

                    var response = new InitialAuthResponse()
                    {
                        IsAuth = true,
                        StatusType = "success",
                        Message = "Product successfully saved",

                    };

                    return Ok(response);
                   

                }
                else
                {

                    var errorResponse = new InitialAuthResponse()
                    {
                        IsAuth = true,
                        StatusType = "error",
                        Message = "Product not successfully saved2",

                    };

                    return BadRequest(errorResponse);

                }

                

            }

            
        }





        [HttpPut("products/edit-product")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Staff")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductUpdateRequest req)
        {
            var bad_request = new InitialAuthResponse()
                    {
                        IsAuth = true,
                        StatusType = "error",
                        Message = "Product already exists",

                    };

            if (req == null)
            {
                bad_request.Message = "No request";
                return BadRequest(bad_request);
            }

            if (!ModelState.IsValid)
            {
                bad_request.Message = "Bad model state";
                return BadRequest(bad_request);


            }

            var productId = req.Id;

            if (!await _productRepository.ProductExists(productId))
                return NotFound();

            var updateProduct = new Product()
            {
                ProductName = req.ProductName,
                ProductDescription = req.Description,
                ProductSlug = req.ProductSlug,
                ModifiedDate = DateTime.Now,
                Variations = new List<ProductVariation>()
                {
                   new ProductVariation()
                   {
                        Size = req.Size,
                        Color = req.Color,
                        Quantity = req.Quantity,
                        UnitPrice = req.UnitPrice,
                        ImageUrl = req.ImageUrl,
                        NumberInPack = req.NumberInPack

                   }
                },

            };

            //update project
            var productUpdated = await _productRepository.UpdateProduct(productId, updateProduct);

            if (productUpdated)
            {

                //check if the public id existed
                if (req.DeleteImage)
                {
                    // delete the image from cloudinary
                    var publicId = req.PrevImage;

                    CloudinarySettings cloudinarySettings = new CloudinarySettings()
                    {
                        CloudName = _cloudName,
                        ApiKey = _apiKey,
                        ApiSecret = _apiSecret
                    };

                    var removeImage = PhotoService.RemovePhoto(publicId, cloudinarySettings);

                    if (removeImage)
                    {
                        var response = new InitialAuthResponse()
                        {
                            IsAuth = true,
                            StatusType = "success",
                            Message = "Product updated successfully",

                        };

                        return Ok(response);
                    }
                    else
                    {
                        var errorRes = new InitialAuthResponse()
                        {
                            IsAuth = true,
                            StatusType = "error",
                            Message = "Unable to delete product image",

                        };

                        return Ok(errorRes);


                    }
                }
                else
                {

                    var response = new InitialAuthResponse()
                    {
                        IsAuth = true,
                        StatusType = "success",
                        Message = "Product updated successfully",

                    };

                    return Ok(response);

                }





                

            }

            var errorResponse = new InitialAuthResponse()
            {
                IsAuth = true,
                StatusType = "error",
                Message = "Unable to update product",

            };

            return Ok(errorResponse);

            

        }




        [HttpDelete("products/{Id}")]
        public async Task<IActionResult> DeleteProduct(string Id)
        {

            var bad_request = new InitialAuthResponse()
            {
                IsAuth = true,
                StatusType = "error",


            };

            if (Id == null)
            {
                bad_request.Message = "No request";
                return BadRequest(bad_request);
            }

            if (!ModelState.IsValid)
            {
                bad_request.Message = "Bad model state";
                return BadRequest(bad_request);


            }


            var savedProduct = await _productRepository.GetProductById(Id);

            if (savedProduct != null)
            {
                var productImage = savedProduct.Variations.FirstOrDefault().ImageUrl;

                var deleteProduct = _productRepository.DeleteProduct(savedProduct);

                if (deleteProduct)
                {

                    CloudinarySettings cloudinarySettings = new CloudinarySettings()
                    {
                        CloudName = _cloudName,
                        ApiKey = _apiKey,
                        ApiSecret = _apiSecret
                    };

                    var removeImage = PhotoService.RemovePhoto(productImage, cloudinarySettings);

                    if (removeImage)
                    {
                        var response = new InitialAuthResponse()
                        {
                            IsAuth = true,
                            StatusType = "success",
                            Message = "Product deleted successfully",

                        };

                        return Ok(response);
                    }
                    else
                    {
                        var errorRes = new InitialAuthResponse()
                        {
                            IsAuth = true,
                            StatusType = "error",
                            Message = "Unable to delete product image",

                        };

                        return Ok(errorRes);


                    }


                }
                else
                {
                    var errorResponse = new InitialAuthResponse()
                    {
                        IsAuth = true,
                        StatusType = "error",
                        Message = "Unable to delete product",

                    };

                    return Ok(errorResponse);
                }

            }
            else
            {
                var errorResponse = new InitialAuthResponse()
                {
                    IsAuth = true,
                    StatusType = "error",
                    Message = "This product does not exist",

                };

                return Ok(errorResponse);
            }


        }



    }
}
