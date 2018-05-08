
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
			List<UserWidget> widgets = widgetManager.GetWidgetsForDashboard(dash.DashboardId).ToList();
			
			if (widgets == null || widgets.Count() == 0) return StatusCode(HttpStatusCode.NoContent);

			return Ok(Mapper.Map(widgets, new List<UserWidgetDTO>()));
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
			
			//Get widgets
			IEnumerable<Widget> widgets = widgetManager.GetAllWidgetsWithAllDataForItem(itemId);
			
			IEnumerable<WidgetData> widgetDatas = widgets.FirstOrDefault(w => w.WidgetId == widgetId)?.WidgetDatas;
			
			//If widgetdata's is null, either something went wrong or
			//the user is trying to add another graph to the given widget
			if (widgetDatas == null) {
				string keyValue = widgetManager.GetWidgetWithAllData(widgetId)?.WidgetDatas.FirstOrDefault()?.KeyValue;
				if (keyValue == null) return StatusCode(HttpStatusCode.Conflict);
				widgetDatas = widgets.SingleOrDefault(w => w.WidgetDatas.Any(wd => wd.KeyValue == keyValue)).WidgetDatas;
			}
			IEnumerable<WidgetDataDTO> widgetDataDtos = Mapper.Map(widgetDatas, new List<WidgetDataDTO>());
			if (widgetDataDtos == null) return StatusCode(HttpStatusCode.NoContent);
			
			return Ok(widgetDataDtos);
		}
		
		/// <summary>
		/// Transfers a Widget to the dashboard of a user.
		/// The given ItemWidget will be copied to a UserWidget.
		/// </summary>
		[System.Web.Http.HttpPost]
		[System.Web.Http.Route("api/MoveWidget/{widgetId}")]
		public IHttpActionResult MoveWidgetToDashboard(int widgetId, [Bind(Exclude = "ItemIds")] UserWidgetDTO model)
		{
			widgetManager = new WidgetManager();
			widgetManager.MoveWidgetToDashBoard(widgetId, model.ItemIds, User.Identity.GetUserId());
			return StatusCode(HttpStatusCode.NoContent);
		}
		
		/// <summary>
		/// Creates a new Widget from the body of the post request.
		/// If the widget already exists, returns 409 Conflict
		/// </summary>
		/// <param name="widget"></param>
		/// <returns>The URL on which the created object can be requested</returns>
		public IHttpActionResult Post([FromBody] Widget widget)
		{
			widgetManager = new WidgetManager();

			Dashboard dash = widgetManager.GetDashboard(User.Identity.GetUserId());

			if (!ModelState.IsValid) return BadRequest(ModelState);
			if (widgetManager.GetWidget(widget.WidgetId) != null) return StatusCode(HttpStatusCode.Conflict);

			//Copy operation to make a userWidget from an itemWidget
			//This operation needs to be done because a userWidget has to be from a user
			//And a user has a dashboard.
			widgetManager.AddWidget(widget.WidgetType, widget.Title, widget.RowNumber,
				widget.ColumnNumber, widget.PropertyTags.ToList(), widget.Timestamp, widget.GraphType, widget.RowSpan, widget.ColumnSpan, dash.DashboardId);

			return CreatedAtRoute("DefaultApi"
				, new { controller = "Widget", id = widget.WidgetId }
				, widget);
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

		/// Temp test update to give a widget a new title
		[System.Web.Http.Route("api/Widget/{id}/title")]
		public IHttpActionResult PutName(int id, [FromBody] string newTitle)
		{
			widgetManager = new WidgetManager();

			if (widgetManager.GetWidget(id) == null) return NotFound();
			if (!ModelState.IsValid) return BadRequest(ModelState);
			
			widgetManager.ChangeWidgetTitle(id, newTitle);
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