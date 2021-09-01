using AutoMapper;
using BakeryApi.Data;
using BakeryApi.Domain;
using BakeryApi.DTOs;
using BakeryApi.Filters;
using BakeryApi.Helpers;
using BakeryApi.ResourceParameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BakeryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository repository;
        private readonly ILogger logger;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;

        public ProductsController(IProductRepository repository, ILogger<ProductsController> logger, LinkGenerator linkGenerator, IMapper mapper)
        {
            this.repository = repository;
            this.logger = logger;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
        }

        [HttpGet("", Name = "GetProducts")]
        public ActionResult<ProductDTO[]> GetProducts([FromQuery]ProductsResourceParameters parameters)
        {
            try
            {
                PagedList<Product> products = repository.GetProducts(parameters);

                var previousPageLink = products.HasPrevious ? CreateProductsResourceUri(parameters, ResourceUriType.PreviousPage) : null;
                var nextPageLink = products.HasNext ? CreateProductsResourceUri(parameters, ResourceUriType.NextPage) : null;
                var paginationMetadata = new
                {
                    totalCount = products.TotalCount,
                    pageSize = products.PageSize,
                    currentPage = products.CurrentPage,
                    totalPages = products.TotalPages,
                    previousPageLink,
                    nextPageLink
                };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

                //Response.Headers.Add("X-Total-Count", products.Count().ToString());
                return mapper.Map<ProductDTO[]>(products);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong, please try again.",
                    error = ex.ToString()
                });
            }
        }

        [HttpPost("")]
        public ActionResult<ProductDTO> CreateProduct(ProductDTO product)
        {
            try
            {
                var fullProduct = mapper.Map<Product>(product);
                fullProduct.HiddenProperty = "random-thing"; // Purely demonstrating that things can be hidden from the DTO

                // make sure we don't already have a matching code
                if (repository.GetProduct(fullProduct.ProductCode, includeReviews: false) != null) return BadRequest(new
                {
                    message = "A product already exists with this code, please try again."
                });

                var savedProduct = repository.CreateProduct(fullProduct);
                string uri = linkGenerator.GetPathByAction(HttpContext, action: "GetProduct", controller: "Products", values: new { code = savedProduct.ProductCode });

                return Created(uri, mapper.Map<ProductDTO>(savedProduct));

            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong, please try again.",
                    error = ex.ToString()
                });
            }
        }
        
        [HttpGet("{code}")]
        [HttpHead("{code}")]
        [TypeFilter(typeof(CheckExistingProductFilter))]
        public ActionResult<ProductDTO> GetProduct(string code)
        {
            try
            {
                var product = repository.GetProduct(code, includeReviews: false);
                return mapper.Map<ProductDTO>(product);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong, please try again.",
                    error = ex.ToString()
                });
            }
        }

        [HttpDelete("{code}")]
        [TypeFilter(typeof(CheckExistingProductFilter))]
        public IActionResult DeleteProduct(string code)
        {
            try
            {
                repository.DeleteProduct(code);
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong, please try again.",
                    error = ex.ToString()
                });
            }
        }

        [HttpPut("{code}")]
        [TypeFilter(typeof(CheckExistingProductFilter))]
        public ActionResult<ProductDTO> UpdateProduct(string code, ProductDTO newProduct)
        {
            try
            {
                // Get the product & check exists
                var oldProduct = repository.GetProduct(code, includeReviews: false);

                // Check they have not changed the code
                if (oldProduct.ProductCode != newProduct.ProductCode) return BadRequest(new
                {
                    message = "You cannot modify the product code, this is fixed"
                });

                // Map changes onto the old product
                mapper.Map<Product, Product>(mapper.Map<Product>(newProduct), oldProduct);

                var updatedProduct = repository.UpdateProduct(oldProduct);

                return Ok(mapper.Map<ProductDTO>(updatedProduct));

            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong, please try again.",
                    error = ex.ToString()
                });
            }
        }

        [HttpPatch("{code}/{quantity:int}")]
        [TypeFilter(typeof(CheckExistingProductFilter))]
        public ActionResult<ProductDTO> SubmitStockCheck(string code, int quantity)
        {
            try
            {
                // Get the product & check exists
                var product = repository.GetProduct(code, includeReviews: false);

                repository.SubmitStockCheck(code, quantity);

                return Ok(mapper.Map<ProductDTO>(product));

            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    message = "Something went wrong, please try again.",
                    error = ex.ToString()
                });
            }
        }

        private string CreateProductsResourceUri(ProductsResourceParameters parameters, ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return Url.Link("GetProducts", new
                    {
                        pageNumber = parameters.PageNumber - 1,
                        pageSize = parameters.PageSize,
                        includeOutOfStock = parameters.IncludeOutOfStock,
                        minPrice = parameters.MinPrice,
                        maxPrice = parameters.MaxPrice
                    });
                case ResourceUriType.NextPage:
                    return Url.Link("GetProducts", new
                    {
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize,
                        includeOutOfStock = parameters.IncludeOutOfStock,
                        minPrice = parameters.MinPrice,
                        maxPrice = parameters.MaxPrice
                    });
                default:
                    return Url.Link("GetProducts", new
                    {
                        pageNumber = parameters.PageNumber,
                        pageSize = parameters.PageSize,
                        includeOutOfStock = parameters.IncludeOutOfStock,
                        minPrice = parameters.MinPrice,
                        maxPrice = parameters.MaxPrice
                    });
            }
        }

    }
}
