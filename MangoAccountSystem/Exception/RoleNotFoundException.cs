using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MangoAccountSystem.Exception
{
	/// <summary>
	/// 找不到角色异常
	/// </summary>
	public class RoleNotFoundException : System.Exception
	{
		public RoleNotFoundException()
		{

		}

		public RoleNotFoundException(string message) : base(message)
		{

		}

		public RoleNotFoundException(string message, System.Exception innerException) : base(message, innerException)
		{
		}
	}
}
