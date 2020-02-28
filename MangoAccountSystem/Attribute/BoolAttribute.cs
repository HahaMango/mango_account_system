using System.ComponentModel.DataAnnotations;

namespace MangoAccountSystem.Attribute
{
	/// <summary>
	/// 布尔类型的模型验证特性
	/// </summary>
	public class BoolAttribute : ValidationAttribute
	{
		private readonly bool _expect;

		public BoolAttribute(bool expect)
		{
			_expect = expect;
		}

		public override bool IsValid(object value)
		{
			bool b = (bool)value;
			if (b == _expect)
				return true;
			else
				return false;
		}
	}
}
