using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;

namespace MangoAccountSystem.Models
{
	/// <summary>
	/// 登陆视图模型
	/// </summary>
	public class LoginInputModels
	{
		[Required(ErrorMessage = "必须填写用户名")]
		public string UserName { get; set; }

		[DataType(DataType.Password)]
		[Required(ErrorMessage = "必须填写密码")]
		[MinLength(6, ErrorMessage = "请填写不小于6位的密码")]
		public string Password { get; set; }

		public bool OnlyLocalLogin { get; set; }
		public string ReturnUrl { get; set; }
	}
}
