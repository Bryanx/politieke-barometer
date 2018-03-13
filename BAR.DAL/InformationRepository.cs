using BAR.BL.Domain.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

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
    }

    public int GetAantalInfo(int itemId, DateTime since)
    {
      //TODO: implement logic
      return 0;
    }

    public void GenerateProperties()
    {
      //Make all properties
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

      //Add all properties
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

    public void ReadJson()
    {
      string json = File.ReadAllText("C:\\Users\\remi\\Google Drive\\KdG\\Leerstof\\2017 - 2018\\Integratieproject\\Repo\\politieke-barometer\\BAR.DAL\\data.json");
      dynamic deserializedJson = JsonConvert.DeserializeObject(json);
      for (int i = 0; i < deserializedJson.records.Count; i++)
      {
        Information information = new Information();
        information.InformationId = i+1;
        information.Properties = new Dictionary<Property, ICollection<PropertyValue>>();

        List<PropertyValue> list = new List<PropertyValue>();
        for (int j = 0; j < deserializedJson.records[i].words.Count; j++)
        {
          PropertyValue propertyValue = new PropertyValue();
          propertyValue.PropertyValueId = j + 1;
          propertyValue.Value = deserializedJson.records[i].words[j];
          list.Add(propertyValue);
        }

        information.Properties.Add(propertyList.ElementAt(1), list);

        Console.ReadKey();
      }
      //information.Properties.Add(properties.ElementAt(0), list);
    }
  }
}
