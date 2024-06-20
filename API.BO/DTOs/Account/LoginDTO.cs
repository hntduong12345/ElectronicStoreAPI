using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.BO.DTOs.Account
{
    public class LoginDTO
    {
        //Can be email or phone
        public string Input { get; set; }
        public string Password { get; set; }
    }
}
