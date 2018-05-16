﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using BAR.BL.Domain.Items;
using BAR.BL.Managers;
using BAR.UI.MVC.App_GlobalResources;
using BAR.UI.MVC.Helpers;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;
using static BAR.UI.MVC.Models.ItemViewModels;

namespace BAR.UI.MVC.Controllers
{
  /// <summary>
  /// This controller is used for managing the homepage.
  /// </summary>
  public class HomeController : LanguageController
  {
    private const string INDEX_PAGE_TITLE = "Politieke Barometer";
    private IItemManager itemManager;
    private IUserManager userManager;
    private ISubplatformManager subplatformManager;

    /// <summary>
    /// Landing page for logged-in and non-logged-in users.
    /// </summary>
    [AllowAnonymous]
    public ActionResult Index()
    {
      userManager = new UserManager();
      itemManager = new ItemManager();

      List<Item> persons = itemManager.GetMostTrendingItemsForType(ItemType.Person, 4, true).ToList();

      
      List<Person> personsWDetails = new List<Person>();
      itemManager.GetMostTrendingItemsForType(ItemType.Person, 4, true).ForEach(i => personsWDetails.Add(itemManager.GetPersonWithDetails(i.ItemId)));

      
      List<PersonViewModel> personViewModels = Mapper.Map(personsWDetails, new List<PersonViewModel>());
      for (int i = 0; i < persons.Count; i++) {
        personViewModels[i].Item = Mapper.Map(persons[i], new ItemDTO());
      }
      

      //Assembling the view
      return View(new ItemViewModel
      {
        PageTitle = INDEX_PAGE_TITLE,
        User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null,
        Items = Mapper.Map<IList<Item>, IList<ItemDTO>>(itemManager.GetAllItems().ToList()),
        WeeklyReviewModel = new WeeklyReviewModel
        {
          WeeklyItems = Mapper.Map(itemManager.GetMostTrendingItems(4, true), new List<ItemDTO>()),
          PViewModel = personViewModels
        }
      });
    }
    /// <summary>
    /// Privacy page for logged-in and non-logged-in users.
    /// </summary>
    [AllowAnonymous]
    public ActionResult Privacy()
    {
      userManager = new UserManager();

      //Assembling the view
      return View(new BaseViewModel()
      {
        PageTitle = Resources.PrivacyAndSafety,
        User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null
      });
    }

    /// <summary>
    /// FAQ page for logged-in and non-logged-in users.
    /// </summary>
    [AllowAnonymous]
    public ActionResult Faq()
    {
      userManager = new UserManager();

      //Assembling the view
      return View(new BaseViewModel()
      {
        PageTitle = Resources.QuestionAndAnswer,
        User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null
      });
    }

    /// <summary>
    /// Saves the language preferences in a cookie.
    /// </summary>
    public ActionResult SetCulture(string culture)
    {
      // Validate input
      culture = LanguageHelper.GetImplementedCulture(culture);
      // Save culture in a cookie
      HttpCookie cookie = Request.Cookies["_culture"];
      if (cookie != null)
        cookie.Value = culture;   // update cookie value
      else
      {
        cookie = new HttpCookie("_culture");
        cookie.Value = culture;
        cookie.Expires = DateTime.Now.AddYears(1);
      }
      Response.Cookies.Add(cookie);
      return Redirect(Request.UrlReferrer.ToString());
    }

    /// <summary>
    /// Returns a javascript object with language resources.
    /// This way local resources can be used in javascript files.
    /// </summary>
    public ActionResult GetResources()
    {
      Response.ContentType = "text/javascript";
      return View();
    }

  }
}