using AutoMapper;
using E_Commerce.Application.DTOs.Products;
using E_Commerce.Domain.Entities.Products;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Profiles
{
    public class PictureUrlResolver : IValueResolver<Product, ProductDto, string>
    {
        private readonly UrlSettings _urlSetting;

        public PictureUrlResolver(IOptions<UrlSettings> options)
        {
            _urlSetting = options.Value;
        }
        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            var baseUrl = _urlSetting.BaseUrl.TrimEnd('/');
            var path = source.PictureUrl.TrimEnd('/');

            return $"{baseUrl}/Files/{path}";
        }

        public class UrlSettings
        {
            public string BaseUrl { get; set; } = default!;
        }
    }
}
