using System;
using System.Linq;
using System.Timers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Configuration;
using AutoMapper;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Widgets;
using BAR.UI.MVC.Models;
using BAR.BL.Domain.Core;
using Microsoft.Owin.BuilderProperties;
using BAR.UI.MVC.Controllers.api;
using BAR.BL.Managers;
using BAR.BL.Domain.Data;

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
				  .ForMember(m => m.NumberOfMentions, opt => opt.MapFrom(src => src.NumberOfMentions))
				  .ForMember(m => m.SentimentNegative, opt => opt.MapFrom(src => src.SentimentNegative))
				  .ForMember(m => m.SentimentPositive, opt => opt.MapFrom(src => src.SentimentPositve))
				  .ForMember(m => m.TrendingPercentage, opt => opt.MapFrom(src => Math.Floor(src.TrendingPercentage)));

				//Mapping UserWidgets
				cfg.CreateMap<UserWidget, UserWidgetDTO>()
				  .ForMember(w => w.WidgetId, opt => opt.MapFrom(src => src.WidgetId))
				  .ForMember(w => w.Title, opt => opt.MapFrom(src => src.Title))
				  .ForMember(w => w.ColumnSpan, opt => opt.MapFrom(src => src.ColumnSpan))
				  .ForMember(w => w.RowSpan, opt => opt.MapFrom(src => src.RowSpan))
				  .ForMember(w => w.ColumnNumber, opt => opt.MapFrom(src => src.ColumnNumber))
				  .ForMember(w => w.RowNumber, opt => opt.MapFrom(src => src.RowNumber))
				  .ForMember(w => w.WidgetType, opt => opt.MapFrom(src => src.WidgetType))
				  .ForMember(w => w.DashboardId, opt => opt.MapFrom(src => src.Dashboard.DashboardId))
				  .ForMember(w => w.ItemIds, opt => opt.MapFrom(src => src.Items.Select(i => i.ItemId).ToList()));

				//Mapping widgets
				cfg.CreateMap<Widget, UserWidgetDTO>()
				  .ForMember(w => w.WidgetId, opt => opt.MapFrom(src => src.WidgetId))
				  .ForMember(w => w.Title, opt => opt.MapFrom(src => src.Title))
				  .ForMember(w => w.ColumnSpan, opt => opt.MapFrom(src => src.ColumnSpan))
				  .ForMember(w => w.RowSpan, opt => opt.MapFrom(src => src.RowSpan))
				  .ForMember(w => w.ColumnNumber, opt => opt.MapFrom(src => src.ColumnNumber))
				  .ForMember(w => w.RowNumber, opt => opt.MapFrom(src => src.RowNumber))
				  .ForMember(w => w.WidgetType, opt => opt.MapFrom(src => src.WidgetType))
				  .ForMember(w => w.ItemIds, opt => opt.MapFrom(src => src.Items.Select(i => i.ItemId).ToList()));

				//Mapping perons
				cfg.CreateMap<Person, ItemViewModels.PersonViewModel>()
				  .ForMember(p => p.District, opt => opt.MapFrom(src => src.District))
				  .ForMember(p => p.Town, opt => opt.MapFrom(src => src.Area.Residence))
				  .ForMember(p => p.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
				  .ForMember(p => p.Gender, opt => opt.MapFrom(src => src.Gender))
				  .ForMember(p => p.Site, opt => opt.MapFrom(src => src.Site))
				  .ForMember(p => p.Level, opt => opt.MapFrom(src => src.Level))
				  .ForMember(p => p.OrganisationId, opt => opt.MapFrom(src => src.Organisation.ItemId))
				  .ForMember(p => p.OrganisationName, opt => opt.MapFrom(src => src.Organisation.Name))
				  .ForMember(p => p.Position, opt => opt.MapFrom(src => src.Position))
				  .ForMember(p => p.SocialMediaNames, opt => opt.MapFrom(src => src.SocialMediaNames));

				//Mapping organisations
				cfg.CreateMap<Organisation, ItemViewModels.OrganisationViewModel>()
				  .ForMember(p => p.Site, opt => opt.MapFrom(src => src.Site))
				  .ForMember(p => p.PageTitle, opt => opt.MapFrom(src => src.Name))
				  .ForMember(p => p.SocialMediaNames, opt => opt.MapFrom(src => src.SocialMediaUrls));

				//Mapping themes
				cfg.CreateMap<Theme, ItemViewModels.ThemeViewModel>()
					.ForMember(p => p.Keywords, opt => opt.MapFrom(src => src.Keywords));

				//Mapping widgetdata
				cfg.CreateMap<WidgetData, WidgetDataDTO>()
					.ForMember(p => p.WidgetDataId, opt => opt.MapFrom(src => src.WidgetDataId))
					.ForMember(p => p.GraphValues, opt => opt.MapFrom(src => src.GraphValues))
					.ForMember(p => p.KeyValue, opt => opt.MapFrom(src => src.KeyValue))
					.ForMember(p => p.WidgetId, opt => opt.MapFrom(src => src.Widget.WidgetId));

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
				
				//Mapping user widgets
				cfg.CreateMap<UserWidget, UserWidgetViewModel>()
				  .ForMember(w => w.WidgetId, opt => opt.MapFrom(src => src.WidgetId))
				  .ForMember(w => w.Title, opt => opt.MapFrom(src => src.Title))
				  .ForMember(w => w.ColumnSpan, opt => opt.MapFrom(src => src.ColumnSpan))
				  .ForMember(w => w.RowSpan, opt => opt.MapFrom(src => src.RowSpan))
				  .ForMember(w => w.ColumnNumber, opt => opt.MapFrom(src => src.ColumnNumber))
				  .ForMember(w => w.RowNumber, opt => opt.MapFrom(src => src.RowNumber))
				  .ForMember(w => w.WidgetType, opt => opt.MapFrom(src => src.WidgetType))
				  .ForMember(w => w.DashboardId, opt => opt.MapFrom(src => src.Dashboard.DashboardId))
				  .ForMember(w => w.ItemIds, opt => opt.MapFrom(src => src.Items.Select(i => i.ItemId).ToList()));
			});

      double TimerIntervalInMilliseconds = 60000;
      Timer timer = new Timer(TimerIntervalInMilliseconds);
      timer.Enabled = true;
      timer.Elapsed += new ElapsedEventHandler(timerElapsed);
      timer.Start();
    }
    private void timerElapsed(object sender, ElapsedEventArgs e)
    {
        DataManager dataManager = new DataManager();
        List<DataSource> datasources = dataManager.GetAllDataSources().ToList();
        for (int i=0;i<datasources.Count();i++)
            {
                DataSource datasource = datasources[i];
                if (datasource.LastTimeChecked.AddMinutes(datasource.Interval) < DateTime.Now)
                {
                    DataApiController controller = new DataApiController();
                    controller.Synchronize(i);
                    dataManager.ChangeLastTimeChecked(i, DateTime.Now);
                }
            }
    }      
  }
}
