using Microsoft.AspNet.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Configuration;
using System.Threading.Tasks;

namespace BAR.BL.Managers
{
	/// <summary>
	/// Configures Identity with our SendGrid API.
	/// </summary>
	public class EmailService : IIdentityMessageService
	{
		/// <summary>
		/// Sends an async message from our platform to a specific email adress
		/// that is mentioned in the IdentityMessage
		/// </summary>
		public async Task SendAsync(IdentityMessage message)
		{
			var clientKey = ConfigurationManager.AppSettings["SendGridClient"];
			SendGridClient client = new SendGridClient(clientKey);
			EmailAddress from = new EmailAddress("Politiek@barometer.be", "Politieke barometer");
			string subject = message.Subject;
			EmailAddress to = new EmailAddress(message.Destination, "Politieke barometer gebruiker");
			string plainTextContent = "";
			string htmlContent = message.Body;
			SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
			Response response = await client.SendEmailAsync(msg);
		}
	}
}
