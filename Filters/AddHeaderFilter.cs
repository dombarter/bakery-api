using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryApi.Filters
{
    public class AddHeaderFilter : IResultFilter
    {
        private readonly string headerName;
        private readonly string headerValue;

        public AddHeaderFilter(string headerName, string headerValue)
        {
            this.headerName = headerName;
            this.headerValue = headerValue;
        }

        public void OnResultExecuted(ResultExecutedContext context) {}

        public void OnResultExecuting(ResultExecutingContext context) {
            context.HttpContext.Response.Headers.Add(headerName, headerValue);
        }
    }
}
