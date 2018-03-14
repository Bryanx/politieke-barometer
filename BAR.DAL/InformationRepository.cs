using BAR.BL.Domain.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BAR.DAL
{

  /// <summary>
  /// At this moment the repository works HC.
  /// </summary>
  public class InformationRepository : IInformationRepository
  {
    private List<Information> informationList;
    private List<Property> propertyList;

    public InformationRepository()
    {
      informationList = new List<Information>();
      propertyList = new List<Property>();
      GenerateProperties();
      ReadJson();
    }

    /// <summary>
    /// Gets the number of informations of a specific given item
    /// form since till now.
    /// </summary
    public int ReadNumberInfo(int itemId, DateTime since)
    {
      return informationList.Where(info => info.Item.ItemId == itemId)
          .Where(info => info.LastUpdated <= since).Count();
    }

    /// <summary>
    /// Make a set of properties that will be linked to a value from the json file
    /// </summary>
    public void GenerateProperties()
    {
      Property hashtag = new Property
      {
        PropertyId = 1,
        Name = "Hashtag"
      };
      Property word = new Property
      {
        PropertyId = 2,
        Name = "Word"
      };
      Property date = new Property
      {
        PropertyId = 3,
        Name = "Date"
      };
      Property politician = new Property
      {
        PropertyId = 4,
        Name = "Politician"
      };
      Property geo = new Property
      {
        PropertyId = 5,
        Name = "Geo"
      };
      Property postId = new Property
      {
        PropertyId = 6,
        Name = "TweetId"
      };
      Property userId = new Property
      {
        PropertyId = 7,
        Name = "UserId"
      };
      Property sentiment = new Property
      {
        PropertyId = 8,
        Name = "Sentiment"
      };
      Property retweet = new Property
      {
        PropertyId = 9,
        Name = "Retweet"
      };
      Property url = new Property
      {
        PropertyId = 10,
        Name = "Url"
      };
      Property mention = new Property
      {
        PropertyId = 11,
        Name = "Mention"
      };

      propertyList.Add(hashtag);
      propertyList.Add(word);
      propertyList.Add(date);
      propertyList.Add(politician);
      propertyList.Add(geo);
      propertyList.Add(postId);
      propertyList.Add(userId);
      propertyList.Add(sentiment);
      propertyList.Add(retweet);
      propertyList.Add(url);
      propertyList.Add(mention);
    }

    /// <summary>
    /// This method will read a json file located in UI-CA/bin/debug, after reading this file
    /// into the json variable at will start putting the values into a list of properties which will be
    /// added in a key-value pair that is added to an Information object
    /// </summary>
    public void ReadJson()
    {
      string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.json");
      string json = File.ReadAllText(path);
      dynamic deserializedJson = JsonConvert.DeserializeObject(json);

      for (int i = 0; i < deserializedJson.records.Count; i++)
      {
        PropertyValue propertyValue;
        Information information = new Information();
        information.InformationId = i + 1;
        information.PropertieValues = new Dictionary<Property, ICollection<PropertyValue>>();

        List<PropertyValue> list = new List<PropertyValue>();
        for (int j = 0; j < deserializedJson.records[i].hashtags.Count; j++)
        {
          propertyValue = new PropertyValue();
          propertyValue.PropertyValueId = j + 1;
          propertyValue.Value = deserializedJson.records[i].hashtags[j];
          list.Add(propertyValue);
        }
        information.PropertieValues.Add(propertyList.ElementAt(0), list);

        list = new List<PropertyValue>();
        for (int j = 0; j < deserializedJson.records[i].words.Count; j++)
        {
          propertyValue = new PropertyValue();
          propertyValue.PropertyValueId = j + 1;
          propertyValue.Value = deserializedJson.records[i].words[j];
          list.Add(propertyValue);
        }
        information.PropertieValues.Add(propertyList.ElementAt(1), list);

        list = new List<PropertyValue>();
        propertyValue = new PropertyValue();
        propertyValue.PropertyValueId = 1;
        propertyValue.Value = deserializedJson.records[i].date;
        list.Add(propertyValue);
        information.PropertieValues.Add(propertyList.ElementAt(2), list);

        list = new List<PropertyValue>();
        for (int j = 0; j < deserializedJson.records[i].politician.Count; j++)
        {
          propertyValue = new PropertyValue();
          propertyValue.PropertyValueId = j + 1;
          propertyValue.Value = deserializedJson.records[i].politician[j];
          list.Add(propertyValue);
        }
        information.PropertieValues.Add(propertyList.ElementAt(3), list);

        list = new List<PropertyValue>();
        propertyValue = new PropertyValue();
        propertyValue.PropertyValueId = 1;
        propertyValue.Value = deserializedJson.records[i].geo;
        list.Add(propertyValue);
        information.PropertieValues.Add(propertyList.ElementAt(4), list);

        list = new List<PropertyValue>();
        propertyValue = new PropertyValue();
        propertyValue.PropertyValueId = 1;
        propertyValue.Value = deserializedJson.records[i].id;
        list.Add(propertyValue);
        information.PropertieValues.Add(propertyList.ElementAt(5), list);

        list = new List<PropertyValue>();
        propertyValue = new PropertyValue();
        propertyValue.PropertyValueId = 1;
        propertyValue.Value = deserializedJson.records[i].user_id;
        list.Add(propertyValue);
        information.PropertieValues.Add(propertyList.ElementAt(6), list);

        list = new List<PropertyValue>();
        for (int j = 0; j < deserializedJson.records[i].sentiment.Count; j++)
        {
          propertyValue = new PropertyValue();
          propertyValue.PropertyValueId = j + 1;
          propertyValue.Value = deserializedJson.records[i].sentiment[j];
          list.Add(propertyValue);
        }
        information.PropertieValues.Add(propertyList.ElementAt(7), list);

        list = new List<PropertyValue>();
        propertyValue = new PropertyValue();
        propertyValue.PropertyValueId = 1;
        propertyValue.Value = deserializedJson.records[i].retweet;
        list.Add(propertyValue);
        information.PropertieValues.Add(propertyList.ElementAt(8), list);

        list = new List<PropertyValue>();
        for (int j = 0; j < deserializedJson.records[i].urls.Count; j++)
        {
          propertyValue = new PropertyValue();
          propertyValue.PropertyValueId = j + 1;
          propertyValue.Value = deserializedJson.records[i].urls[j];
          list.Add(propertyValue);
        }
        information.PropertieValues.Add(propertyList.ElementAt(9), list);

        list = new List<PropertyValue>();
        for (int j = 0; j < deserializedJson.records[i].mentions.Count; j++)
        {
          propertyValue = new PropertyValue();
          propertyValue.PropertyValueId = j + 1;
          propertyValue.Value = deserializedJson.records[i].mentions[j];
          list.Add(propertyValue);
        }
        information.PropertieValues.Add(propertyList.ElementAt(10), list);
        informationList.Add(information);
      }
    }

    /// <summary>
    /// Method used for testing the contents of the informationList
    /// </summary>
    public void PrintInformationList()
    {
      foreach (var item in informationList)
      {
        Console.WriteLine(String.Format("Post {0}\n--------", item.InformationId));
        foreach (KeyValuePair<Property, ICollection<PropertyValue>> entry in item.PropertieValues)
        {
          Console.Write(String.Format("{0}: ", entry.Key.Name));
          foreach (var property in entry.Value)
          {
            Console.Write(String.Format("{0} ", property.Value));
          }
          Console.WriteLine("");
        }
        Console.WriteLine("");
      }
      Console.ReadKey();
    }

    /// <summary>
    /// Get all the PropertyValues available for politician
    /// </summary>
    /// <param name="key"></param>
    public void FilterProperty(string key)
    {
      ISet<string> set = new HashSet<string>();

      foreach (var item in informationList)
      {
        foreach (KeyValuePair<Property, ICollection<PropertyValue>> entry in item.PropertieValues)
        {
          StringBuilder sb = new StringBuilder();
          foreach (var property in entry.Value)
          {
            if (entry.Key.Name.Equals(key))
            {
              sb.Append(String.Format("{0} ", property.Value));
            }
          }
          set.Add(sb.ToString());
        }
      }

      foreach (var item in set)
      {
        Console.WriteLine(item);
      }
      Console.ReadKey();
    }
  }
}

