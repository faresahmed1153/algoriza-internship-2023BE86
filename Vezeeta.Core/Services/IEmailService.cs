using Vezeeta.Core.Dtos;

namespace Vezeeta.Core.Services
{
	public interface IEmailService
	{
		public Task SendEmail(EmailDto email);
	}
}
