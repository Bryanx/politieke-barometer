using BAR.DAL.EF;
using System.Collections.Generic;
using System.Linq;
using BAR.BL.Domain.Widgets;
using System.Data.Entity;
using BAR.BL.Domain.Items;

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
		/// Gives back a dashboard object with all the widgets
		/// for a specific dashboard id.
		/// </summary>
		public Dashboard ReadDashboardWithWidgets(int dashboardId)
		{
			return ctx.Dashboards.Include(dash => dash.Widgets)
								 .Include(dash => dash.Widgets.Select(widget => widget.Items))
								 .Where(dash => dash.DashboardId == dashboardId)
								 .SingleOrDefault();
		}

		/// <summary>
		/// Gives back a dashboard object with all the widgets
		/// for a specific user id.
		/// </summary>
		public Dashboard ReadDashboardWithWidgets(string userId)
		{
			return ctx.Dashboards.Include(dash => dash.Widgets)
								 .Where(dash => dash.User.Id == userId)
								 .SingleOrDefault();
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
			return ReadDashboardWithWidgets(dashboardId).Widgets;
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
		public int CreateWidget(Widget widget)
		{
			ctx.Widgets.Add(widget);
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
		/// Updates a specific widget.
		/// Returns -1 if SaveChanges() is delayed by unit of work.
		/// </summary>
		public int DeleteWidget(Widget widget)
		{
			if (widget.WidgetDatas != null) ctx.WidgetDatas.RemoveRange(widget.WidgetDatas);
			ctx.Widgets.Remove(widget);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Gives back a widget with all the informations
		/// </summary>
		public Widget ReadWidgetWithAllitems(int widgetid)
		{
			return ctx.Widgets.Include(widget => widget.PropertyTags)
							  .Include(widget => widget.Items)
							  .Where(widget => widget.WidgetId == widgetid)
							  .SingleOrDefault();
		}

		/// <summary>
		/// Reads a widget with all the data of that specific widget.
		/// </summary>
		public Widget ReadWidgetWithAllData(int widgetId)
		{
			return ctx.Widgets.Include(widget => widget.Items)
							  .Include(widget => widget.PropertyTags)
							  .Include(widget => widget.WidgetDatas)
							  .Include(widget => widget.WidgetDatas.Select(widgetData => widgetData.GraphValues))
							  .Where(widget => widget.WidgetId == widgetId)
							  .SingleOrDefault();
		}

		/// <summary>
		/// Creates a new widgetData in the database
		/// </summary>
		public int CreateWidgetData(WidgetData widgetData)
		{
			ctx.WidgetDatas.Add(widgetData);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Gives back all widgets for a specific item id
		/// The widget contains all the data needed to construct a graph
		/// </summary>
		public IEnumerable<Widget> ReadWidgetsWithAllDataForItem(int itemId)
		{
			return ctx.Widgets.Include(widget => widget.Items)
							  .Include(widget => widget.WidgetDatas)
							  .Include(widget => widget.WidgetDatas.Select(data => data.GraphValues))
							  .Include(widget => widget.PropertyTags)
							  .Where(widget => widget.Items.Any(item => item.ItemId == itemId))
							  .AsEnumerable();
		}

		/// <summary>
		/// Gives back all the widgetdatas for a specific itemId
		/// </summary>
		public IEnumerable<WidgetData> ReadWidgetDatasForitemid(int itemId)
		{
			return ctx.WidgetDatas.Include(data => data.Widget)
								  .Include(data => data.GraphValues)
								  .Include(data => data.Widget.Items)
								  .Where(data => data.Widget.Items.Any(item => item.ItemId == itemId))
								  .AsEnumerable();
		}

		/// <summary>
		/// Gives back all the widgetData in the system
		/// </summary>
		public IEnumerable<WidgetData> ReadAllWidgetDatas()
		{
			return ctx.WidgetDatas.Include(data => data.GraphValues)
								  .AsEnumerable();
		}

		/// <summary>
		/// Creates a range of WidgetData.
		/// </summary>
		public int CreateWidgetDatas(ICollection<WidgetData> widgetDatas)
		{
			ctx.WidgetDatas.AddRange(widgetDatas);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Gives back all data from a dashboard for a
		/// specific user
		/// </summary>
		public Dashboard ReadDashboardWithAllDataForUserId(string userId)
		{
			return ctx.Dashboards.Include(dash => dash.User)
								 .Include(dash => dash.Widgets)
								 .Include(dash => dash.Widgets.Select(widget => widget.WidgetDatas))
								 .Include(dash => dash.Widgets.Select(widget => widget.WidgetDatas.Select(data => data.GraphValues)))
								 .Where(dash => dash.User.Id.ToLower().Equals(userId.ToLower()))
								 .SingleOrDefault();
		}

		/// <summary>
		/// Deletes the given widgetdatas from the database
		/// </summary>
		public int DeleteWidgetDatas(IEnumerable<WidgetData> datas)
		{
			foreach (WidgetData data in datas) ctx.Entry(data).State = EntityState.Deleted;		
			ctx.WidgetDatas.RemoveRange(datas);
			return ctx.SaveChanges();
		}

		/// <summary>
		/// Gives back all the widgetdatas for a specific keyvalue
		/// </summary>
		public IEnumerable<WidgetData> ReadWidgetDatasForKeyvalue(string value)
		{
			return ctx.WidgetDatas.Where(data => data.KeyValue.ToLower().Equals(value.ToLower()))
								  .AsEnumerable();
		}

		/// <summary>
		/// Gives back the geolocation widget for displaying on the homepage.
		/// </summary>
		public Widget ReadGeoLocationWidget()
		{
			return ctx.Widgets.Include(widget => widget.PropertyTags)
							  .Include(widget => widget.WidgetDatas)
							  .Include(widget => widget.WidgetDatas.Select(data => data.GraphValues))
							  .Where(widget => widget.PropertyTags.Any(tag => tag.Name.ToLower().Equals("geo")))
							  .SingleOrDefault();
		}

		/// <summary>
		/// Gives back all the widgets for a specific type
		/// </summary>
		public IEnumerable<Widget> ReadWidgetsForItemtype(ItemType type)
		{
			return ctx.Widgets.Include(widget => widget.Items)
							  .Include(widget => widget.WidgetDatas)
							  .Include(widget => widget.WidgetDatas.Select(data => data.GraphValues))
							  .Include(widget => widget.Items)
							  .Include(widget => widget.PropertyTags)
							  .Where(widget => widget.Items.Any(item => item.ItemType == type))
							  .AsEnumerable();
		}

		/// <summary>
		/// Gives back all the activity widgets
		/// </summary>
		public IEnumerable<Widget> ReadActivityWidgets()
		{
			return ctx.Widgets.Include(widget => widget.PropertyTags)
							  .Include(widget => widget.WidgetDatas)
							  .Include(widget => widget.WidgetDatas.Select(data => data.GraphValues))
							  .Where(widget => widget.PropertyTags.Any(tag => tag.Name.ToLower().Contains("activity")))
							  .AsEnumerable();
		}

		/// <summary>
		/// Gives back all the widgetdatas based on the widgetId
		/// </summary>
		public IEnumerable<WidgetData> ReadWidgetDatasForWidgetId(int widgetId)
		{
			return ctx.WidgetDatas.Include(data => data.Widget)
								  .Where(data => data.Widget.WidgetId == widgetId)
								  .AsEnumerable();
		}

		/// <summary>
		/// Resets all the data for a clean synchronisation
		/// </summary>
		public int ResetAllData()
		{
			ctx.Database.ExecuteSqlCommand("TRUNCATE TABLE GraphValues");
			return ctx.SaveChanges();
		}
	}
}
