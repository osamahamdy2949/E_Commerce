using E_Commerce.Application.Common;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    internal class ProductWithTypeAndBrandSpec : BaseSpecification<Product,int>
    {
        public ProductWithTypeAndBrandSpec(ProductQueryParams queryParams) 
            //: base(p=> (brandId == null || p.BrandId == brandId) && (typeId == null || p.TypeId == typeId))
            :base(p=> (!queryParams.BrandId.HasValue || p.BrandId == queryParams.BrandId.Value) 
            && (!queryParams.TypeId.HasValue || p.TypeId == queryParams.TypeId.Value)
            && (string.IsNullOrWhiteSpace(queryParams.SearchValue) || p.Name.ToLower().Contains(queryParams.SearchValue.ToLower())))
        {
            AddInclude(p=> p.ProductType);
            AddInclude(p=> p.ProductBrand);

            switch(queryParams.Sort)
            {
                case SortingOptions.NameAsc:
                    AddOrderBy(p=> p.Name);
                    break;
                case SortingOptions.NameDesc:
                    AddOrderByDesc(p=> p.Name);
                    break;
                case SortingOptions.PriceAsc:
                    AddOrderBy(P=> P.Price);
                    break;
                case SortingOptions.PriceDesc:
                    AddOrderByDesc(P=> P.Price);
                    break;
                default:
                    AddOrderBy(P=> P.Id); 
                    break;

            }

            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }

        public ProductWithTypeAndBrandSpec(int id) : base(p=> p.Id == id)
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
        }
    }
}
