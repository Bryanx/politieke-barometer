using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Widgets;
using BAR.UI.MVC.Models;
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
      Mapper.Initialize(cfg => {
        cfg.CreateMap<Item, ItemDTO>()
          .ForMember(m => m.ItemId, opt => opt.MapFrom(src => src.ItemId))
          .ForMember(m => m.Name, opt => opt.MapFrom(src => src.Name))
          .ForMember(m => m.NumberOfMentions, opt => opt.MapFrom(src => src.NumberOfMentions))
          .ForMember(m => m.SentimentNegative, opt => opt.MapFrom(src => src.SentimentNegative*100))
          .ForMember(m => m.SentimentPositive, opt => opt.MapFrom(src => src.SentimentPositve*100))
          .ForMember(m => m.TrendingPercentage, opt => opt.MapFrom(src => Math.Floor(src.TrendingPercentage)));
        cfg.CreateMap<UserWidget, UserWidgetDTO>()
          .ForMember(w => w.WidgetId, opt => opt.MapFrom(src => src.WidgetId))
          .ForMember(w => w.Title, opt => opt.MapFrom(src => src.Title))
          .ForMember(w => w.ColumnSpan, opt => opt.MapFrom(src => src.ColumnSpan))
          .ForMember(w => w.RowSpan, opt => opt.MapFrom(src => src.RowSpan))
          .ForMember(w => w.ColumnNumber, opt => opt.MapFrom(src => src.ColumnNumber))
          .ForMember(w => w.RowNumber, opt => opt.MapFrom(src => src.RowNumber))
          .ForMember(w => w.WidgetType, opt => opt.MapFrom(src => src.WidgetType))
          .ForMember(w => w.DashboardId, opt => opt.MapFrom(src => src.Dashboard.DashboardId));
        cfg.CreateMap<Widget, UserWidgetDTO>()
          .ForMember(w => w.WidgetId, opt => opt.MapFrom(src => src.WidgetId))
          .ForMember(w => w.Title, opt => opt.MapFrom(src => src.Title))
          .ForMember(w => w.ColumnSpan, opt => opt.MapFrom(src => src.ColumnSpan))
          .ForMember(w => w.RowSpan, opt => opt.MapFrom(src => src.RowSpan))
          .ForMember(w => w.ColumnNumber, opt => opt.MapFrom(src => src.ColumnNumber))
          .ForMember(w => w.RowNumber, opt => opt.MapFrom(src => src.RowNumber))
          .ForMember(w => w.WidgetType, opt => opt.MapFrom(src => src.WidgetType))
          .ForMember(w => w.ItemIds, opt => opt.MapFrom(src => src.Items.Select(i => i.ItemId)));
        cfg.CreateMap<Person, ItemViewModels.PersonViewModel>()
          .ForMember(p => p.District, opt => opt.MapFrom(src => src.District))
          .ForMember(p => p.Area, opt => opt.MapFrom(src => src.Area))
          .ForMember(p => p.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
          .ForMember(p => p.Gender, opt => opt.MapFrom(src => src.Gender))
          .ForMember(p => p.Site, opt => opt.MapFrom(src => src.Site))
          .ForMember(p => p.Level, opt => opt.MapFrom(src => src.Level))
          .ForMember(p => p.OrganisationId, opt => opt.MapFrom(src => src.Organisation.ItemId))
          .ForMember(p => p.OrganisationName, opt => opt.MapFrom(src => src.Organisation.Name))
          .ForMember(p => p.Position, opt => opt.MapFrom(src => src.Position))
          .ForMember(p => p.SocialMediaNames, opt => opt.MapFrom(src => src.SocialMediaNames));
        cfg.CreateMap<Organisation, ItemViewModels.OrganisationViewModel>()
          .ForMember(p => p.Site, opt => opt.MapFrom(src => src.Site))
          .ForMember(p => p.SocialMediaNames, opt => opt.MapFrom(src => src.SocialMediaUrls));
        cfg.CreateMap<WidgetData, WidgetDataDTO>()
          .ForMember(p => p.WidgetDataId, opt => opt.MapFrom(src => src.WidgetDataId))
          .ForMember(p => p.GraphValues, opt => opt.MapFrom(src => src.GraphValues))
          .ForMember(p => p.KeyValue, opt => opt.MapFrom(src => src.KeyValue))
          .ForMember(p => p.WidgetId, opt => opt.MapFrom(src => src.Widget.WidgetId));
      });
    }
  }
}
