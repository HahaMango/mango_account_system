using MangoAccountSystem.Models;
using System.Threading.Tasks;

namespace MangoAccountSystem.Service
{
    /// <summary>
    /// 发送邮件服务
    /// </summary>
    public interface IEmailService
    {
        Task SendSignUpSuccessAsync(MangoUser mangoUser);
        Task SendEmailConfirmAsync(MangoUser mangoUser);
    }
}
