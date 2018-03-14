using BAR.BL.Domain.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;

namespace BAR.DAL.EF
{
  internal class BarometerInitializer : DropCreateDatabaseAlways<BarometerDbContext>
  {
    /// <summary>
    /// Dummy data form the json file will be generated
    /// in this file for the first wave of information
    /// </summary>       
    protected override void Seed(BarometerDbContext ctx)
    {
      //Write properties to Set
      Property hashtag = new Property
      {
        Name = "Hashtag"
      };
      Property word = new Property
      {
        Name = "Word"
      };
      Property date = new Property
      {
        Name = "Date"
      };
      Property politician = new Property
      {
        Name = "Politician"
      };
      Property geo = new Property
      {
        Name = "Geo"
      };
      Property postId = new Property
      {
        Name = "PostId"
      };
      Property userId = new Property
      {
        Name = "UserId"
      };
      Property sentiment = new Property
      {
        Name = "Sentiment"
      };
      Property retweet = new Property
      {
        Name = "Retweet"
      };
      Property url = new Property
      {
        Name = "Url"
      };
      Property mention = new Property
      {
        Name = "Mention" 
      };

      ctx.Properties.Add(hashtag);
      ctx.Properties.Add(word);
      ctx.Properties.Add(date);
      ctx.Properties.Add(politician);
      ctx.Properties.Add(geo);
      ctx.Properties.Add(postId);
      ctx.Properties.Add(userId);

      //TODO: implement logic
      string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.json");
      string json = File.ReadAllText(path);
      dynamic deserializedJson = JsonConvert.DeserializeObject(json);

      for (int i = 0; i < deserializedJson.records.Count; i++)
      {
        PropertyValue propertyValue;
        Information information = new Information();
        information.InformationId = i + 1;
        information.Properties = new Dictionary<Property, ICollection<PropertyValue>>();

        List<PropertyValue> list = new List<PropertyValue>();
        for (int j = 0; j < deserializedJson.records[i].hashtags.Count; j++)
        {
          propertyValue = new PropertyValue();
          propertyValue.PropertyValueId = j + 1;
          propertyValue.Value = deserializedJson.records[i].hashtags[j];
          list.Add(propertyValue);
        }
        information.Properties.Add(propertyList.ElementAt(0), list);

        list = new List<PropertyValue>();
        for (int j = 0; j < deserializedJson.records[i].words.Count; j++)
        {
          propertyValue = new PropertyValue();
          propertyValue.PropertyValueId = j + 1;
          propertyValue.Value = deserializedJson.records[i].words[j];
          list.Add(propertyValue);
        }
        information.Properties.Add(propertyList.ElementAt(1), list);

        list = new List<PropertyValue>();
        propertyValue = new PropertyValue();
        propertyValue.PropertyValueId = 1;
        propertyValue.Value = deserializedJson.records[i].date;
        list.Add(propertyValue);
        information.Properties.Add(propertyList.ElementAt(2), list);

        list = new List<PropertyValue>();
        for (int j = 0; j < deserializedJson.records[i].politician.Count; j++)
        {
          propertyValue = new PropertyValue();
          propertyValue.PropertyValueId = j + 1;
          propertyValue.Value = deserializedJson.records[i].politician[j];
          list.Add(propertyValue);
        }
        information.Properties.Add(propertyList.ElementAt(3), list);

        list = new List<PropertyValue>();
        propertyValue = new PropertyValue();
        propertyValue.PropertyValueId = 1;
        propertyValue.Value = deserializedJson.records[i].geo;
        list.Add(propertyValue);
        information.Properties.Add(propertyList.ElementAt(4), list);

        list = new List<PropertyValue>();
        propertyValue = new PropertyValue();
        propertyValue.PropertyValueId = 1;
        propertyValue.Value = deserializedJson.records[i].id;
        list.Add(propertyValue);
        information.Properties.Add(propertyList.ElementAt(5), list);

        list = new List<PropertyValue>();
        propertyValue = new PropertyValue();
        propertyValue.PropertyValueId = 1;
        propertyValue.Value = deserializedJson.records[i].user_id;
        list.Add(propertyValue);
        information.Properties.Add(propertyList.ElementAt(6), list);

        list = new List<PropertyValue>();
        for (int j = 0; j < deserializedJson.records[i].sentiment.Count; j++)
        {
          propertyValue = new PropertyValue();
          propertyValue.PropertyValueId = j + 1;
          propertyValue.Value = deserializedJson.records[i].sentiment[j];
          list.Add(propertyValue);
        }
        information.Properties.Add(propertyList.ElementAt(7), list);

        list = new List<PropertyValue>();
        propertyValue = new PropertyValue();
        propertyValue.PropertyValueId = 1;
        propertyValue.Value = deserializedJson.records[i].retweet;
        list.Add(propertyValue);
        information.Properties.Add(propertyList.ElementAt(8), list);

        list = new List<PropertyValue>();
        for (int j = 0; j < deserializedJson.records[i].urls.Count; j++)
        {
          propertyValue = new PropertyValue();
          propertyValue.PropertyValueId = j + 1;
          propertyValue.Value = deserializedJson.records[i].urls[j];
          list.Add(propertyValue);
        }
        information.Properties.Add(propertyList.ElementAt(9), list);

        list = new List<PropertyValue>();
        for (int j = 0; j < deserializedJson.records[i].mentions.Count; j++)
        {
          propertyValue = new PropertyValue();
          propertyValue.PropertyValueId = j + 1;
          propertyValue.Value = deserializedJson.records[i].mentions[j];
          list.Add(propertyValue);
        }
        information.Properties.Add(propertyList.ElementAt(10), list);
        informationList.Add(information);

      }
    }
  }
