using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using AutoMapper;
using BAR.BL.Domain.Widgets;
using BAR.BL.Managers;
using BAR.UI.MVC.Models;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;
using Widget = BAR.BL.Domain.Widgets.Widget;

namespace BAR.UI.MVC.Controllers.api
{
	/// <summary>
	/// This class is used to transfer all widget information from UI to the managers.
	/// The api calls are in ajax requests on the dashboard index page.
	/// </summary>

	public class WidgetController : ApiController
	{
		private IWidgetManager widgetManager;
		
		/// <summary>
		///Reads all widgets for a user dashboard and returns them (possibly in json)
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
		///Reads all widget for an item
		/// </summary>
		[HttpGet]
		[Route("api/GetWidgets/{itemId}")]
		public IHttpActionResult GetWidgets(int itemId)
		{
			widgetManager = new WidgetManager();

			var widgets = widgetManager.GetWidgetsForItem(itemId);
			
			if (widgets == null || widgets.Count() == 0) return StatusCode(HttpStatusCode.NoContent);
			
			return Ok(widgets);
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
			Widget widget = widgetManager.CreateWidget(WidgetType.GraphType, 
				widgetToCopy.Title, widgetToCopy.RowNumber, widgetToCopy.ColumnNumber, widgetToCopy.RowSpan,
				widgetToCopy.ColumnSpan, dash.DashboardId);

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
			widgetManager.CreateUserWidget(widget, dash.DashboardId);
			return CreatedAtRoute("DefaultApi"
				, new { controller = "Widget", id = widget.WidgetId }
				, widget);
		}

		/// <summary>
		/// Updates an existing widget.
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

		/// Temp delete a widget.
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