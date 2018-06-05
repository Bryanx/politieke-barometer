using BAR.BL.Domain.Core;
using BAR.BL.Domain.Data;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Users;
using BAR.BL.Domain.Widgets;
using BAR.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

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
		/// Determines if the repo needs a unit of work
		/// if the unitOfWorkManager is present.
		/// </summary>
		private void InitRepo()
		{
			if (uowManager == null) widgetRepo = new WidgetRepository();
			else widgetRepo = new WidgetRepository(uowManager.UnitOfWork);

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
		/// Deletes a widget from the dashboard.
		/// </summary>
		public void RemoveWidget(int widgetId)
		{
			InitRepo();

			//Get widget
			Widget widget = GetWidgetWithAllData(widgetId);
			if (widget == null) return;

			//Remove widget
			widgetRepo.DeleteWidget(widget);
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
		/// for a specific item.
		/// </summary>
		public IEnumerable<Widget> GetItemwidgetsForItem(int itemId)
		{
			InitRepo();
			return new ItemManager().GetItemWithWidgets(itemId).ItemWidgets.OfType<ItemWidget>();
		}

		/// <summary>
		/// Updates the details of the widget.
		/// </summary>
		public Widget ChangeWidgetDetails(int widgetId, int rowNbr, int colNbr, List<int> itemIds, int rowspan = 1, int colspan = 1, GraphType graphType = 0)
		{
			uowManager = new UnitOfWorkManager();
			InitRepo();

			ItemManager itemManager = new ItemManager(uowManager);

			//Get items
			List<Item> items = itemManager.GetAllItems().Where(item => itemIds.Contains(item.ItemId)).ToList();

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
			uowManager = null;
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
			List<Item> items = itemManager.GetAllItems().Where(item => itemIds.Contains(item.ItemId)).ToList();

			//Get dashboard
			Dashboard dash = GetDashboard(userId);
			if (dash == null) return;

			//Get widget
			Widget widget = GetWidgetWithAllData(widgetId);
			if (widget == null) return;

			//make new widget and attach items to the new widgetwidget.GraphType
			Widget newWidget = AddWidget(WidgetType.GraphType, widget.Title, 0,
				0, proptags: new List<PropertyTag>(), rowspan: widget.RowSpan,
				colspan: widget.ColumnSpan, dashboardId: dash.DashboardId, items: items, graphType: graphType);

			uowManager.Save();

			//Copy the property tags.
			widget.PropertyTags.ToList().ForEach(proptag => newWidget.PropertyTags.Add(new PropertyTag() { Name = proptag.Name }));

			//Create a copy of all graphvalues and widgetDatas
			List<WidgetData> widgetDataCopy = new List<WidgetData>();
			widget.WidgetDatas.ToList().ForEach(data =>
			{
				//copy graphvalues
				List<GraphValue> graphValuesCopy = new List<GraphValue>();
				data.GraphValues.ToList().ForEach(value => graphValuesCopy.Add(new GraphValue(value)));

				//copy widgetdata
				WidgetData newWidgetData = new WidgetData(data);
				newWidgetData.GraphValues = graphValuesCopy;
				newWidgetData.Widget = newWidget;
				AddWidgetData(newWidgetData);

				widgetDataCopy.Add(newWidgetData);
			});

			newWidget.WidgetDatas = widgetDataCopy;

			//Update database
			ChangeWidget(newWidget);
			uowManager.Save();
			uowManager = null;
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

			//Get user
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
			uowManager = null;
			return dashboard;
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
		/// Gives back a widget with all the data.
		/// </summary>
		public Widget GetWidgetWithAllData(int widgetId)
		{
			InitRepo();
			return widgetRepo.ReadWidgetWithAllData(widgetId);
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
		/// Generate new data for all the widgets in the system
		/// This method takes time, but it happens in the background.
		/// </summary>
		public void GenerateDataForPersonsAndThemes()
		{
			InitRepo();

			//Remove old widgetdatas
			widgetRepo.ResetAllData();
			RemoveWidgetDatas(GetAllWidgetDatas());

			//Fill widgets with new widgetdata
			DataManager dataManager = new DataManager();
			List<Widget> widgets = widgetRepo.ReadWidgetsForItemtype(ItemType.Person).ToList();
			widgets.AddRange(widgetRepo.ReadWidgetsForItemtype(ItemType.Theme).ToList());
			int widgetCount = widgets.Count();

			//Extract data from informations
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

			//Generate data for geolocation
			GenerateDataForGeolocation();
		}

		/// <summary>
		/// Generates data for geoloaction
		/// </summary>
		private void GenerateDataForGeolocation()
		{
			InitRepo();

			//Remove old geodata
			RemoveWidgetDatas(GetWidgetDatasForKeyvalue("geo"));
			Widget oldGeodata = widgetRepo.ReadGeoLocationWidget();
			if (oldGeodata != null) RemoveWidget(oldGeodata.WidgetId);

			//Create widget for geo-data
			List<PropertyTag> tags = new List<PropertyTag>
			{
				new PropertyTag
				{
					Name = "geo"
				}
			};
			Widget geoloactionWidget = AddWidget(WidgetType.GraphType, "geoloaction of number of mentions", 1, 1, tags);

			//Get widgetdata for geolocaton
			WidgetData geoData = new DataManager().GetGeoLocationData();
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
		public IEnumerable<Widget> GetWidgetsForWeeklyReview(int platformId, string userId = null)
		{
			InitRepo();
			List<Widget> widgets = new List<Widget>();

			//Get trending items
			ItemManager itemManager = new ItemManager();
			IEnumerable<Item> items = null;
			itemManager.RefreshItemData(platformId);
			if (userId == null) items = itemManager.GetMostTrendingItems(platformId, useWithOldData: true);
			else items = itemManager.GetMostTrendingItemsForUser(platformId, userId, useWithOldData: true);

			if (items == null || items.Count() == 0) return widgets;

			//Query widgets
			foreach (Item item in items)
			{
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
		/// Geolocation should already be determined for this method to work
		/// </summary>
		public Widget GetGeoLocationWidget()
		{
			InitRepo();
			return widgetRepo.ReadGeoLocationWidget();
		}

		/// <summary>
		/// Gives back all the widgets for monitoring activities
		/// </summary>
		public IEnumerable<Widget> GetWidgetsForActivities(int platformId)
		{
			InitRepo();

			//Get widgets
			IEnumerable<Widget> widgets = widgetRepo.ReadActivityWidgets().AsEnumerable();
			if (widgets == null || widgets.Count() == 0) return widgets;

			//Check lastUpdated
			DateTime? lastUpdated = new SubplatformManager().GetSubPlatform(platformId).LastUpdatedActivities;

			//If lastUpdated was to long ago, then the activities shall be udpated
			if (lastUpdated == null || !lastUpdated.Value.ToString("dd-MM-yy").Equals(DateTime.Now.ToString("dd-MM-yy"))) widgets = ChangeWidgetActities(widgets, platformId);

			//Link widdatas to widgets
			foreach (Widget widget in widgets) widget.WidgetDatas.ToList().AddRange(GetWidgetDatasForWidgetId(widget.WidgetId));
			return widgets;
		}

		/// <summary>
		/// Forces an update for the activity widgets
		/// </summary>
		public IEnumerable<Widget> ChangeWidgetActities(IEnumerable<Widget> widgets, int platformId)
		{
			//** Create new widgetDatas **//
			DataManager dataManager = new DataManager();

			//1st widget
			WidgetData loginData = dataManager.GetUserActivitiesData(ActivityType.LoginActivity, DateTime.Now.AddDays(-30));
			Widget loginWidget = widgets.Where(widget => widget.PropertyTags.All(tag => tag.Name.ToLower().Contains("login"))).SingleOrDefault();
			loginWidget.WidgetDatas = new List<WidgetData>
			{
				loginData
			};
			loginData.Widget = loginWidget;
			widgetRepo.CreateWidgetData(loginData);

			//2nd widget
			WidgetData registerData = dataManager.GetUserActivitiesData(ActivityType.RegisterActivity, DateTime.Now.AddDays(-30));
			Widget registerWidget = widgets.Where(widget => widget.PropertyTags.All(tag => tag.Name.ToLower().Contains("register"))).SingleOrDefault();
			loginWidget.WidgetDatas = new List<WidgetData>
			{
				loginData
			};
			registerData.Widget = registerWidget;
			widgetRepo.CreateWidgetData(registerData);

			//3rd widget
			WidgetData visitData = dataManager.GetUserActivitiesData(ActivityType.VisitActitiy, DateTime.Now.AddDays(-30));
			Widget visitWidget = widgets.Where(widget => widget.PropertyTags.All(tag => tag.Name.ToLower().Contains("visit"))).SingleOrDefault();
			loginWidget.WidgetDatas = new List<WidgetData>
			{
				loginData
			};
			visitData.Widget = visitWidget;
			widgetRepo.CreateWidgetData(visitData);

			//Get last updated
			SubplatformManager platformManager = new SubplatformManager();
			SubPlatform platform = platformManager.GetSubPlatform(platformId);
			platform.LastUpdatedActivities = DateTime.Now;
			platformManager.ChangeSubplatform(platform);

			return widgets;
		}

		/// Generates data for organisations based on the data
		/// of the persons
		/// </summary>
		public void GenerateDataForOrganisations()
		{
			InitRepo();

			//Get items
			IEnumerable<Item> items = new ItemManager().GetAllOrganisations();
			if (items == null || items.Count() == 0) return;

			//Get widgets
			IEnumerable<Widget> widgets = widgetRepo.ReadWidgetsForItemtype(ItemType.Organisation);
			if (widgets == null || widgets.Count() == 0) return;

			//Extract data form persons to fill organisations
			DataManager dataManager = new DataManager();
			for (int i = 0; i < items.Count(); i++)
			{
				//Get widgetdata for organisation
				WidgetData organisationData = dataManager.GetOrganisationData(items.ElementAt(i).ItemId, "dd-MM");
				organisationData.Widget = widgets.ElementAt(i);
				widgetRepo.CreateWidgetData(organisationData);
			}
		}

		/// <summary>
		/// Gives back all the widgetdatas based on the widgetId
		/// </summary>
		public IEnumerable<WidgetData> GetWidgetDatasForWidgetId(int widgetId)
		{
			InitRepo();
			return widgetRepo.ReadWidgetDatasForWidgetId(widgetId).AsEnumerable();
		}

		public string NoteboxData(int itemId)
		{
			IEnumerable<Widget> data = GetAllWidgetsWithAllDataForItem(itemId);

			double male = 0;
			double female = 0;
			double unknown = 0;

			int age25plus = 0;
			int age25min = 0;
			int ageUnknown = 0;

			foreach (Widget w in data)
			{
				foreach (WidgetData wd in w.WidgetDatas)
				{
					if (wd.KeyValue.ToLower().Equals("gender"))
					{
						foreach (GraphValue gv in wd.GraphValues)
						{
							if (gv.Value.Equals("m"))
							{
								male += (int)gv.NumberOfTimes;
							}
							else if (gv.Value.Equals("f"))
							{
								female += (int)gv.NumberOfTimes;
							}
							else
							{
								unknown += (int)gv.NumberOfTimes;
							}
						}
					}
					else if(wd.KeyValue.ToLower().Equals("age"))
					{
						foreach (GraphValue gv in wd.GraphValues)
						{
							if (gv.Value.Equals("25+"))
							{
								age25plus += (int)gv.NumberOfTimes;
							}
							else if (gv.Value.Equals("25-"))
							{
								age25min += (int)gv.NumberOfTimes;
							}
							else
							{
								ageUnknown += (int)gv.NumberOfTimes;
							}
						}
					}
				}
			}

			double totaal = male + female + unknown;
			male = Math.Round(male / totaal * 50);
			female = Math.Round(female / totaal * 50);
			unknown = Math.Round(unknown / totaal * 50);
			

			return male + "," + female + "," + unknown + "," + age25min + "," + age25plus + "," + ageUnknown;
		}
	}
}



