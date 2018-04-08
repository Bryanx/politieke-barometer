using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using BAR.BL.Domain.Items;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using static BAR.UI.MVC.Models.ItemViewModels;

namespace BAR.UI.MVC.Controllers {
    public class HomeController : Controller {
        private const string INDEX_PAGE_TITLE = "Politieke Barometer";
        private IItemManager itemMgr = new ItemManager();
        private UserManager userManager = new UserManager();
        private readonly IMapper mapper;

        [AllowAnonymous]
        public ActionResult Index() {
            IEnumerable<ItemDTO> people = mapper.Map(itemMgr.GetAllItems(), new List<ItemDTO>());
            ItemViewModel itemViewModel = new ItemViewModel() {
                PageTitle = INDEX_PAGE_TITLE,
                User = User.Identity.IsAuthenticated ? userManager.GetUser(User.Identity.GetUserId()) : null,
                People = people
            };
            return View("Index", "~/Views/Shared/Layouts/_HomeLayout(temp).cshtml", itemViewModel);
        }
    }
}