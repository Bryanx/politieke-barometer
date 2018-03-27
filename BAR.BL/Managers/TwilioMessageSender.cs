using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace BAR.BL.Managers
{
  public class TwilioMessageSender : ITwilioMessageSender
  {
    public TwilioMessageSender()
    {
      TwilioClient.Init(ConfigurationManager.AppSettings["SMSAccountIdentification"], ConfigurationManager.AppSettings["SMSAccountPassword"]);
    }

    public async Task SendMessageAsync(string to, string from, string body)
    {
      await MessageResource.CreateAsync(new PhoneNumber(to),
                                        from: new PhoneNumber(from),
                                        body: body);
    }
  }
}
