using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BAR.UI.MVC.Helpers
{
  public class ControllerHelpers
  {
    public async Task<bool> SendPushNotificationAsync(string to, string title, string body)
    {
      var serverKey = string.Format("key={0}", "AAAA5ymxBWA:APA91bEiU1oM6esTAqJCpMGDBGnVzI71BEMKxP2siyaj59xiu4e3u3VfbbBAT7NXq-5ey8ErSdgnLMXLDsQTbsB8ZAXFmsKLKcXai3c8yCc1SMw4j0XK1rkCCwe6xnThOTH3-RVomrbM");

      var data = new
      {
        to,
        notification = new { title, body }
      };

      var jsonBody = JsonConvert.SerializeObject(data);

      using (var httpRequest = new HttpRequestMessage(HttpMethod.Post, "https://fcm.googleapis.com/fcm/send"))
      {
        httpRequest.Headers.TryAddWithoutValidation("Authorization", serverKey);
        httpRequest.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        using (var httpClient = new HttpClient())
        {
          var result = await httpClient.SendAsync(httpRequest);

          if (result.IsSuccessStatusCode)
          {
            return true;
          }
          else
          {
            return false;
          }
        }
      }
    }
  }
}