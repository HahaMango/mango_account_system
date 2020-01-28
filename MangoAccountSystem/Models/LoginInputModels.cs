using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MangoAccountSystem.Models
{
    public class LoginInputModels
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
    }
}
