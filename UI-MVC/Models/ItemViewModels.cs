using System;
using System.Collections.Generic;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;
using System.Web.Mvc;

namespace BAR.UI.MVC.Models {
    public class ItemViewModels {
        public class ItemViewModel : BaseViewModel {
            public IEnumerable<ItemDTO> Items { get; set; }
            public string Json { get; set; }
        }

				public class ItemCreateViewModel : BaseViewModel
				{
						public IEnumerable<ItemDTO> Items { get; set; }
						public string Json { get; set; }
						public int OrganisationId { get; set; }
						public IEnumerable<SelectListItem> Organisations { get; set; }
				}

				public class PersonViewModel : BaseViewModel {
            public ItemDTO Item { get; set; }
            public List<PersonViewModel> PeopleFromSameOrg { get; set; }
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
            public int RankNumberOfMentions  { get; set; }
            public int RankTrendingPercentage  { get; set; }
            
        }
        
        public class PersonViewModels : BaseViewModel {
            public List<PersonViewModel> Persons { get; set; }
            public string Json { get; set; }
        }
        
        public class OrganisationViewModel : BaseViewModel {
            public ItemDTO Organisation { get; set; }
            public bool Subscribed { get; set; }
            public string Site { get; set; }
            public List<SocialMediaName> SocialMediaNames { get; set; }
            public List<PersonViewModel> MemberList { get; set; }
        }
        
        public class ThemeViewModel : BaseViewModel
        {
            public ItemDTO Theme { get; set; }
            public bool Subscribed { get; set; }
            public List<Keyword> Keywords { get; set; }
        }
    }
}