using API.BO.Models.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.BO.Models
{
    public class Combo
    {
        public int ComboId { get; set; }
        public List<ComboProducts> Products { get; set; }
        public decimal Price { get; set; }
    }
}
