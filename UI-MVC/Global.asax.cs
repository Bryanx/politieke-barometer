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
        cfg.CreateMap<BAR.BL.Domain.Widgets.Widget, UserWidgetDTO>()
          .ForMember(w => w.WidgetId, opt => opt.MapFrom(src => src.WidgetId))
          .ForMember(w => w.Title, opt => opt.MapFrom(src => src.Title))
          .ForMember(w => w.ColumnSpan, opt => opt.MapFrom(src => src.ColumnSpan))
          .ForMember(w => w.RowSpan, opt => opt.MapFrom(src => src.RowSpan))
          .ForMember(w => w.ColumnNumber, opt => opt.MapFrom(src => src.ColumnNumber))
          .ForMember(w => w.RowNumber, opt => opt.MapFrom(src => src.RowNumber))
          .ForMember(w => w.WidgetType, opt => opt.MapFrom(src => src.WidgetType))
          .ForMember(w => w.ItemIds, opt => opt.MapFrom(src => src.Items.Select(i => i.ItemId)));
      });
    }
  }
}
