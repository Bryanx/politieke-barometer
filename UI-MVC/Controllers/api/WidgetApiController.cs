using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web.Http;
using AutoMapper;
using BAR.BL.Domain.Data;
using BAR.BL.Domain.Widgets;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using BAR.BL.Domain.Items;
using BAR.UI.MVC.App_GlobalResources;
using BAR.UI.MVC.Attributes;
using Newtonsoft.Json;
using WebGrease.Css.Extensions;

namespace BAR.UI.MVC.Controllers.api
{
	/// <summary>
	/// This class is used to transfer all widget information from UI to the managers.
	/// The api calls are in ajax requests on the dashboard index page.
	/// </summary>
	public class WidgetApiController : ApiController
	{
		private IWidgetManager widgetManager;
		private IItemManager itemManager;

		/// <summary>
		///Reads all widgets for a user dashboard and returns them
		/// </summary>
		public IHttpActionResult GetUserWidgets()
		{
			widgetManager = new WidgetManager();
			Dashboard dash = widgetManager.GetDashboard(User.Identity.GetUserId());

			try {
				List<UserWidget> widgets = widgetManager.GetWidgetsForDashboard(dash.DashboardId).ToList();

				if (widgets == null || widgets.Count() == 0)
					return StatusCode(HttpStatusCode.NoContent);

				return Ok(Mapper.Map(widgets, new List<UserWidgetDTO>()));

			} catch (Exception e) {
				return BadRequest(e.Message);
			}
		}
		
		/// <summary>
		///Reads all widgets for an item and returns them.
		/// </summary>
		[HttpGet]
		[Route("api/GetItemWidgets/{itemId}")]
		public IHttpActionResult GetItemWidgets(int itemId)
		{
			widgetManager = new WidgetManager();
			IEnumerable<Widget> widgets = widgetManager.GetItemwidgetsForItem(itemId);

			if (widgets == null || widgets.Count() == 0)
				return StatusCode(HttpStatusCode.NoContent);

			List<UserWidgetDTO> uWidgets = Mapper.Map(widgets, new List<UserWidgetDTO>());
			foreach (UserWidgetDTO widgetDto in uWidgets) widgetDto.DashboardId = -1;

            return Ok(uWidgets);
		}

		/// <summary>
		/// Retrieves the graph data for a given itemId and widget.
		/// </summary>
		[HttpGet]
		[Route("api/GetGraphs/{itemId}/{widgetId}")]
		public IHttpActionResult GetGraphs(int itemId, int widgetId)
		{
			widgetManager = new WidgetManager();
			itemManager = new ItemManager();
			
			//Get widgets for item
			IEnumerable<Widget> widgets = widgetManager.GetAllWidgetsWithAllDataForItem(itemId);

			//Get keyvalue for the widgetid.
			string keyValue = widgetManager.GetWidgetWithAllData(widgetId)?.WidgetDatas.FirstOrDefault()?.KeyValue;
			if (keyValue == null) keyValue = widgetManager.GetWidgetWithAllData(widgetId)?.PropertyTags.FirstOrDefault()?.Name;
			
			try
			{
				IEnumerable<WidgetData> widgetDatas = widgets.FirstOrDefault(widget => widget.WidgetDatas.Any(data => data.KeyValue == keyValue)).WidgetDatas;
				IEnumerable<WidgetDataDTO> widgetDataDtos = Mapper.Map(widgetDatas, new List<WidgetDataDTO>());

				//Get item name
				widgetDataDtos.First().ItemName = itemManager.GetItem(itemId).Name;

				return Ok(widgetDataDtos);
#pragma warning disable CS0168 // Variable is declared but never used
			}
			catch (Exception e)
#pragma warning restore CS0168 // Variable is declared but never used
			{
				IEnumerable<WidgetData> widgetDatas = new List<WidgetData>();
				IEnumerable<WidgetDataDTO> widgetDataDtos = Mapper.Map(widgetDatas, new List<WidgetDataDTO>());

				return Ok(widgetDataDtos);
			}
		}

		/// <summary>
		/// Transfers a Widget to the dashboard of a user.
		/// The given ItemWidget will be copied to a UserWidget.
		/// </summary>
		[HttpPost]
		[Route("api/MoveWidget/")]
		[Authorize]
		public IHttpActionResult MoveWidgetToDashboard([FromBody] UserWidgetDTO model)
		{
			widgetManager = new WidgetManager();
			widgetManager.MoveWidgetToDashBoard(model.WidgetId, model.GraphType, model.ItemIds, User.Identity.GetUserId());
			return StatusCode(HttpStatusCode.NoContent);
		}
		
		/// <summary>
		/// Creates a new Widget from the body of the post request.
		/// If the widget already exists, returns 409 Conflict
		/// </summary>
		[HttpPost]
		[Route("api/NewWidget/")]
		[Authorize]
		public IHttpActionResult AddNewWidgetToDashboard([FromBody] AddWidgetDTO model)
		{
			UnitOfWorkManager uowManager = new UnitOfWorkManager();
			widgetManager = new WidgetManager(uowManager);
			itemManager = new ItemManager(uowManager);

			Dashboard dash = widgetManager.GetDashboard(User.Identity.GetUserId());

			List<PropertyTag> propertyTags = new List<PropertyTag> {new PropertyTag() {Name = model.PropertyTag}};

			List<Item> items = itemManager.GetAllItems().Where(i => model.ItemIds.Contains(i.ItemId)).ToList();

			if (string.IsNullOrEmpty(model.Title)) model.Title = Resources.Title;
			
			widgetManager.AddWidget(WidgetType.GraphType, model.Title, 0,
				0, propertyTags, graphType: model.GraphType, dashboardId:dash.DashboardId, items:items);
			
			uowManager.Save();

			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Updates existing widgets.
		/// </summary>
		[HttpPost]
		[Route("api/UpdateWidget/")]
		public IHttpActionResult UpdateWidget([FromBody] UserWidgetDTO[] widgets)
		{
			widgetManager = new WidgetManager();
			
			foreach (UserWidgetDTO widget in widgets) {
				if (widget == null)
					return BadRequest("No widget given");
				if (widgetManager.GetWidget(widget.WidgetId) == null)
					return NotFound();

				widgetManager.ChangeWidgetDetails(widget.WidgetId, widget.RowNumber, widget.ColumnNumber, widget.ItemIds.ToList(),
						widget.RowSpan, widget.ColumnSpan, widget.GraphType);
			}
			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Change the title of a widget
		/// </summary>
		[HttpPost]
		[Route("api/WidgetApi/{id}/{title}")]
		public IHttpActionResult PutName(int id, string title)
		{
			widgetManager = new WidgetManager();

			if (widgetManager.GetWidget(id) == null)
				return NotFound();
			if (!ModelState.IsValid)
				return BadRequest(ModelState);
			
			widgetManager.ChangeWidgetTitle(id, title);
			return StatusCode(HttpStatusCode.NoContent);
		}
		
		/// <summary>
		/// Deletes a widget
		/// </summary>
		[HttpPost]
		[Route("api/WidgetApi/Delete/{id}")]
		public IHttpActionResult Delete(int id)
		{
			widgetManager = new WidgetManager();

			if (widgetManager.GetWidget(id) == null)
				return NotFound();

			widgetManager.RemoveWidget(id);
			return StatusCode(HttpStatusCode.NoContent);
		}
		
		/// <summary>
		/// Gets weeklyReview
		/// </summary>
		[SubPlatformCheckAPI]
		[HttpGet]
		[Route("api/WidgetApi/GetWeeklyReview/{id}")]
		public IHttpActionResult GetWeeklyReview(string id)
		{
			//Determine subplatform
			int suplatformID = -1;
			if (Request.Properties.TryGetValue("SubPlatformID", out object _customObject))
			{
				suplatformID = (int)_customObject;
			}

			widgetManager = new WidgetManager();
			IEnumerable<Widget> widgets;
			if (id.Equals("0")){
				widgets = widgetManager.GetWidgetsForWeeklyReview(suplatformID, null);
			} else {
				widgets = widgetManager.GetWidgetsForWeeklyReview(suplatformID, id);
			}
			
			return Ok(Mapper.Map(widgets, new List<UserWidgetDTO>()));
		}
		
		/// <summary>
		/// Returns all activity widgets. These widgets contain information about user activity.
		/// </summary>
		[SubPlatformCheckAPI]
		[HttpGet]
		[Route("api/GetActivityWidgets")]
		public IHttpActionResult GetActivityWidgets()
		{
			widgetManager = new WidgetManager();

			int suplatformID = -1;
			if (Request.Properties.TryGetValue("SubPlatformID", out object _customObject))
			{
				suplatformID = (int)_customObject;
			}
			
			List<Widget> widgets = widgetManager.GetWidgetsForActivities(suplatformID).ToList();
			
			if (widgets == null) return NotFound();
			
			return Ok(Mapper.Map(widgets.ToList(), new List<UserWidgetDTO>()));
		}

		[HttpGet]
		[Route("api/WidgetApi/CreateDataJson/{itemId}")]
		public IHttpActionResult CreateDataJson(int itemId)
		{
			widgetManager = new WidgetManager();
			string val = widgetManager.NoteboxData(itemId);
			List<int> vals = new List<int>();
			val.Split(',').ForEach(v => vals.Add(Int32.Parse(v)));
			
			Itemstats itemstats = new Itemstats
			{
				Male = vals.ElementAt(0),
				Female = vals.ElementAt(1),
				GenderUnknown = vals.ElementAt(2),
				Old = vals.ElementAt(3),
				Young = vals.ElementAt(4),
				AgeUnknown = vals.ElementAt(5)

			};
			

			return Ok(itemstats);
		}
 
	}
}