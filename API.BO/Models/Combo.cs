using API.BO.Models.Documents;
using MongoDB.Bson;
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
        public string Name { get; set; }
        public List<ComboProducts> Products { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable {  get; set; }

        public Combo(int comboId, string name, List<ComboProducts> products, decimal price)
        {
            //ComboId = ObjectId.GenerateNewId().ToString();
            ComboId = comboId;
            Name = name;
            Products = products;
            Price = price;
            IsAvailable = true;
        }
    }
}
