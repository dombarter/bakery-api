using AutoMapper;
using BakeryApi.Data;
using BakeryApi.Domain;
using BakeryApi.DTOs;
using BakeryApi.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryApi.Controllers
{
    [ApiController]
    [Route("/api/products/{code}/[controller]")]
    public class ReviewsController : Controller
    {
        private readonly IProductRepository repository;
        private readonly ILogger<ProductsController> logger;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;

        public ReviewsController(IProductRepository repository, ILogger<ProductsController> logger, LinkGenerator linkGenerator, IMapper mapper)
        {
            this.repository = repository;
            this.logger = logger;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
        }

        [HttpGet("", Name = "GetReviews")]
        [TypeFilter(typeof(CheckExistingProductFilter))]
        public ActionResult<ReviewDTO[]> GetReviews(string code)
        {
            try
            {
                var reviews = repository.GetReviews(code);

                // Return the new review
                return mapper.Map<ReviewDTO[]>(reviews);
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

        [HttpPost("", Name = "CreateReview")]
        [TypeFilter(typeof(CheckExistingProductFilter))]
        public ActionResult<ReviewDTO> CreateReview(string code, ReviewDTO review)
        {
            try
            {
                // Create the review
                var savedReview = mapper.Map<ReviewDTO>(repository.CreateReview(code, mapper.Map<Review>(review)));
                string uri = linkGenerator.GetPathByAction("GetReviews", "Reviews", new { code = code });


                // Return the new review
                return Created(uri, savedReview);
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
    }
}
