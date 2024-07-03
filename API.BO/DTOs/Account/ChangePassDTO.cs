using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.BO.DTOs.Account
{
    public class ChangePassDTO
    {
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }
}
