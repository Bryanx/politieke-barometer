using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Domain.Users
{
    public class Subscription
    {
        public int SubscriptionId { get; set; }
        public int Treshhold { get; set; }
        public DateTime DateSubscribed { get; set; }
        public List<Alert> Alerts { get; set; }

        public void AddAlert(Alert alert)
        {
            Alerts.Add(alert);
        }
    }
}
