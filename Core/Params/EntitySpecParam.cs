using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Params
{
    public class EntitySpecParams
    {
        private const int MaxPageSize = 25;

        private int _pageSize = 4;

        private string _search = "";

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public decimal Price { get; set; }

        // public string DecimalOrderBy { get; set; } = "greater";

        public int Quantity { get; set; }

        public string SortBy { get; set; } = "Name";

        public string SortOrder { get; set; } = "asc";

        public string Search
        {
            get => _search;
            set => _search = value.ToLower();
        }

    }
}