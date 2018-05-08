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
		/// Gets the number of informations of a specific given item.
		/// </summary
		public int GetNumberInfo(int itemId, DateTime since)
		{
			InitRepo();
			return dataRepo.ReadNumberInfo(itemId, since);
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
		/// Gives back a list of informations for a specific widget
		/// </summary>
		public IEnumerable<Information> GetInformationsForWidgetid(int widgetId)
		{
			//Get widget
			WidgetManager widgetManager = new WidgetManager();
			Widget widget = widgetManager.GetWidgetWithAllItems(widgetId);
			if (widget == null) return null;

			//Get informations
			List<Information> infos = new List<Information>();
			ItemManager itemManager = new ItemManager();
			foreach (Item item in widget.Items)
			{
				infos.AddRange(GetInformationsForItemid(item.ItemId));
			}

			return infos.AsEnumerable();
		}

		/// <summary>
		/// Gives back all the infomations based on a given timestamp
		/// </summary>
		public IEnumerable<Information> GetInformationsWithTimestamp(int widgetId)
		{
			//Get timestap of widget
			WidgetManager widgetManager = new WidgetManager();
			DateTime? timestamp = widgetManager.GetWidget(widgetId).Timestamp;

			//Return filterd informations
			if (timestamp == null) return GetInformationsForItemid(widgetId).AsEnumerable();
			else return GetInformationsForItemid(widgetId).Where(info => info.CreationDate >= timestamp).AsEnumerable();
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

			DateTime timestamp = widget.Timestamp.Value;
			if (startDate == null) startDate = DateTime.Now;
			else if (startDate < timestamp) return widgetData;


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
		/// Gives back a map with all the propertievalues of a specific propertie
		/// works dynamicly. The returnvalue contains the following:
		/// 
		/// Dictionary:
		/// - key: name of the property-value
		/// - value: number of times the property-value was mentioned
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
			IItemManager itemManager = new ItemManager(uowManager);
			IEnumerable<Item> items = itemManager.GetAllPersons();
			IEnumerable<Property> properties = dataRepo.ReadAllProperties();
			IEnumerable<Source> sources = dataRepo.ReadAllSources();
			dynamic deserializedJson = JsonConvert.DeserializeObject(json);
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
					Value = deserializedJson[i].profile.gender,
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
			SynchronizeAudit synchronizeAudit = GetAudit(synchronizeAuditId);
			synchronizeAudit.Succes = true;
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

			if (informationCount == 0)
			{
				return true;
			}
			return false;
		}

		/// <summary>
		/// Gets all sources.
		/// </summary>
		public IEnumerable<Source> GetAllSources()
		{
			InitRepo();
			return dataRepo.ReadAllSources();
		}

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

		public void RemoveSource(string sourceId)
		{
			InitRepo();
			Source source = dataRepo.ReadSource(Convert.ToInt32(sourceId));
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
		public WidgetData GetUserActivitiesData(DateTime? timestamp = null)
		{
			InitRepo();

			//Create widgetdata
			WidgetData widgetData = new WidgetData()
			{
				GraphValues = new List<GraphValue>(),
				KeyValue = "User Actitivities"
			};

			//Get actitivies
			IEnumerable<UserActivity> activities = new SubplatformManager().GetUserActivities(timestamp);
			if (activities == null || activities.Count() == 0) return widgetData;

			//Query data
			DateTime startdate = DateTime.Now;
			while (timestamp >= startdate)
			{
				GraphValue graphValue = new GraphValue()
				{
					NumberOfTimes = activities.Where(act => act.TimeStamp.Day == startdate.Day).Count(),
					Value = startdate.ToString("dd-MM")
				};
				startdate = startdate.AddDays(-1);
			}

			//Reverse data
			widgetData.GraphValues.Reverse();
			return widgetData;
		}
	}
}


