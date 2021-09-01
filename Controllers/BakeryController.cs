using BakeryApi.Filters;
using BakeryApi.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BakeryController : Controller
    {
        private readonly IOptionsSnapshot<BakeryOptions> options;

        public BakeryController(IOptionsSnapshot<BakeryOptions> options)
        {
            this.options = options;
        }

        [HttpGet("name")]
        [ServiceFilter(typeof(BakeryFilter))]
        [ResponseCache(Duration = 1000)]
        public ActionResult<string> GetBakeryName()
        {
            return options.Value.BakeryName;
        }

        [HttpGet("hours")]
        [TypeFilter(typeof(AddHeaderFilter), Arguments = new object[] { "bread-of-the-day", "Sourdough"})]
        [ResponseCache(Duration = 1000)]
        public ActionResult<OpeningHours> GetBakeryOpeningHours()
        {
            return options.Value.BakeryOpeningHours;
        }

    }
}
