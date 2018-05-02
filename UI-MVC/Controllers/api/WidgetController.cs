
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
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
		public IHttpActionResult Get()
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
		[HttpGet]
		[Route("api/GetItemWidgets/{itemId}")]
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
		/// Temp get graph
		/// </summary>
		[HttpGet]
		[Route("api/GetGraphs/{itemId}")]
		public IHttpActionResult GetGraphs(int itemId)
		{
			widgetManager = new WidgetManager();
			IEnumerable<Widget> requestedWidgets = widgetManager.GetAllWidgetsWithAllDataForItem(itemId).AsEnumerable();
			int count = requestedWidgets.Count();
			
			if (requestedWidgets == null) return StatusCode(HttpStatusCode.NoContent);
			
			return Ok(requestedWidgets);
		}
		
		/// <summary>
		/// Transfers a Widget to the dashboard of a user.
		/// The given ItemWidget will be copied to a UserWidget.
		/// </summary>
		[HttpPost]
		[Route("api/MoveWidget/{widgetId}")]
		public IHttpActionResult MoveWidgetToDashboard(int widgetId)
		{
			widgetManager = new WidgetManager();

			Dashboard dash = widgetManager.GetDashboard(User.Identity.GetUserId());


			if (widgetManager.GetWidget(widgetId) == null) return StatusCode(HttpStatusCode.Conflict);
			
			Widget widgetToCopy = widgetManager.GetWidget(widgetId);
			Widget widget = widgetManager.AddWidget(WidgetType.GraphType, 
				widgetToCopy.Title, widgetToCopy.RowNumber, widgetToCopy.ColumnNumber, widgetToCopy.PropertyTags.ToList(), rowspan: widgetToCopy.RowSpan,
				colspan: widgetToCopy.ColumnSpan, dashboardId: dash.DashboardId);
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
		public IHttpActionResult Put([FromBody] UserWidgetDTO[] widgets)
		{
			widgetManager = new WidgetManager();
			
			foreach (UserWidgetDTO widget in widgets) {
				if (widget == null) return BadRequest("No widget given");
				if (widgetManager.GetWidget(widget.WidgetId) == null) return NotFound();
				widgetManager.ChangeWidgetPos(widget.WidgetId, widget.RowNumber, widget.ColumnNumber, widget.RowSpan,
					widget.ColumnSpan);
			}
			return StatusCode(HttpStatusCode.NoContent);
		}

		/// Temp test update to give a widget a new title
		[Route("api/Widget/{id}/title")]
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
		[HttpDelete]
		[Route("api/Widget/Delete/{id}")]
		public IHttpActionResult Delete(int id)
		{
			widgetManager = new WidgetManager();

			if (widgetManager.GetWidget(id) == null) return NotFound();
			widgetManager.RemoveWidget(id);
			return StatusCode(HttpStatusCode.NoContent);
		}
	}
}