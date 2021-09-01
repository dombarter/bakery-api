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

        private const int DEFAULT_PAGE_NUMBER = 1;
        private int pageNumber = DEFAULT_PAGE_NUMBER;
        public int PageNumber
        {
            get
            {
                return pageNumber;
            }
            set
            {
                if (pageNumber < 1)
                    pageNumber = DEFAULT_PAGE_NUMBER;
                else
                    pageNumber = value;
            }
        }

        private const int MAX_PAGE_SIZE = 10;
        private const int DEFAULT_PAGE_SIZE = 3;
        private int pageSize = DEFAULT_PAGE_SIZE;
        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                if (value > MAX_PAGE_SIZE)
                    pageSize = MAX_PAGE_SIZE;
                else if (value < 1)
                    pageSize = DEFAULT_PAGE_SIZE;
                else
                    pageSize = value;
            }
        }
    }
}
