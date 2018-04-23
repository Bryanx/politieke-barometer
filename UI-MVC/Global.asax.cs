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
      });
    }
  }
}
