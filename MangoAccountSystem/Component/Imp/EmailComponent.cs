using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace MangoAccountSystem.Component.Imp
{
	public class EmailComponent : IEmailComponent
	{
		public Task SendHtmlAsync(string username, string email, string title, string html)
		{
			throw new NotImplementedException();
		}

		public async Task SendStringAsync(string username, string email, string title, string message)
		{
			MimeMessage mes = new MimeMessage();

			MailboxAddress from = new MailboxAddress("Mango User System", "932104843@qq.com");
			MailboxAddress to = new MailboxAddress(username, email);

			mes.From.Add(from);
			mes.To.Add(to);
			mes.Subject = title;

			mes.Body = new TextPart("plain") { Text = message };

			using (var client = new SmtpClient())
			{
				await client.ConnectAsync("smtp.exmail.qq.com", 465, true);
				await client.AuthenticateAsync("932104843@qq.com", "czh228887474/");
				await client.SendAsync(mes);
				await client.DisconnectAsync(true);
			}
		}
	}
}
