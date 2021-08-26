using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryApi.Domain
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public string Reviewer { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        [Range(0, 5)]
        public int? StarRating { get; set; }

        // Relationships
        [Required]
        public Product Product { get; set; }
    }
}
