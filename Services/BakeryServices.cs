using BakeryApi.Filters;
using BakeryApi.Options;
using BakeryApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryApi.Services
{
    public static class BakeryServices
    {
        public static void AddBakeryServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BakeryDbContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("BakeryDbContext")));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddAutoMapper(typeof(Startup));
            services.Configure<BakeryOptions>(configuration.GetSection("BakeryOptions"));
            services.AddScoped<BakeryFilter>();

            services.AddCors(o => o.AddPolicy("BakeryCORSPolicy", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

            services.AddResponseCaching();
        }
    }
}
