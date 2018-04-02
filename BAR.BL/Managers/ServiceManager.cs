using Microsoft.AspNet.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Managers
{
  /// <summary>
  /// Configures Identity with our SendGrid API.
  /// </summary>
  public class EmailService : IIdentityMessageService
  {
    public async Task SendAsync(IdentityMessage message)
    {
      var clientKey = ConfigurationManager.AppSettings["SendGridClient"];
      var client = new SendGridClient(clientKey);
      var from = new EmailAddress("Politiek@barometer.be", "Politieke barometer");
      var subject = message.Subject;
      var to = new EmailAddress(message.Destination, "Politieke barometer gebruiker");
      var plainTextContent = "";
      var htmlContent = message.Body;
      var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
      var response = await client.SendEmailAsync(msg);
    }
  }
}
