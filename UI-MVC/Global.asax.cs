using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using BAR.BL.Domain.Items;
using BAR.UI.MVC.Models;
using BAR.BL.Domain.Core;
using Microsoft.Owin.BuilderProperties;

namespace BAR.UI.MVC
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			Mapper.Initialize(cfg =>
			{
				//Mapping items
				cfg.CreateMap<Item, ItemDTO>()
					.ForMember(m => m.ItemId, opt => opt.MapFrom(src => src.ItemId))
					.ForMember(m => m.Name, opt => opt.MapFrom(src => src.Name))
					.ForMember(m => m.Description, opt => opt.MapFrom(src => src.Description))
					.ForMember(m => m.NumberOfMentions, opt => opt.MapFrom(src => src.NumberOfMentions))
					.ForMember(m => m.TrendingPercentage, opt => opt.MapFrom(src => Math.Floor(src.TrendingPercentage)));

				//Mapping customization
				cfg.CreateMap<Customization, CustomizationViewModel>()
					.ForMember(m => m.CustomizationId, opt => opt.MapFrom(src => src.CustomizationId))

					//Mapping Page colors
					.ForMember(m => m.PrimaryColor, opt => opt.MapFrom(src => src.PrimaryColor))
					.ForMember(m => m.SecondairyColor, opt => opt.MapFrom(src => src.SecondairyColor))
					.ForMember(m => m.TertiaryColor, opt => opt.MapFrom(src => src.TertiaryColor))
					.ForMember(m => m.BackgroundColor, opt => opt.MapFrom(src => src.BackgroundColor))
					.ForMember(m => m.TextColor, opt => opt.MapFrom(src => src.TextColor))
					//Mapping Page aliases
					.ForMember(m => m.PersonAlias, opt => opt.MapFrom(src => src.PersonAlias))
					.ForMember(m => m.PersonsAlias, opt => opt.MapFrom(src => src.PersonsAlias))
					.ForMember(m => m.OrganisationAlias, opt => opt.MapFrom(src => src.OrganisationsAlias))
					.ForMember(m => m.OrganisationsAlias, opt => opt.MapFrom(src => src.OrganisationsAlias))
					.ForMember(m => m.ThemeAlias, opt => opt.MapFrom(src => src.ThemeAlias))
					.ForMember(m => m.ThemesAlias, opt => opt.MapFrom(src => src.ThemesAlias))
					//Privacy page
					.ForMember(m => m.PrivacyTitle, opt => opt.MapFrom(src => src.PrivacyTitle))
					.ForMember(m => m.PrivacyText, opt => opt.MapFrom(src => src.PrivacyText))
					//FAQ page
					.ForMember(m => m.FAQTitle, opt => opt.MapFrom(src => src.FAQTitle))
					//Contact page
					.ForMember(m => m.StreetAndHousenumber, opt => opt.MapFrom(src => src.StreetAndHousenumber))
					.ForMember(m => m.Zipcode, opt => opt.MapFrom(src => src.Zipcode))
					.ForMember(m => m.City, opt => opt.MapFrom(src => src.City))
					.ForMember(m => m.Country, opt => opt.MapFrom(src => src.Country))
					.ForMember(m => m.Email, opt => opt.MapFrom(src => src.Email));
			});		
		}
	}
}
