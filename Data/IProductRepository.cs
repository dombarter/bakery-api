using BakeryApi.Domain;
using BakeryApi.Helpers;
using BakeryApi.ResourceParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryApi.Data
{
    public interface IProductRepository
    {
        public PagedList<Product> GetProducts(ProductsResourceParameters parameters);
        public Product GetProduct(string code, bool includeReviews);
        public Product CreateProduct(Product product);
        public Product UpdateProduct(Product product);
        public Product SubmitStockCheck(string code, int quantity);
        public void DeleteProduct(string code);
        public Review[] GetReviews(string code);
        public Review CreateReview(string code, Review review);
    }
}
