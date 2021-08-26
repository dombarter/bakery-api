using BakeryApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryApi.Data
{
    public interface IProductRepository
    {
        public Product[] GetProducts(bool includeOutOfStock, float minPrice, float maxPrice);
        public Product GetProduct(string code, bool includeReviews);
        public Product CreateProduct(Product product);
        public Product UpdateProduct(Product product);
        public Product SubmitStockCheck(string code, int quantity);
        public void DeleteProduct(string code);
        public Review[] GetReviews(string code);
        public Review CreateReview(string code, Review review);
    }
}
