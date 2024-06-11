﻿using API.BO.Models.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.BO.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public decimal TotalPrice { get; set; }
        public int AccountId { get; set; }
        public List<OrderDetail> OrderDetails { get; set; } 
        public string Status { get; set; }
        public decimal TruePrice { get; set; }
    }
}