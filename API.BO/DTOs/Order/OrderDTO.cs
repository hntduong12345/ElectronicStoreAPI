using API.BO.Models.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.BO.DTOs.Order
{
    public class OrderDTO
    {
        public decimal TotalPrice { get; set; }
        public string AccountId { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public decimal TruePrice { get; set; }
    }
}
