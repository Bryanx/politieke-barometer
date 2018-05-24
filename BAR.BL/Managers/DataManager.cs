using System;
using System.Linq;
using System.Collections.Generic;
using BAR.BL.Domain.Data;
using BAR.DAL;
using Newtonsoft.Json;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Widgets;
using BAR.BL.Domain.Users;

namespace BAR.BL.Managers
{
	/// <summary>
	/// Responsable for working with 
	/// data we received from TextGain.
	/// </summary>
	public class DataManager : IDataManager
	{
		private IDataRepository dataRepo;
		private UnitOfWorkManager uowManager;

		/// <summary>
		/// When unit of work is present, it will effect
		/// initRepo-method. (see documentation of initRepo)
		/// </summary>
		public DataManager(UnitOfWorkManager uowManager = null)
		{
			this.uowManager = uowManager;
		}

		/// <summary>
		/// Determines if the repo needs a unit of work
		/// if the unitOfWorkManager is present.
		/// </summary>
		private void InitRepo()
		{
			if (uowManager == null) dataRepo = new DataRepository();
			else dataRepo = new DataRepository(uowManager.UnitOfWork);
		}

		/// <summary>
		/// Returns a list of informations for
		/// a specific item id.
		/// </summary>
		public IEnumerable<Information> GetInformationsForItemid(int itemId)
		{
			InitRepo();
			return dataRepo.ReadInformationsForItemid(itemId);
		}

		/// <summary>
		/// Gives back a map with the number of metntions
		/// mapped to a specific day or week
		/// 
		/// NOTE
		/// This method is not the same as getNumberInfo
		/// This method will be used for widgets.
		/// </summary>
		public WidgetData GetNumberOfMentionsForItem(int itemId, int widgetId, string dateFormat, DateTime? startDate = null)
		{
			//Create widgetdata
			WidgetData widgetData = new WidgetData()
			{
				KeyValue = "Number of mentions",
				GraphValues = new List<GraphValue>()
			};

			//Get Widget
			Widget widget = new WidgetManager(uowManager).GetWidget(widgetId);
			if (widget == null) return widgetData;

			//Map informations to datetime and add them to the list	
			IEnumerable<Information> informations = GetInformationsWithAllInfoForItem(itemId);
			if (informations == null || informations.Count() == 0) return widgetData;

			//Determine startdata
			DateTime timestamp = widget.Timestamp.Value;
			if (startDate == null) startDate = DateTime.Now;
			else if (startDate < timestamp) return widgetData;

			//Extract information
			while (timestamp <= startDate)
			{
				//Each grapvalue represents a total number of mentions mapped
				//To a specific data
				int couny = informations.Count(i => i.CreationDate.Value.Day == startDate.Value.Day);
				widgetData.GraphValues.Add(new GraphValue()
				{
					Value = startDate.Value.ToString(dateFormat),
					NumberOfTimes = informations.Count(i => i.CreationDate.Value.Day == startDate.Value.Day)
				});
				startDate = startDate.Value.AddDays(-1);
			}

			return widgetData;
		}

		/// <summary>
		/// Gives back a widgetdata with all the propertievalues of a specific propertie
		/// works dynamicly.
		/// 
		/// WARNING
		/// This method will only work if the widget has a propertytag
		/// </summary>
		public WidgetData GetPropvaluesForWidget(int itemid, int widgetId, string proptag, DateTime? startDate = null)
		{
			//Create widgetdata
			WidgetData widgetData = new WidgetData()
			{
				KeyValue = proptag,
				GraphValues = new List<GraphValue>()
			};

			//Get propertytag and timestamp
			Widget widget = new WidgetManager(uowManager).GetWidget(widgetId);
			if (widget == null) return widgetData;
			DateTime? timestamp = widget.Timestamp.Value;
			if (timestamp == null) return widgetData;

			//Determine startdata
			if (startDate == null) startDate = DateTime.Now;
			else if (startDate < timestamp) return widgetData;

			//Get informations for item
			IEnumerable<Information> infosQueried = GetInformationsWithAllInfoForItem(itemid).Where(info => info.CreationDate <= startDate)
																							 .Where(info => info.CreationDate > timestamp)
																							 .AsEnumerable();
			if (infosQueried == null || infosQueried.Count() == 0) return widgetData;

			//Map timestap to number of propertyValues			
			foreach (Information information in infosQueried)
			{
				foreach (PropertyValue propval in information.PropertieValues)
				{
					//If the name of the property is the same as the propertytag,
					//Then the propertyvalue shall be added to the widgetdata
					if (propval.Property.Name.ToLower().Equals(proptag.ToLower()))
					{
						GraphValue grapValue = widgetData.GraphValues.Where(value => value.Value.ToLower().Equals(propval.Value.ToLower())).SingleOrDefault();
						//If A grapvalue yet exists for a specific widgetData object
						if (grapValue != null) grapValue.NumberOfTimes++;
						//If a grapvalue does not yet exists for a specific widgetData object
						else
						{
							grapValue = new GraphValue()
							{
								Value = propval.Value,
								NumberOfTimes = 1
							};
							widgetData.GraphValues.Add(grapValue);
						}
					}
				}
			}

			return widgetData;
		}

		/// <summary>
		/// Reads json and creates batches which will then call upon BatchUpdate for further handling.
		/// </summary>
		public bool SynchronizeData(string json)
		{
			dynamic deserializedJson = JsonConvert.DeserializeObject(json);
			int informationCount = deserializedJson.Count;

			//Synchronize data in batches to forecome an out of memory exception
			for (int i = 0; i < informationCount; i += 1000)
			{
				if (i + 1000 < informationCount)
				{
					BatchUpdate(json, i, i + 1000);
				}
				else
				{
					BatchUpdate(json, i, informationCount);
				}
			}

			return true;
		}

		/// <summary>
		/// Gets json which it will convert to Information objects dat are stored into the database afterwards.
		/// </summary>
		private void BatchUpdate(string json, int start, int end)
		{
			uowManager = new UnitOfWorkManager();
			InitRepo();

			//Load everything in memory to gain performance
			IItemManager itemManager = new ItemManager(uowManager);
			IEnumerable<Item> items = itemManager.GetAllPersons();
			List<Theme> themes = itemManager.GetAllThemes().ToList();
			IEnumerable<Property> properties = dataRepo.ReadAllProperties();
			IEnumerable<Source> sources = dataRepo.ReadAllSources();
			dynamic deserializedJson = JsonConvert.DeserializeObject(json);

			//Map propertyvalues to
			List<Information> informationList = new List<Information>();
			for (int i = start; i < end; i++)
			{
				PropertyValue propertyValue;
				Information information = new Information
				{
					PropertieValues = new List<PropertyValue>()
				};
				//Read gender
				propertyValue = new PropertyValue
				{
					Property = properties.Where(x => x.Name.Equals("Gender")).SingleOrDefault(),
					Value = deserializedJson[i].profile.gender,
					Confidence = 1
				};
				information.PropertieValues.Add(propertyValue);
				//Read age
				propertyValue = new PropertyValue
				{
					Property = properties.Where(x => x.Name.Equals("Age")).SingleOrDefault(),
					Value = deserializedJson[i].profile.age,
					Confidence = 1
				};
				information.PropertieValues.Add(propertyValue);
				//Read education
				propertyValue = new PropertyValue
				{
					Property = properties.Where(x => x.Name.Equals("Education")).SingleOrDefault(),
					Value = deserializedJson[i].profile.education,
					Confidence = 1
				};
				information.PropertieValues.Add(propertyValue);
				//Read language
				propertyValue = new PropertyValue
				{
					Property = properties.Where(x => x.Name.Equals("Language")).SingleOrDefault(),
					Value = deserializedJson[i].profile.language,
					Confidence = 1
				};
				information.PropertieValues.Add(propertyValue);
				//Read personality
				propertyValue = new PropertyValue
				{
					Property = properties.Where(x => x.Name.Equals("Personality")).SingleOrDefault(),
					Value = deserializedJson[i].profile.personality,
					Confidence = 1
				};
				information.PropertieValues.Add(propertyValue);
				//Read words
				for (int j = 0; j < deserializedJson[i].words.Count; j++)
				{
					propertyValue = new PropertyValue
					{
						Property = properties.Where(x => x.Name.Equals("Word")).SingleOrDefault(),
						Value = deserializedJson[i].words[j],
						Confidence = 1
					};
					information.PropertieValues.Add(propertyValue);
				}
				//Read sentiment
				for (int j = 0; j < deserializedJson[i].sentiment.Count; j++)
				{
					propertyValue = new PropertyValue
					{
						Property = properties.Where(x => x.Name.Equals("Sentiment")).SingleOrDefault(),
						Value = deserializedJson[i].sentiment[0],
						Confidence = deserializedJson[i].sentiment[1]
					};
					information.PropertieValues.Add(propertyValue);
				}
				//Read hashtags
				for (int j = 0; j < deserializedJson[i].hashtags.Count; j++)
				{
					propertyValue = new PropertyValue
					{
						Property = properties.Where(x => x.Name.Equals("Hashtag")).SingleOrDefault(),
						Value = deserializedJson[i].hashtags[j],
						Confidence = 1
					};
					information.PropertieValues.Add(propertyValue);
				}
				//Read mentions
				for (int j = 0; j < deserializedJson[i].mentions.Count; j++)
				{
					propertyValue = new PropertyValue
					{
						Property = properties.Where(x => x.Name.Equals("Mention")).SingleOrDefault(),
						Value = deserializedJson[i].mentions[j],
						Confidence = 1
					};
					information.PropertieValues.Add(propertyValue);
				}
				//Read urls
				for (int j = 0; j < deserializedJson[i].urls.Count; j++)
				{
					propertyValue = new PropertyValue
					{
						Property = properties.Where(x => x.Name.Equals("Url")).SingleOrDefault(),
						Value = deserializedJson[i].urls[j],
						Confidence = 1
					};
					information.PropertieValues.Add(propertyValue);
				}
				//Read date
				propertyValue = new PropertyValue
				{
					Property = properties.Where(x => x.Name.Equals("Date")).SingleOrDefault(),
					Value = deserializedJson[i].date,
					Confidence = 1
				};
				information.PropertieValues.Add(propertyValue);
				//Read postid
				propertyValue = new PropertyValue
				{
					Property = properties.Where(x => x.Name.Equals("PostId")).SingleOrDefault(),
					Value = deserializedJson[i].id,
					Confidence = 1
				};
				information.PropertieValues.Add(propertyValue);
				//Read retweet
				propertyValue = new PropertyValue
				{
					Property = properties.Where(x => x.Name.Equals("Retweet")).SingleOrDefault(),
					Value = deserializedJson[i].retweet,
					Confidence = 1
				};
				information.PropertieValues.Add(propertyValue);

				//Add connection to Item (Person)
				//Read persons
				information.Items = new List<Item>();
				for (int j = 0; j < deserializedJson[i].persons.Count; j++)
				{
					string name = deserializedJson[i].persons[j];
					information.Items.Add(items.Where(x => x.Name.Equals(name)).SingleOrDefault());
				}

				//Add connection to Item (Theme)
				//Read themes
				for (int j = 0; j < deserializedJson[i].themes.Count; j++)
				{
					string name = deserializedJson[i].themes[j];
					information.Items.Add(themes.Where(x => x.Name.Equals(name)).SingleOrDefault());
				}

				//Add other information
				information.Source = sources.Where(x => x.Name.Equals("Twitter")).SingleOrDefault();
				string stringDate = Convert.ToString(deserializedJson[i].date);
				DateTime infoDate = DateTime.ParseExact(stringDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
				information.CreationDate = infoDate;
				informationList.Add(information);
			}

			dataRepo.CreateInformations(informationList);
			uowManager.Save();
			uowManager = null;
		}

		/// <summary>
		/// Gets last succesfull audit.
		/// </summary>
		public SynchronizeAudit GetLastAudit()
		{
			InitRepo();
			return dataRepo.ReadLastAudit();
		}

		/// <summary>
		/// Adds an audit with boolean false.
		/// </summary>
		public SynchronizeAudit AddAudit(DateTime timestamp, bool succes)
		{
			InitRepo();
			SynchronizeAudit synchronizeAudit = new SynchronizeAudit()
			{
				TimeStamp = timestamp,
				Succes = succes
			};
			dataRepo.CreateAudit(synchronizeAudit);
			return synchronizeAudit;
		}

		/// <summary>
		/// Gets audit with given id.
		/// </summary>
		public SynchronizeAudit GetAudit(int synchronizeAuditId)
		{
			return dataRepo.ReadAudit(synchronizeAuditId);
		}

		/// <summary>
		/// Changes status of audit to true.
		/// </summary>
		public SynchronizeAudit ChangeAudit(int synchronizeAuditId)
		{
			InitRepo();

			//Get audit
			SynchronizeAudit synchronizeAudit = GetAudit(synchronizeAuditId);
			if (synchronizeAudit == null) return null;

			//Change audit
			synchronizeAudit.Succes = true;

			//Updata audit in the database
			dataRepo.UpdateAudit(synchronizeAudit);
			return synchronizeAudit;
		}

		/// <summary>
		/// Checks if json is empty.
		/// </summary>
		public bool IsJsonEmpty(string json)
		{
			dynamic deserializedJson = JsonConvert.DeserializeObject(json);
			int informationCount = deserializedJson.Count;

			if (informationCount == 0) return true;
			else return false;
		}

		/// <summary>
		/// Gets all sources.
		/// </summary>
		public IEnumerable<Source> GetAllSources()
		{
			InitRepo();
			return dataRepo.ReadAllSources();
		}

		/// <summary>
		/// Adds a source to the database.
		/// </summary>
		public Source AddSource(string name, string site)
		{
			InitRepo();
			Source source = new Source()
			{
				Name = name,
				Site = site
			};
			dataRepo.CreateSource(source);
			return source;
		}

		/// <summary>
		/// Removes a source from the database
		/// </summary>
		public void RemoveSource(string sourceId)
		{
			InitRepo();

			//Get source
			Source source = dataRepo.ReadSource(Convert.ToInt32(sourceId));
			if (source == null) return;

			//Delete source
			dataRepo.DeleteSource(source);
		}

		/// <summary>
		/// Returns a list of all the informations objects that are
		/// related to a specific item.
		/// </summary>
		public IEnumerable<Information> GetInformationsWithAllInfoForItem(int itemId)
		{
			InitRepo();
			return dataRepo.ReadInformationsWithAllInfoForItem(itemId);
		}

		/// <summary>
		/// Gives back all the widgetdata for monitoring the
		/// registerd users per day
		/// </summary>
		public WidgetData GetUserActivitiesData(ActivityType type, DateTime? timestamp = null)
		{
			InitRepo();

			//Create widgetdata
			WidgetData widgetData = new WidgetData()
			{
				GraphValues = new List<GraphValue>(),
				KeyValue = "activity"
			};

			//Check timestamp
			if (timestamp == null) timestamp = DateTime.Now.AddDays(-30);

			//Get actitivies
			IEnumerable<UserActivity> activities = new SubplatformManager().GetUserActivities(type, timestamp);
			if (activities == null || activities.Count() == 0) return widgetData;
			int count = activities.Count();

			//Query data
			DateTime startdate = DateTime.Now;
			UserActivity activity;
			while (timestamp <= startdate)
			{
				GraphValue graphValue = new GraphValue()
				{
					NumberOfTimes = 0,
					Value = startdate.ToString("dd-MM")
				};
				activity = activities.Where(act => act.TimeStamp.ToString("dd-MM-yy").Equals(startdate.ToString("dd-MM-yy"))).SingleOrDefault();
				if (activity != null) graphValue.NumberOfTimes = activity.NumberOfTimes;
				startdate = startdate.AddDays(-1);
				widgetData.GraphValues.Add(graphValue);
			}

			return widgetData;
		}

		/// <summary>
		/// Gives back all the urls from a specific item
		/// </summary>
		public IEnumerable<string> GetUrlsForItem(int itemId)
		{
			List<string> urls = new List<string>();

			//Get informations for item
			IEnumerable<Information> infos = GetInformationsWithAllInfoForItem(itemId).Take(50);
			if (infos == null || infos.Count() == 0) return urls;

			//Extract urls
			foreach (Information info in infos)
			{
				foreach (PropertyValue propval in info.PropertieValues)
				{
					if (propval.Property.Name.ToLower().Equals("url")) urls.Add(propval.Value);
				}
			}

			return urls.AsEnumerable();
		}

		/// <summary>
		/// Returns a source from the database
		/// based on the source name.
		/// </summary>
		public Source GetSource(string sourceName)
		{
			InitRepo();
			return dataRepo.ReadSource(sourceName);
		}

		/// <summary>
		/// Gives back all the datasources
		/// </summary>
		public IEnumerable<DataSource> GetAllDataSources()
		{
			InitRepo();
			return dataRepo.ReadAllDataSources();
		}

		/// <summary>
		/// Returns a source from the database
		/// based on the sourceId.
		/// </summary>
		public DataSource GetDataSource(int dataSourceId)
		{
			InitRepo();
			return dataRepo.ReadDataSource(dataSourceId);
		}

		/// <summary>
		/// Removes a datasource from the database
		/// </summary>
		public void RemoveDataSource(int dataSourceId)
		{
			InitRepo();

			//Get datasource
			DataSource dataSource = dataRepo.ReadDataSource(dataSourceId);
			if (dataSource == null) return;

			//Remove datasource
			dataRepo.DeleteDataSource(dataSource);
		}

		/// <summary>
		/// Gives back the geoloactionData of all the items
		/// </summary>
		public WidgetData GetGeoLocationData(DateTime? timestamp = null)
		{
			//Create Widgetdata
			WidgetData geoData = new WidgetData()
			{
				KeyValue = "geo",
				GraphValues = new List<GraphValue>()
			};

			//Get all persons
			IEnumerable<Person> persons = new ItemManager().GetAllItemsWithInformations();
			if (persons == null || persons.Count() == 0) return geoData;

			//Determine timestamp
			if (timestamp == null) timestamp = DateTime.Now.AddDays(-30);

			//Fill geoData
			foreach (Person item in persons)
			{
				//Check is district already exsists
				GraphValue graphValue;
				graphValue = geoData.GraphValues.Where(value => value.Value.ToLower().Equals(item.District.ToLower())).SingleOrDefault();
				int numberOfInfos = item.Informations.Where(info => info.CreationDate >= timestamp).Count();

				if (graphValue == null)
				{
					graphValue = new GraphValue()
					{
						Value = item.District,
						NumberOfTimes = numberOfInfos
					};
					geoData.GraphValues.Add(graphValue);
				}
				else
				{
					graphValue.NumberOfTimes += numberOfInfos;
				}
			}

			return geoData;
		}

		/// <summary>
		/// Gives back a widgetdata based on the persons that are part
		/// of the organisations.
		/// </summary>
		public WidgetData GetOrganisationData(int itemId, string dateFormat, DateTime? timestamp = null)
		{
			//Create widgetdata
			WidgetData widgetData = new WidgetData()
			{
				KeyValue = "Number of mentions",
				GraphValues = new List<GraphValue>()
			};

			//Get items
			IEnumerable<Item> items = new ItemManager().GetItemsForOrganisation(itemId);
			if (items == null || items.Count() == 0) return widgetData;

			//Determine timestamp
			if (timestamp == null) timestamp = DateTime.Now;

			//Query data
			while (timestamp > DateTime.Now.AddDays(-30))
			{
				GraphValue graphValue = new GraphValue()
				{
					Value = timestamp.Value.ToString(dateFormat)
				};
				foreach (Item item in items)
				{
					graphValue.NumberOfTimes += item.Informations.Where(info => info.CreationDate.Value.ToString("dd-MM-yy").Equals(timestamp.Value.ToString("dd-MM-yy"))).Count();
				}
				widgetData.GraphValues.Add(graphValue);
				timestamp = timestamp.Value.AddDays(-1);
			}

			return widgetData;
		}

		/// <summary>
		/// changes the interval from the datasource with the given ID.
		/// </summary>
		public DataSource ChangeTimerInterval(int dataSourceId, int interval)
		{
			InitRepo();

			//Get datasource
			DataSource source = GetDataSource(dataSourceId);
			if (source == null) return null;

			//Change datasource
			source.Interval = interval;

			//Update database
			dataRepo.UpdateDataSource(source);
			return source;
		}

		/// <summary>
		/// Changes the StratTimer from the datasource with the given ID.
		/// </summary>
		public DataSource ChangeStartTimer(int dataSourceId, string startTimer)
		{
			InitRepo();

			//Get datasource
			DataSource source = GetDataSource(dataSourceId);
			if (source == null) return null;

			//Change datasource
			source.SetTime = startTimer;

			//Updat
			dataRepo.UpdateDataSource(source);
			return source;
		}


		/// <summary>
		/// Changes the time when the datasource has last been chacked.
		/// </summary>
		public DataSource ChangeLastTimeCheckedTime(int dataSourceId, DateTime date)
		{
			InitRepo();

			//Get datasource
			DataSource source = GetDataSource(dataSourceId);
			if (source == null) return null;

			//Change datasource
			source.LastTimeChecked = date;

			//Update database
			dataRepo.UpdateDataSource(source);
			return source;
		}
	}
}


