using API.BO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Service.Interface
{
    public interface ITokenService
    {
        public string CreateToken(Account account);
    }
}
