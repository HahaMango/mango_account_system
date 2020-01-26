using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MangoAccountSystem.Exception
{
    public class UserNotFoundException : System.Exception
    {
        public UserNotFoundException()
        {

        }

        public UserNotFoundException(string userName):base($"无法找到名为：{userName} 的用户")
        {

        }

        public UserNotFoundException(int userid) : base($"无法找到id为：{userid} 的用户")
        {

        }
    }
}
