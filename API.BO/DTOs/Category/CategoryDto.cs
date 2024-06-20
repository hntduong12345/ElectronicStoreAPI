using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.BO.DTOs.Category
{
    public class CategoryDto
    {
        public string? CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryDescription { get; set; }
    }
    public class CreateCategoryDto
    {
        [Required]
        [MinLength(1)]
        public string CategoryName { get; set; }
        [MinLength(1)]
        [Required]
        public string CategoryDescription { get; set; }
    }
    public class UpdateCategoryDto
    {
        [Required]
        [MinLength(1)]
        public string CategoryName { get; set; }
        [Required]
        [MinLength(1)]
        public string CategoryDescription { get; set; }
    }
}
