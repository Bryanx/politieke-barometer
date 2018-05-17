using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using BAR.BL.Domain.Widgets;
using BAR.DAL;
using BAR.BL.Domain.Users;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Data;

namespace BAR.BL.Managers
{
	/// <summary>
	/// Responsable for managing widgets
	/// and their dashboards 
	/// </summary>
	public class WidgetManager : IWidgetManager
	{
		private IWidgetRepository widgetRepo;
		private UnitOfWorkManager uowManager;

		/// <summary>
		/// When unit of work is present, it will effect
		/// initRepo-method. (see documentation of initRepo)
		/// </summary>
		public WidgetManager(UnitOfWorkManager uowManager = null)
		{
			this.uowManager = uowManager;
		}

		/// <summary>
		/// Creates a widget based on the parameters
		/// and links that widget to a dasboard.
		/// </summary>
		public Widget AddWidget(WidgetType widgetType, string title, int rowNbr, int colNbr, List<PropertyTag> proptags, DateTime? timestamp = null,
			GraphType? graphType = null, int rowspan = 1, int colspan = 1, int dashboardId = -1, List<WidgetData> datas = null, List<Item> items = null)
		{
			InitRepo();
			Widget widget;

			//Checks if a userwidget or an itemWidget needs to be created
			if (dashboardId == -1) widget = new ItemWidget();
			else widget = new UserWidget();

			//Check if timestamp was given
			if (timestamp == null) timestamp = DateTime.Now.AddMonths(-1);

			widget.WidgetType = widgetType;
			widget.Title = title;
			widget.RowNumber = rowNbr;
			widget.ColumnNumber = colNbr;
			widget.RowSpan = rowspan;
			widget.ColumnSpan = colspan;
			widget.Timestamp = timestamp;
			widget.Items = items ?? new List<Item>();
			if (graphType != 0) widget.GraphType = graphType;
			widget.PropertyTags = proptags;

			//Check for adding widgetData
			if (datas == null) widget.WidgetDatas = new List<WidgetData>();
			else widget.WidgetDatas = datas;

			//Update database
			if (dashboardId != -1)
			{
				Dashboard dasboardToAddWidget = widgetRepo.ReadDashboardWithWidgets(dashboardId);
				dasboardToAddWidget.Widgets.Add((UserWidget)widget);
			}
			widgetRepo.CreateWidget(widget);

			return widget;
		}

		/// <summary>
		/// Adds an item to a widget.
		/// 
		/// WARNING
		/// THIS METHOD USES UNIT OF WORK
		/// </summary>
		public Widget AddItemToWidget(int widgetId, int itemId)
		{
			uowManager = new UnitOfWorkManager();
			InitRepo();

			//Get Item
			ItemManager itemManager = new ItemManager(uowManager);
			Item itemToAdd = itemManager.GetItemWithWidgets(itemId);

			//Add item to widget
			Widget widgetToUpdate = GetWidgetWithAllItems(widgetId);
			widgetToUpdate.Items.Add(itemToAdd);

			//Update database
			widgetRepo.UpdateWidget(widgetToUpdate);

			uowManager.Save();
			uowManager = null;

			return widgetToUpdate;
		}

		/// <summary>
		/// Deletes a widget from the dashboard.
		/// </summary>
		public void RemoveWidget(int widgetId)
		{
			InitRepo();
			Widget widgetToRemove = GetWidgetWithAllData(widgetId);
			if (widgetToRemove != null) widgetRepo.DeleteWidget(widgetToRemove);
		}

		/// <summary>
		/// Gives back a widget for a
		/// specific widgetId.
		/// </summary>
		public Widget GetWidget(int widgetId)
		{
			InitRepo();
			return widgetRepo.ReadWidget(widgetId);
		}

		/// <summary>
		/// Gives back a list of widgets
		/// for a specific dashboard.
		/// </summary>
		public IEnumerable<UserWidget> GetWidgetsForDashboard(int dashboardId)
		{
			InitRepo();
			return widgetRepo.ReadWidgetsForDashboard(dashboardId);
		}

		/// <summary>
		/// Gives back a list of widgets
		/// for a specific item.
		/// </summary>
		/// 

		public IEnumerable<Widget> GetWidgetsForItem(int itemId)
		{
			InitRepo();
			ItemManager itemManager = new ItemManager();
			return itemManager.GetItemWithWidgets(itemId).ItemWidgets.OfType<ItemWidget>();
		}

		/// <summary>
		/// Updates the details of the widget.
		/// </summary>
		public Widget ChangeWidgetDetails(int widgetId, int rowNbr, int colNbr, List<int> itemIds, int rowspan = 1, int colspan = 1, GraphType graphType = (GraphType) 0)
		{
			uowManager = new UnitOfWorkManager();
			InitRepo();
			
			ItemManager itemManager = new ItemManager(uowManager);
				
			//Get items
			List<Item> items = itemManager.GetAllItems().Where(i => itemIds.Contains(i.ItemId)).ToList();

			//get widget
			Widget widgetToUpdate = GetWidgetWithAllItems(widgetId);
			if (widgetToUpdate == null) return null;

			//update widget
			widgetToUpdate.RowNumber = rowNbr;
			widgetToUpdate.ColumnNumber = colNbr;
			if (items.Count > 0) widgetToUpdate.Items = items;
			widgetToUpdate.RowSpan = rowspan;
			widgetToUpdate.ColumnSpan = colspan;
			if (graphType != 0) widgetToUpdate.GraphType = graphType;

			//update database
			widgetRepo.UpdateWidget(widgetToUpdate);

			uowManager.Save();

			return widgetToUpdate;
		}

		/// <summary>
		/// Updates the position of the widget.
		/// </summary>
		public Widget ChangeWidgetTitle(int widgetId, string title)
		{
			InitRepo();

			//get widget
			Widget widgetToUpdate = GetWidget(widgetId);
			if (widgetToUpdate == null) return null;

			//update widget
			widgetToUpdate.Title = title;

			//update database
			widgetRepo.UpdateWidget(widgetToUpdate);

			return widgetToUpdate;
		}

		/// <summary>
		/// Updates a widget.
		/// </summary>
		public Widget ChangeWidget(Widget widget)
		{
			InitRepo();
			widgetRepo.UpdateWidget(widget);
			return widget;
		}

		/// <summary>
		/// Copies a widget to the dashboard
		/// All attributes of the given Widget are copied and used to generate a new UserWidget.
		/// </summary>
		public void MoveWidgetToDashBoard(int widgetId, GraphType graphType, IEnumerable<int> itemIds, string userId) 
		{
			uowManager = new UnitOfWorkManager();
			InitRepo();
			
			ItemManager itemManager = new ItemManager(uowManager);
			
			//Get items
			List<Item> items = itemManager.GetAllItems().Where(i => itemIds.Contains(i.ItemId)).ToList();
			//Get dashboard
			Dashboard dash = GetDashboard(userId);
			//Get widget
			Widget widget = GetWidgetWithAllData(widgetId);

			//make new widget and attach items to the new widgetwidget.GraphType
			Widget newWidget = AddWidget(WidgetType.GraphType, widget.Title, 0, 
				0, proptags: new List<PropertyTag>(), rowspan: widget.RowSpan,
				colspan: widget.ColumnSpan, dashboardId: dash.DashboardId, items: items, graphType: graphType);
			
			uowManager.Save();
			
			//Copy the property tags.
			//TODO: widget-PropertyTag should be a Many:Many relationship, that way a copy is not necessary.
			widget.PropertyTags.ToList().ForEach(p => newWidget.PropertyTags.Add(new PropertyTag() {Name = p.Name}));
			
			//Create a copy of all graphvalues and widgetDatas
			List<WidgetData> widgetDataCopy = new List<WidgetData>();
			widget.WidgetDatas.ToList().ForEach(w => 
			{
				//copy graphvalues
				List<GraphValue> graphValuesCopy = new List<GraphValue>();
				w.GraphValues.ToList().ForEach(g => graphValuesCopy.Add(new GraphValue(g)));
				//copy widgetdata
				WidgetData newWidgetData = new WidgetData(w);
				newWidgetData.GraphValues = graphValuesCopy;
				newWidgetData.Widget = newWidget;
				AddWidgetData(newWidgetData);
				
				widgetDataCopy.Add(newWidgetData);
			});

			newWidget.WidgetDatas = widgetDataCopy;
			
			ChangeWidget(newWidget);
			uowManager.Save();
		}

		/// <summary>
		/// Gives back a dashboard with their widgets.
		/// </summary>
		public Dashboard GetDashboard(int dashboardId)
		{
			InitRepo();
			return widgetRepo.ReadDashboardWithWidgets(dashboardId);
		}

		/// <summary>
		/// Gives back a dashboard with their widgets.
		/// </summary>
		public Dashboard GetDashboard(string userId)
		{
			InitRepo();
			Dashboard dash = widgetRepo.ReadDashboardWithWidgets(userId);
			if (dash == null) return AddDashboard(userId);
			return dash;
		}

		/// <summary>
		/// Creates a new dashboard based on the user.
		/// 
		/// NOTE
		/// THIS METHOD USES UNIT OF WORK
		/// </summary>
		public Dashboard AddDashboard(string userId = "-1", DashboardType dashType = DashboardType.Private)
		{
			uowManager = new UnitOfWorkManager();
			InitRepo();

			//Create dashboard
			Dashboard dashboard = new Dashboard()
			{
				DashboardType = dashType,
				Widgets = new List<UserWidget>(),
			};

			//Get user if not general dashboard
			if (!userId.Equals("-1"))
			{
				UserManager userManager = new UserManager(uowManager);
				User user = userManager.GetUser(userId);

				if (user == null) return null;
				else dashboard.User = user;
			}

			//Create database
			widgetRepo.CreateDashboard(dashboard);
			uowManager.Save();

			return dashboard;
		}

		/// <summary>
		/// Deletes a dashboard from the database.
		/// </summary>
		public void RemoveDashboard(int dashboardId)
		{
			InitRepo();
			widgetRepo.DeleteDashboard(dashboardId);
		}

		/// <summary>
		/// Gives back a widget with all the items
		/// </summary>
		public Widget GetWidgetWithAllItems(int widgetId)
		{
			InitRepo();
			return widgetRepo.ReadWidgetWithAllitems(widgetId);
		}

		/// <summary>
		/// Gives back a list with all the widgets and all the items.
		/// </summary>
		public IEnumerable<Widget> GetAllWidgetsWithAllItems()
		{
			InitRepo();
			return widgetRepo.ReadAllWidgetsWithAllItems().AsEnumerable();
		}

		/// <summary>
		/// Determines if the repo needs a unit of work
		/// if the unitOfWorkManager is present.
		/// </summary>
		private void InitRepo()
		{
			if (uowManager == null) widgetRepo = new WidgetRepository();
			else widgetRepo = new WidgetRepository(uowManager.UnitOfWork);

		}

		/// <summary>
		/// Gives back a widget with all the data.
		/// </summary>
		public Widget GetWidgetWithAllData(int widgetId)
		{
			InitRepo();
			return widgetRepo.ReadWidgetWithAllData(widgetId);
		}

		/// <summary>
		/// Gives back all the widgets with all the data.
		/// </summary>
		public IEnumerable<Widget> GetAllWidgetsWithAllData()
		{
			InitRepo();
			return widgetRepo.ReadAllWidgetsWithAllData().AsEnumerable();
		}

		/// <summary>
		/// Updates all the given widgets
		/// </summary>
		public IEnumerable<Widget> ChangeWidgets(IEnumerable<Widget> widgets)
		{
			InitRepo();
			widgetRepo.UpdateWidgets(widgets);
			return widgets;
		}

		/// <summary>
		/// Creates a new widgetdata in the database
		/// </summary>
		public WidgetData AddWidgetData(WidgetData widgetData)
		{
			InitRepo();
			widgetRepo.CreateWidgetData(widgetData);
			return widgetData;
		}

		/// <summary>
		/// Changes a specific widgetdata in the database
		/// </summary>
		public WidgetData ChangeWidgetData(WidgetData widgetData)
		{
			InitRepo();
			widgetRepo.UpdateWidgetData(widgetData);
			return widgetData;
		}

		/// <summary>
		/// Generate new data for all the widgets in the system
		/// This method takes time, but it happens in the background.
		/// </summary>
		public void GenerateDataForMwidgets()
		{
			InitRepo();

			//Remove old widgetdatas
			RemoveWidgetDatas(GetAllWidgetDatas());

			//Fill widgets with new widgetdata
			DataManager dataManager = new DataManager();
			List<Widget> widgets = GetAllWidgetsWithAllData().ToList();
			int widgetCount = widgets.Count();

			List<WidgetData> widgetDatas = new List<WidgetData>();

			foreach (Widget widget in widgets)
			{
				for (int i = 0; i < widget.Items.Count(); i++)
				{
					foreach (PropertyTag proptag in widget.PropertyTags)
					{
						WidgetData widgetData;
						if (proptag.Name.ToLower().Equals("mentions"))
						{
							widgetData = dataManager.GetNumberOfMentionsForItem
								(widget.Items.ElementAt(i).ItemId, widget.WidgetId, "dd-MM");
						}
						else
						{
							widgetData = dataManager.GetPropvaluesForWidget
								(widget.Items.ElementAt(i).ItemId, widget.WidgetId, proptag.Name);
						}
						widgetData.Widget = widget;
						widgetDatas.Add(widgetData);
					}
				}				
			}
			widgetRepo.CreateWidgetDatas(widgetDatas);

			//Remove overflowing items (temporary solution)
			new ItemManager().RemoveOverflowingItems();

			//Remove old geodata
			RemoveWidgetDatas(GetWidgetDatasForKeyvalue("geo"));

			//Create widget for geo-data
			List<PropertyTag> tags = new List<PropertyTag>();
			PropertyTag tag = new PropertyTag()
			{
				Name = "geo"
			};
			Widget geoloactionWidget = AddWidget(WidgetType.GraphType, "geoloaction of number of mentions", 1, 1, tags);

			//Get widgetdata for geolocaton
			WidgetData geoData = dataManager.GetGeoLocationData();
			geoData.Widget = geoloactionWidget;
			widgetRepo.CreateWidgetData(geoData);
		}

		/// <summary>
		/// Gives back all the widgets for a specific itemId
		/// The widgets contain all the information to construct a graph
		/// </summary>
		public IEnumerable<Widget> GetAllWidgetsWithAllDataForItem(int itemId)
		{
			InitRepo();
			return widgetRepo.ReadWidgetsWithAllDataForItem(itemId).AsEnumerable();
		}

		/// <summary>
		/// Gives back all the widgetdatas for a specifc itemId.
		/// </summary>
		public IEnumerable<WidgetData> GetWidgetDatasForItemId(int itemId)
		{
			InitRepo();
			return widgetRepo.ReadWidgetDatasForitemid(itemId).AsEnumerable();
		}

		/// <summary>
		/// Gives back all the widgetData of the system
		/// </summary>
		public IEnumerable<WidgetData> GetAllWidgetDatas()
		{
			InitRepo();
			return widgetRepo.ReadAllWidgetDatas().AsEnumerable();
		}

		/// <summary>
		/// Gives back a dashboard with all data
		/// for a specific userId
		/// </summary>
		public Dashboard GetDashboardWithAllDataForUserId(string userId)
		{
			InitRepo();
			return widgetRepo.ReadDashboardWithAllDataForUserId(userId);
		}

		/// <summary>
		/// Gives back all the widgets for the generic dashboard or
		/// for a specific user
		/// For now, this method will only return the widgets "number of metnions" because these are the most logical.
		/// </summary>
		public IEnumerable<Widget> GetWidgetsForWeeklyReview(string userId = null, int platformId = 1)
		{
			InitRepo();
			List<Widget> widgets = new List<Widget>();

			//Get trending items
			ItemManager itemManager = new ItemManager();
			IEnumerable<Item> items = null;
			itemManager.UpdateWeeklyReviewData(platformId);
			if (userId == null) items = itemManager.GetMostTrendingItems(useWithOldData: true);
			else items = itemManager.GetMostTrendingItemsForUser(userId, useWithOldData: true); 
			
			if (items == null || items.Count() == 0) return widgets;

			//Query widgets
			foreach (Item item in items) {
				IEnumerable<Widget> widgetsToAdd = widgetRepo.ReadWidgetsWithAllDataForItem(item.ItemId)
					.Where(widget => widget.PropertyTags
						.Any(proptag => proptag.Name.ToLower().Equals("mentions")))
					.AsEnumerable();
				if (widgetsToAdd != null) widgets.AddRange(widgetsToAdd);
			}

			return widgets.AsEnumerable();
		}
	


		/// <summary>
		/// Removes all the the given widgetdata from the database
		/// </summary>
		public void RemoveWidgetDatas(IEnumerable<WidgetData> datas)
		{
			InitRepo();
			widgetRepo.DeleteWidgetDatas(datas);
		}

		/// <summary>
		/// Gives back the widgetdatas from a specific keyvalue
		/// </summary>
		public IEnumerable<WidgetData> GetWidgetDatasForKeyvalue(string value)
		{
			InitRepo();
			return widgetRepo.ReadWidgetDatasForKeyvalue(value).AsEnumerable();
		}

		/// <summary>
		/// Gives back the geolocation widget for displaying on the homepage
		/// </summary>
		public Widget GetGeoLocationWidget()
		{
			InitRepo();
			return widgetRepo.ReadGeoLocationWidget();
		}
	}
}



