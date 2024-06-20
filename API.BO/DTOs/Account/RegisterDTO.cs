using API.BO.Models.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.BO.DTOs.Account
{
    public class RegisterDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
