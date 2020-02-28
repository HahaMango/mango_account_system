using System.Threading.Tasks;

namespace MangoAccountSystem.Component
{
	/// <summary>
	/// 邮件组件
	/// </summary>
    public interface IEmailComponent
    {
        Task SendStringAsync(string username,string email,string title,string message);
        Task SendHtmlAsync(string username, string email, string title,string html);
    }
}
