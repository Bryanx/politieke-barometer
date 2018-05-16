
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using AutoMapper;
using BAR.BL.Domain.Data;
using BAR.BL.Domain.Widgets;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;
using Widget = BAR.BL.Domain.Widgets.Widget;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;
using BAR.UI.MVC.App_GlobalResources;


namespace BAR.UI.MVC.Controllers.api
{
	/// <summary>
	/// This class is used to transfer all widget information from UI to the managers.
	/// The api calls are in ajax requests on the dashboard index page.
	/// </summary>

	public class WidgetController : ApiController
	{
		private IWidgetManager widgetManager;
		private IItemManager itemManager;
		private IDataManager dataManager;

		/// <summary>
		///Reads all widgets for a user dashboard and returns them
		/// </summary>
		public IHttpActionResult GetUserWidgets()
		{
			widgetManager = new WidgetManager();

			Dashboard dash = widgetManager.GetDashboard(User.Identity.GetUserId());

			try {
				List<UserWidget> widgets = widgetManager.GetWidgetsForDashboard(dash.DashboardId).ToList();
				if (widgets == null || widgets.Count() == 0) return StatusCode(HttpStatusCode.NoContent);

				return Ok(Mapper.Map(widgets, new List<UserWidgetDTO>()));

			} catch (Exception e) {
				return StatusCode(HttpStatusCode.BadRequest);
			}
		}
		
		/// <summary>
		///Reads all widgets for an item and returns them.
		/// </summary>
		[System.Web.Http.HttpGet]
		[System.Web.Http.Route("api/GetItemWidgets/{itemId}")]
		public IHttpActionResult GetItemWidgets(int itemId)
		{
			widgetManager = new WidgetManager();

			IEnumerable<Widget> widgets = widgetManager.GetWidgetsForItem(itemId);
			
			if (widgets == null || widgets.Count() == 0) return StatusCode(HttpStatusCode.NoContent);

			List<UserWidgetDTO> uWidgets = Mapper.Map(widgets, new List<UserWidgetDTO>());
			foreach (UserWidgetDTO widgetDto in uWidgets) widgetDto.DashboardId = -1;

            return Ok(uWidgets);
		}
		
		/// <summary>
		/// Retrieves the graph data for a given itemId and widget.
		/// </summary>
		[System.Web.Http.HttpGet]
		[System.Web.Http.Route("api/GetGraphs/{itemId}/{widgetId}")]
		public IHttpActionResult GetGraphs(int itemId, int widgetId)
		{
			widgetManager = new WidgetManager();
			itemManager = new ItemManager();
			
			//Get widgets for item
			IEnumerable<Widget> widgets = widgetManager.GetAllWidgetsWithAllDataForItem(itemId);

			//Get keyvalue for the widgetid.
			string keyValue = widgetManager.GetWidgetWithAllData(widgetId)?.WidgetDatas.FirstOrDefault()?.KeyValue;

			if (keyValue == null) keyValue = widgetManager.GetWidgetWithAllData(widgetId)?.PropertyTags.FirstOrDefault()?.Name;
			IEnumerable<WidgetDataDTO> widgetDataDtos = null;
			try
			{
				IEnumerable<WidgetData> widgetDatas = widgets.FirstOrDefault(w => w.WidgetDatas.Any(wd => wd.KeyValue == keyValue)).WidgetDatas;
				widgetDataDtos = Mapper.Map(widgetDatas, new List<WidgetDataDTO>());
				widgetDataDtos.First().ItemName = itemManager.GetItem(itemId).Name;
			} catch (Exception e)
			{
				widgetDataDtos = Mapper.Map(new List<WidgetData>(), new List<WidgetDataDTO>());
			}

			//Get item name
			
			
			return Ok(widgetDataDtos);
		}

		/// <summary>
		/// Transfers a Widget to the dashboard of a user.
		/// The given ItemWidget will be copied to a UserWidget.
		/// </summary>
		[System.Web.Http.HttpPost]
		[System.Web.Http.Route("api/MoveWidget/")]
		[System.Web.Http.Authorize]
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
		[System.Web.Http.HttpPost]
		[System.Web.Http.Route("api/NewWidget/")]
		[System.Web.Http.Authorize]
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
		/// <param name="widgets"></param>
		/// <returns>If all goes well, 204 No Content is returned</returns>
		[System.Web.Http.HttpPost]
		[System.Web.Http.Route("api/UpdateWidget/")]
		public IHttpActionResult UpdateWidget([FromBody] UserWidgetDTO[] widgets)
		{
			widgetManager = new WidgetManager();
			
			foreach (UserWidgetDTO widget in widgets) {
				if (widget == null) return BadRequest("No widget given");
				if (widgetManager.GetWidget(widget.WidgetId) == null) return NotFound();
				widgetManager.ChangeWidgetDetails(widget.WidgetId, widget.RowNumber, widget.ColumnNumber, widget.ItemIds.ToList(),
						widget.RowSpan, widget.ColumnSpan, widget.GraphType);
			}
			return StatusCode(HttpStatusCode.NoContent);
		}

		/// <summary>
		/// Change the title of a widget
		/// </summary>
		[System.Web.Http.HttpPost]
		[System.Web.Http.Route("api/Widget/{id}/{title}")]
		public IHttpActionResult PutName(int id, string title)
		{
			widgetManager = new WidgetManager();

			if (widgetManager.GetWidget(id) == null) return NotFound();
			if (!ModelState.IsValid) return BadRequest(ModelState);
			
			widgetManager.ChangeWidgetTitle(id, title);
			return StatusCode(HttpStatusCode.NoContent);
		}
		
		/// <summary>
		/// Deletes a widget
		/// </summary>
		[System.Web.Http.HttpDelete]
		[System.Web.Http.Route("api/Widget/Delete/{id}")]
		public IHttpActionResult Delete(int id)
		{
			widgetManager = new WidgetManager();

			if (widgetManager.GetWidget(id) == null) return NotFound();
			widgetManager.RemoveWidget(id);
			return StatusCode(HttpStatusCode.NoContent);
		}
	}
}