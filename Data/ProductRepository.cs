using AutoMapper;
using BakeryApi.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryApi.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly BakeryDbContext context;

        public ProductRepository(BakeryDbContext context)
        {
            this.context = context;
        }

        public Product CreateProduct(Product product)
        {
            context.Products.Add(product);
            context.SaveChanges();
            return product;
        }

        public Review CreateReview(string code, Review review)
        {
            GetProduct(code, includeReviews: true).Reviews.Add(review);
            context.SaveChanges();
            return review;
        }

        public void DeleteProduct(string code)
        {
            var product = GetProduct(code, includeReviews: false);
            context.Products.Remove(product);
            context.SaveChanges();
        }

        public Product GetProduct(string code, bool includeReviews)
        {
            if (includeReviews)
                return context.Products
                    .Include("Reviews")
                    .FirstOrDefault(p => p.ProductCode == code.ToUpper());

            return context.Products
                    .FirstOrDefault(p => p.ProductCode == code.ToUpper());
        }

        public Product[] GetProducts(bool includeOutOfStock, float minPrice, float maxPrice)
        {
            return context.Products
                .Where(x => x.Price >= minPrice)
                .Where(x => x.Price <= maxPrice)
                .Where(x => includeOutOfStock ? true : x.Quantity > 0)
                .ToArray();
        }

        public Review[] GetReviews(string code)
        {
            var product = GetProduct(code, includeReviews: true);
            var reviews = product.Reviews;


            return GetProduct(code, includeReviews: true).Reviews.ToArray();
        }

        public Product SubmitStockCheck(string code, int quantity)
        {
            var product = GetProduct(code, includeReviews: false);
            product.Quantity = quantity;
            context.SaveChanges();
            return product;
        }

        public Product UpdateProduct(Product product)
        {
            context.Products.Update(product);
            context.SaveChanges();
            return product;
        }
    }
}
