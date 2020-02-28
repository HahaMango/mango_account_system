using System.ComponentModel.DataAnnotations;
using MangoAccountSystem.Attribute;

namespace MangoAccountSystem.Models
{
	/// <summary>
	/// 注册视图对象
	/// </summary>
	public class SignUpInputModel
    {
		[Required(ErrorMessage = "必须填写用户名")]
		[StringLength(20,ErrorMessage = "用户名长度最长为20")]
        public string UserName { get; set; }

		[Required(ErrorMessage = "必须填写邮箱")]
		[EmailAddress(ErrorMessage = "邮箱格式错误")]
        public string Email { get; set; }

		[DataType(DataType.Password)]
		[Required(ErrorMessage = "必须填写密码")]
		[MinLength(6,ErrorMessage = "请填写不小于6位的密码")]
        public string Password { get; set; }

		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "密码要一致")]
        public string PasswordConfirm { get; set; }

		[Bool(true,ErrorMessage = "请勾选同意")]
        public bool IsAgree { get; set; }
    }
}
