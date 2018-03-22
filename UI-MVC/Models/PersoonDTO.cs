using System;
using System.Collections.Generic;
using BAR.BL.Domain;
using BAR.BL.Domain.Data;

namespace BAR.UI.MVC.Models {
    public class PersoonDTO {
        public int ItemId { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string Description { get; set; }
        public int NumberOfFollowers { get; set; }
        public double TrendingPercentage { get; set; }
        public int NumberOfMentions { get; set; }
        public double Baseline { get; set; }
    }
}