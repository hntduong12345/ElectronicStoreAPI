using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.BO.DTOs
{
    public class PagingResponseDto<T> where T : class
    {
        public int Total { get; set; } = 0;
        public IList<T> Values { get; set; } = new List<T>();
    }
}
