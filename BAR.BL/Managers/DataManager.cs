using System;
using System.Linq;
using System.Collections.Generic;
using BAR.BL.Domain.Data;
using BAR.DAL;
using Newtonsoft.Json;
using BAR.BL.Domain.Items;
using BAR.BL.Domain.Widgets;

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
		/// mapped to a specific day
		/// 
		/// NOTE
		/// This method is not the same as getNumberInfo
		/// This method will be used for widgets.
		/// </summary>
		IDictionary<string, double> IDataManager.GetNumberOfMentionsForItem(int itemId)
		{
			ItemManager itemManager = new ItemManager();

			Item item = itemManager.GetItemWithAllWidgets(itemId);
			Widget widget = item.ItemWidgets.First();

			IDictionary<string, double> data = new Dictionary<string, double>();

			IEnumerable<Information> informations = GetInformationsForItemid(itemId);

			if (informations == null || informations.Count() == 0) return null;

			DateTime checkTime = DateTime.Now;
			while (checkTime > widget.Timestamp)
			{
				string key = checkTime.ToString();
				data[key] = informations.Count(i => i.CreationDate.Value.Day == checkTime.Day);
				checkTime = checkTime.AddDays(-1);
			}

			return data;
		}

		public IEnumerable<Item> SynchronizeData(string json)
		{
			IEnumerable<Item> items = CheckPeople(json);
			if (UpdateInformations(json)) return items;
			else return null;
		}

		private bool UpdateInformations(string json)
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

		private IEnumerable<Item> CheckPeople(string json)
		{
			dynamic deserializedJson = JsonConvert.DeserializeObject(json);

			IItemManager itemManager = new ItemManager();

			for (int i = 0; i < deserializedJson.Count; i++)
			{
				for (int j = 0; j < deserializedJson[i].persons.Count; j++)
				{
					string name = deserializedJson[i].persons[j];
					Item person = itemManager.GetPerson(name);

					if (person == null)
					{
						person = itemManager.CreateItem(ItemType.Person, name);
					}
				}
			}
			return itemManager.GetAllPersons();
		}

		public SynchronizeAudit GetLastAudit()
		{
			InitRepo();
			return dataRepo.ReadLastAudit();
		}

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

		public SynchronizeAudit GetAudit(int synchronizeAuditId)
		{
			return dataRepo.ReadAudit(synchronizeAuditId);
		}

		public SynchronizeAudit ChangeAudit(int synchronizeAuditId)
		{
			InitRepo();
			SynchronizeAudit synchronizeAudit = GetAudit(synchronizeAuditId);
			synchronizeAudit.Succes = true;
			dataRepo.UpdateAudit(synchronizeAudit);
			return synchronizeAudit;
		}
	}
}


