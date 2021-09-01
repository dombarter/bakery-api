using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryApi.ResourceParameters
{
    public class ProductsResourceParameters
    {
        public bool IncludeOutOfStock { get; set; } = true;

        public float MinPrice { get; set; } = 0;

        public float MaxPrice { get; set; } = float.MaxValue;

        private const int DefaultPageNumber = 1;
        private int pageNumber = DefaultPageNumber;
        public int PageNumber
        {
            get
            {
                return pageNumber;
            }
            set
            {
                if (pageNumber < 1)
                    pageNumber = DefaultPageNumber;
                else
                    pageNumber = value;
            }
        }

        private const int MaxPageSize = 25;
        private const int DefaultPageSize = 3;
        private int pageSize = DefaultPageSize;
        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                if (value > MaxPageSize)
                    pageSize = MaxPageSize;
                else if (value < 1)
                    pageSize = DefaultPageSize;
                else
                    pageSize = value;
            }
        }
    }
}
