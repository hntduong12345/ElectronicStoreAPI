using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using API.BO.Constants;
using API.BO.CustomAttributes;
using API.BO.DTOs.Category;

namespace API.BO.DTOs.Product
{
    public class ProductDto
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string? Description { get; set; }
        public decimal DefaultPrice { get; set; }
        public string CategoryId { get; set; }
        public CategoryDto? Category { get; set; }
        public string Manufacturer { get; set; }
        public int StorageAmount { get; set; }
        public int SaleAmount { get; set; }
        public decimal CurrentPrice { get; set; }
        public bool IsOnSale { get; set; }
        public DateTime? SaleEndDate { get; set; }
        public string? RelativeUrl { get; set; }
    }
    public class CreateProductDto
    {
        [Required]
        [MinLength(1)]
        public string ProductName { get; set; }
        public string? Description { get; set; }
        [Range(0, int.MaxValue)]
        public decimal DefaultPrice { get; set; } = 0;
        [Required]
        [NotNull]
        public string CategoryId { get; set; }
        [MinLength(1)]
        public string Manufacturer { get; set; }
        [Range(0, int.MaxValue)]
        public int StorageAmount { get; set; } = 0;
        //[Range(0, int.MaxValue)]
        //public int SaleAmount { get; set; } = 0;
        //[Range(0, int.MaxValue)]
        //public decimal CurrentPrice { get; set; } = 0;
        //public bool IsOnSale { get; set; } = false;
        //[AllowNull]
        //public DateTime? SaleEndDate { get; set; } = null;
        [Required]
        [AllowFileContentType(new string[] { "image/png", "image/jpeg" })]
        public IFormFile ImageFile { get; set; }
    }

    public class UpdateProductDto
    {
        [Required]
        [MinLength(1)]
        public string ProductName { get; set; }
        public string? Description { get; set; }
        [Range(0, int.MaxValue)]
        public decimal DefaultPrice { get; set; }
        [Required]
        [NotNull]
        public string CategoryId { get; set; }
        [MinLength(1)]
        public string Manufacturer { get; set; }
        [Range(0, int.MaxValue)]
        public int StorageAmount { get; set; }
        //[Range(0, int.MaxValue)]
        //public decimal CurrentPrice { get; set; } = 0;
        public bool IsOnSale { get; set; }
        [AllowNull]
        [IsDateTimeAfterNow]
        public DateTime? SaleEndDate { get; set; }
        [AllowNull]
        [AllowFileContentType(new string[] { "image/png", "image/jpeg" })]
        public IFormFile? ImageFile { get; set; }
    }
}
