using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MangoAccountSystem.Exception
{
    public class RoleNotFoundException : System.Exception
    {
        public RoleNotFoundException()
        {

        }

        public RoleNotFoundException(string message) : base(message)
        {

        }
    }
}
