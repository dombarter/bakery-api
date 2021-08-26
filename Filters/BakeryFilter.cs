using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryApi.Filters
{
    public class BakeryFilter : IActionFilter
    {
        private readonly ILogger logger;

        public BakeryFilter(ILoggerFactory loggerFactory)
        {
            this.logger = loggerFactory.CreateLogger<BakeryFilter>();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation($"OnActionExecuted: {context.HttpContext.Request.Path}");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation($"OnActionExecuting: {context.HttpContext.Request.Path}");
        }
    }
}
