using System;
using System.Collections.Generic;
using BAR.BL.Domain.Data;
using BAR.DAL;
using Newtonsoft.Json;
using BAR.BL.Domain.Items;

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
    public IEnumerable<Information> GetAllInformationForId(int itemId)
    {
      InitRepo();
      return dataRepo.ReadAllInfoForId(itemId);
    }

    public bool SynchronizeData(string json)
    {     
      if (CheckPeople(json) && UpdateInformations(json))
      {
        return true;
      }
      return false;
    }

    private bool UpdateInformations(string json)
    {
      uowManager = new UnitOfWorkManager();
      InitRepo();
      IItemManager itemManager = new ItemManager(uowManager);
      dynamic deserializedJson = JsonConvert.DeserializeObject(json);
      List<Information> informationList = new List<Information>();
      for (int i = 0; i < deserializedJson.Count; i++)
      {
        PropertyValue propertyValue;
        Information information = new Information
        {
          PropertieValues = new List<PropertyValue>()
        };
        //Read gender
        propertyValue = new PropertyValue
        {
          Property = dataRepo.ReadProperty("Gender"),
          Value = deserializedJson[i].profile.gender,
          Confidence = 1
        };
        information.PropertieValues.Add(propertyValue);
        //Read age
        propertyValue = new PropertyValue
        {
          Property = dataRepo.ReadProperty("Age"),
          Value = deserializedJson[i].profile.age,
          Confidence = 1
        };
        information.PropertieValues.Add(propertyValue);
        //Read education
        propertyValue = new PropertyValue
        {
          Property = dataRepo.ReadProperty("Education"),
          Value = deserializedJson[i].profile.education,
          Confidence = 1
        };
        information.PropertieValues.Add(propertyValue);
        //Read language
        propertyValue = new PropertyValue
        {
          Property = dataRepo.ReadProperty("Language"),
          Value = deserializedJson[i].profile.language,
          Confidence = 1
        };
        information.PropertieValues.Add(propertyValue);
        //Read personality
        propertyValue = new PropertyValue
        {
          Property = dataRepo.ReadProperty("Personality"),
          Value = deserializedJson[i].profile.gender,
          Confidence = 1
        };
        information.PropertieValues.Add(propertyValue);
        //Read words
        for (int j = 0; j < deserializedJson[i].words.Count; j++)
        {
          propertyValue = new PropertyValue
          {
            Property = dataRepo.ReadProperty("Word"),
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
            Property = dataRepo.ReadProperty("Sentiment"),
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
            Property = dataRepo.ReadProperty("Hashtag"),
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
            Property = dataRepo.ReadProperty("Mention"),
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
            Property = dataRepo.ReadProperty("Url"),
            Value = deserializedJson[i].urls[j],
            Confidence = 1
          };
          information.PropertieValues.Add(propertyValue);
        }
        //Read date
        propertyValue = new PropertyValue
        {
          Property = dataRepo.ReadProperty("Date"),
          Value = deserializedJson[i].date,
          Confidence = 1
        };
        information.PropertieValues.Add(propertyValue);
        //Read postid
        propertyValue = new PropertyValue
        {
          Property = dataRepo.ReadProperty("PostId"),
          Value = deserializedJson[i].id,
          Confidence = 1
        };
        information.PropertieValues.Add(propertyValue);
        //Read retweet
        propertyValue = new PropertyValue
        {
          Property = dataRepo.ReadProperty("Retweet"),
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
          information.Items.Add(itemManager.GetPerson(name));
        }

        //Add other information
        information.Source = dataRepo.ReadSource("Twitter");
        string stringDate = Convert.ToString(deserializedJson[i].date);
        DateTime infoDate = DateTime.ParseExact(stringDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        information.CreationDate = infoDate;
        informationList.Add(information);
      }
      dataRepo.CreateInformations(informationList);
      uowManager.Save();
      return true;
    }

    private bool CheckPeople(string json)
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
      return true;
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
  }
}


