using BAR.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using BAR.BL.Domain.Widgets;
using System.Data.Entity;

namespace BAR.DAL
{
	/// <summary>
	/// This class is used for the persistance of
	/// widgets and dashboards
	/// </summary>
	public class WidgetRepository : IWidgetRepository
	{
		private readonly BarometerDbContext ctx;

		/// <summary>
		/// If uow is present, the constructor
		/// will get the context from uow.
		/// </summary>
		public WidgetRepository(UnitOfWork uow = null)
		{
			if (uow == null) ctx = new BarometerDbContext();
			else ctx = uow.Context;
		}
		
		/// <summary>
		/// Gives back a list of all the dashboards.
		/// </summary>
		public IEnumerable<Dashboard> ReadAllDashboards()
		{
			return ctx.Dashboards.AsEnumerable();
		}

		/// <summary>
		/// Gives back a list of all the widgets.
		/// </summary>
		public IEnumerable<Widget> ReadAllWidgets()
		{
			return ctx.Widgets.AsEnumerable();
		}

		/// <summary>
		/// Gives back a dashboard object for a 
		/// specific dashboard id.
		/// </summary>
		public Dashboard ReadDashboard(int dashboardId)
		{
			return ctx.Dashboards.Find(dashboardId);
		}

		/// <summary>
		/// Gives back a dashboard object with all the widgets
		/// for a specific dashboard id.
		/// </summary>
		public Dashboard ReadDashboardWithWidgets(int dashboardId)
		{
			return ctx.Dashboards.Include(dash => dash.Widgets)
				.Where(dash => dash.DashboardId == dashboardId).SingleOrDefault();
		}
		
		/// <summary>
		/// Gives back a dashboard object with all the widgets
		/// for a specific user id.
		/// </summary>
		public Dashboard ReadDashboardWithWidgets(string userId) {
			return ctx.Dashboards.FirstOrDefault(dash => dash.User.Id == userId);
		}

		/// <summary>
		/// Gives back the general dashboard.
		/// 
		/// </summary>
		public Dashboard ReadGeneralDashboard()
		{
			return ctx.Dashboards.Include(dash => dash.Widgets)
				.Where(dash => dash.DashboardType == DashboardType.General).FirstOrDefault();
		}

		/// <summary>
		/// Gives back a widget object for a 
		/// specific widget id.
		/// </summary>
		public Widget ReadWidget(int widgetId)
		{
			return ctx.Widgets.Find(widgetId);
		}

		/// <summary>
		/// Gives back a list of widgets for a
		/// specific dashboard id.
		/// </summary>
		public IEnumerable<UserWidget> ReadWidgetsForDashboard(int dashboardId)
		{
			//Get UserWidgete
			List<UserWidget> widgets = new List<UserWidget>();
			foreach (Widget widget in ctx.Widgets.AsEnumerable())
			{
				if (widget is UserWidget) widgets.Add((UserWidget) widget);
			}

			//Return result
			return widgets.AsEnumerable().Where(wid => wid.Dashboard.DashboardId == dashboardId);
		}
		
		/// <summary>
		/// Creates a new dashboard and persist that
		/// to the database.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int CreateDashboard(Dashboard dashboard)
		{
			ctx.Dashboards.Add(dashboard);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Creates a new widget and persist that widget
		/// to the database.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// 
		/// WARNING
		/// widget needs to be linked to dashboard.
		/// Alternative: call ReadDashboard(), add widget to the list, updateDashboard();
		/// </summary>
		public int CreateWidget(Widget widget, int dashboardId)
		{
			if (widget is UserWidget)		
			{
				//Add reference if userwidget
				Dashboard dasboardToAddWidget = ReadDashboardWithWidgets(dashboardId);
				dasboardToAddWidget.Widgets.Add((UserWidget)widget);
				return UpdateDashboard(dasboardToAddWidget);
			} else
			{
				ctx.Widgets.Add(widget);
				return ctx.SaveChanges();
			}		
		}

		/// <summary>
		/// Updates a specific dashboard.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int UpdateDashboard(Dashboard dashboard)
		{
			ctx.Entry(dashboard).State = EntityState.Modified;
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Updates a list of dashboards.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int UpdateDashboards(IEnumerable<Dashboard> dashboards)
		{
			foreach (Dashboard dashboard in dashboards) ctx.Entry(dashboard).State = EntityState.Modified;
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Updates a specific widget.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int UpdateWidget(Widget widget)
		{
			ctx.Entry(widget).State = EntityState.Modified;
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Updates a list of widgets.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int UpdateWidgets(IEnumerable<Widget> widgets)
		{
			foreach (Widget widget in widgets) ctx.Entry(widget).State = EntityState.Modified;
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Deletes a specific dashboard.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// 
		/// WARNING
		/// All the widgets of the dashboard also need the be deleted.
		/// </summary>
		public int DeleteDashboard(int dashboardId)
		{
			Dashboard dashboardToDelete = ReadDashboardWithWidgets(dashboardId);
			if (dashboardToDelete != null) ctx.Dashboards.Remove(dashboardToDelete);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Deletes a list of dashboards.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// 
		/// WARNING
		/// All the widgets of the dashboards also need the be deleted.
		/// </summary>
		public int DeleteDashboards(IEnumerable<int> dashboardIds)
		{
			foreach (int id in dashboardIds)
			{
				Dashboard dashboardToDelete = ReadDashboardWithWidgets(id);
				ctx.Dashboards.Remove(dashboardToDelete);
			}
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Updates a specific widget.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int DeleteWidget(Widget widget)
		{
			ctx.Widgets.Remove(widget);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Deletes a list of widgets.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int DeleteWidgets(IEnumerable<Widget> widgets)
		{
			foreach (UserWidget widget in widgets) ctx.Widgets.Remove(widget);
			return ctx.SaveChanges();
		}		
	}
}
