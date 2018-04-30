using System;
using System.Collections.Generic;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;

namespace BAR.UI.MVC.Models {
    public class ItemViewModels {
        public class ItemViewModel : BaseViewModel {
            public IEnumerable<ItemDTO> Items { get; set; }
            public string Json { get; set; }
        }
        
        public class PersonViewModel : BaseViewModel {
            public ItemDTO Person { get; set; }
            public bool Subscribed { get; set; }
            public string District { get; set; }
            public string Level { get; set; }
            public Gender Gender { get; set; }
            public string Site { get; set; }
            public DateTime DateOfBirth { get; set; }
            public Area Area { get; set; }
            public string Position { get; set; }
            public int OrganisationId { get; set; }
            public string OrganisationName { get; set; }
            public List<SocialMediaName> SocialMediaNames { get; set; }
        }
        
        public class OrganisationViewModel : BaseViewModel {
            public ItemDTO Person { get; set; }
            public bool Subscribed { get; set; }
            public string Site { get; set; }
            public List<SocialMediaName> SocialMediaNames { get; set; }
        }
        
    }
}