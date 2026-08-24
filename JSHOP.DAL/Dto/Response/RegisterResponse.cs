using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSHOP.DAL.Dto.Response
{
    public class RegisterResponse
    {
        public string Message { get; set; }
        public List<string> Errors {get; set; }
    }
}
