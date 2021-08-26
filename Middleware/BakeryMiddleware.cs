using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryApi.Middleware
{
    public class BakeryMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public BakeryMiddleware(RequestDelegate next, ILogger<BakeryMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public Task Invoke(HttpContext context)
        {
            var url = context.Request.Path;
            logger.LogInformation($"URL visited: {url}");

            if (url.ToString().ToLower().Contains("goldenticket"))
            {
                return context.Response.WriteAsync(
                    "You found a golden ticket! Have some free items from our bakery!"
                );

            }

            return next(context);
        }
    }
}
