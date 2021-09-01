using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BakeryApi.DTOs
{
    public class ProductDTO
    {
        private string productCode;
        [Required]
        [MinLength(3)]
        [MaxLength(3)]
        public string ProductCode
        {
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

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<LinkDTO> Links { get; set; }

    }
}
