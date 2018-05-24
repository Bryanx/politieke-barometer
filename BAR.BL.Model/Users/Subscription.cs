using BAR.BL.Domain.Items;
using System;
using System.Collections.Generic;

namespace BAR.BL.Domain.Users
{
    public class Subscription
    {
        public int SubscriptionId { get; set; }
        public int Threshold { get; set; }
        public DateTime? DateSubscribed { get; set; }
        public ICollection<SubAlert> Alerts { get; set; }
        public Item SubscribedItem { get; set; }
        public User SubscribedUser { get; set; }       
    }
}
