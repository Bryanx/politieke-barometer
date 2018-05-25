using System;
using System.ComponentModel.DataAnnotations;
using BAR.BL.Domain.Items;
using BAR.UI.MVC.App_GlobalResources;

namespace BAR.UI.MVC.Models {
    public class ItemDTO {
        public int ItemId { get; set; }
        [Display(Name = "Name", ResourceType = typeof(Resources))]    
        public string Name { get; set; }
        
        [Display(Name = "CreationDate", ResourceType = typeof(Resources))]    
        public DateTime CreationDate { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string Description { get; set; }
        
        [Display(Name = "NumberOfFollowers", ResourceType = typeof(Resources))]    
        public int NumberOfFollowers { get; set; }
        public double TrendingPercentage { get; set; }
        
        [Display(Name = "NumberOfMentions", ResourceType = typeof(Resources))]    
        public int NumberOfMentions { get; set; }
        public double SentimentPositive { get; set; }
        public double SentimentNegative { get; set; }
        public double Baseline { get; set; }
        public bool? Subscribed { get; set; }
        
        [Display(Name = "Category", ResourceType = typeof(Resources))]    
        public ItemType? ItemType { get; set; }
        public bool Deleted { get; set; }

        public byte[] Picture { get; set; }
    }
}