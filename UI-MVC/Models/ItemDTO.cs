using System;
using System.Collections.Generic;
using BAR.BL.Domain;
using BAR.BL.Domain.Data;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;

namespace BAR.UI.MVC.Models {
    public class ItemDTO {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string Description { get; set; }
        public int NumberOfFollowers { get; set; }
        public double TrendingPercentage { get; set; }
        public int NumberOfMentions { get; set; }
        public double Baseline { get; set; }
        public bool? Subscribed { get; set; }
        public ItemType? ItemType { get; set; }
        public bool Deleted { get; set; }
    }
}