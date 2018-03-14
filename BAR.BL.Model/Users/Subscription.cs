using BAR.BL.Domain.Items;
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
        public Item SubscribedItem { get; set; }
<<<<<<< HEAD
        public User SubscribedUser { get; set; }       
=======
        public User SubscribedUser { get; set; }
>>>>>>> 678c8c282e1df7658ffe27ee8959cb19c0820383
    }
}
