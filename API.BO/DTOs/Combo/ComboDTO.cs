using API.BO.Models.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.BO.DTOs.Combo
{
    public class ComboDTO
    {
        public string? Name { get; set; }
        public List<ComboProducts>? Products { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class CreateComboDTO
    {
        public string Name { get; set; }
        public List<ComboProducts> Products { get; set; }
        public decimal Price { get; set; }
    }
}
