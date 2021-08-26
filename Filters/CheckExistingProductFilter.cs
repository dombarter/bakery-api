using BakeryApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryApi.Filters
{
    public class CheckExistingProductFilter : IActionFilter
    {
        private readonly IProductRepository repository;
        private readonly ILogger<CheckExistingProductFilter> logger;

        public CheckExistingProductFilter(IProductRepository repository, ILogger<CheckExistingProductFilter> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context) {}

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string code = (string)context.ActionArguments["code"] ?? (string)context.ActionArguments["productCode"];
            logger.LogInformation($"Action Argument: {code}");

            var product = repository.GetProduct(code, includeReviews: false);

            if (product == null)
            {
                context.Result = new NotFoundObjectResult($"The product with code: {code} could not be found in the database. Please ensure the product code is correct and then retry the attempted action.");
            }
        }
    }
}
