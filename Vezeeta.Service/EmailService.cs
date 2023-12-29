using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using Vezeeta.Core.Dtos;
using Vezeeta.Core.Services;
namespace Vezeeta.Service
{
	public class EmailService : IEmailService
	{
		private readonly EmailDetailsDto _options;


		public EmailService(IOptions<EmailDetailsDto> options)
		{
			_options = options.Value;
		}

		public async Task SendEmail(EmailDto email)
		{
			var mail = new MimeMessage
			{

				Sender = MailboxAddress.Parse(_options.Email),
				Subject = email.Subject
			};
			mail.To.Add(MailboxAddress.Parse(email.To));
			var builder = new BodyBuilder();
			builder.HtmlBody = email.Body;
			mail.Body = builder.ToMessageBody();
			mail.From.Add(new MailboxAddress(_options.DisplayName, _options.Email));
			using var smtp = new SmtpClient();
			await smtp.ConnectAsync(_options.Host, _options.Port, MailKit.Security.SecureSocketOptions.StartTls);
			await smtp.AuthenticateAsync(_options.Email, _options.Password);
			await smtp.SendAsync(mail);
			await smtp.DisconnectAsync(true);
		}



	}
}
