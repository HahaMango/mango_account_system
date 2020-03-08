using MangoAccountSystem.Models;
using System;
using System.Threading.Tasks;
using MangoAccountSystem.Component;

namespace MangoAccountSystem.Service.Imp
{
    public class EmailService : IEmailService
    {
        private readonly IEmailComponent _emailComponent;

		public EmailService(IEmailComponent emailComponent)
        {
            _emailComponent = emailComponent;
        }

        public Task SendEmailConfirmAsync(MangoUser mangoUser)
        {
            throw new NotImplementedException();
        }

        public Task SendSignUpSuccessAsync(MangoUser mangoUser)
        {
            throw new NotImplementedException();
        }
    }
}
