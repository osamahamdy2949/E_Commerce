using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Common
{
    public class ProductQueryParams
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? SearchValue { get; set; }
        public SortingOptions Sort { get; set; }

        public int PageIndex { get; set; }
        private const int _defaultPageSize = 5;
        private const int _maxPageSize = 10;
        private int _pageSize = _defaultPageSize;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > _maxPageSize ? _maxPageSize : (value < 1 ? _defaultPageSize : value);
        }
    }

    public enum SortingOptions
    {
        None =0, 
        NameAsc = 1, 
        NameDesc = 2,
        PriceAsc = 3, 
        PriceDesc = 4
    }
}
