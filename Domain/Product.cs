using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryApi.Domain
{
    public class Product
    {
        public int Id { get; set; }

        private string productCode;
        [Required]
        [MinLength(3)]
        [MaxLength(3)]
        public string ProductCode {
            get { return productCode; }
            set { productCode = value.ToUpper(); }
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ShortDescription { get; set; }

        [Required]
        public float? Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int? Quantity { get; set; }

        [Required]
        public string HiddenProperty { get; set; }

        // Relationships
        public List<Review> Reviews { get; set; }
    }
}
