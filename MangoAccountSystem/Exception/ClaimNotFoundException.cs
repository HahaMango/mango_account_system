using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MangoAccountSystem.Exception
{
	/// <summary>
	/// 找不到声明异常
	/// </summary>
	public class ClaimNotFoundException : System.Exception
	{
		public ClaimNotFoundException()
		{

		}

		public ClaimNotFoundException(string claimType, string claimValue) : base($"找不到声明为：(Type:{claimType}，Value:{claimValue})")
		{

		}

		public ClaimNotFoundException(string message) : base(message)
		{
		}

		public ClaimNotFoundException(string message, System.Exception innerException) : base(message, innerException)
		{
		}
	}
}
