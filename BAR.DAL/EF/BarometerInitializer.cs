using BAR.BL.Domain.Data;
using BAR.BL.Domain.Users;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;

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

      Source c = new Source
      {
        Name = "Twitter",
        SourceLine = "twitter.com"
      };

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
      ctx.Properties.Add(sentiment);
      ctx.Properties.Add(retweet);
      ctx.Properties.Add(url);
      ctx.Properties.Add(mention);
      ctx.SaveChanges();

      //TODO: implement logic
      string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.json");
      string json = File.ReadAllText(path);
      dynamic deserializedJson = JsonConvert.DeserializeObject(json);

      for (int i = 0; i < 5; i++)
      {
        PropertyValue propertyValue;
        Information information = new Information
        {
          PropertieValues = new List<PropertyValue>()
        };
        Console.WriteLine(String.Format("Record {0}", i+1));
        //Read hashtags
        for (int j = 0; j < deserializedJson.records[i].hashtags.Count; j++)
        {
          propertyValue = new PropertyValue
          {
            Property = ctx.Properties.Where(x => x.Name.Equals("Hashtag")).SingleOrDefault(),
            Value = deserializedJson.records[i].hashtags[j],
            Confidence = 1
          };
          information.PropertieValues.Add(propertyValue);
        }
        //Read words
        for (int j = 0; j < deserializedJson.records[i].words.Count; j++)
        {
          propertyValue = new PropertyValue
          {
            Property = ctx.Properties.Where(x => x.Name.Equals("Word")).SingleOrDefault(),
            Value = deserializedJson.records[i].words[j],
            Confidence = 1
          };
          information.PropertieValues.Add(propertyValue);
        }
        //Read date
        propertyValue = new PropertyValue
        {
          Property = ctx.Properties.Where(x => x.Name.Equals("Date")).SingleOrDefault(),
          Value = deserializedJson.records[i].date,
          Confidence = 1
        };
        information.PropertieValues.Add(propertyValue);
        //Read politician
        propertyValue = new PropertyValue
        {
          Property = ctx.Properties.Where(x => x.Name.Equals("Politician")).SingleOrDefault(),
          Value = String.Format("{0} {1}", deserializedJson.records[i].politician[0], deserializedJson.records[i].politician[1]),
          Confidence = 1
        };
        information.PropertieValues.Add(propertyValue);
        ctx.Informations.Add(information);
      }
      ctx.SaveChanges();

      User bryan = new User()
      {
        FirstName = "Bryan",
        Gender = Gender.MAN
      };

      User maarten = new User()
      {
        FirstName = "Maarten",
        Gender = Gender.OTHER
      };

      User remi = new User()
      {
        FirstName = "Remi",
        Gender = Gender.MAN
      };

      User anthony = new User()
      {
        FirstName = "Anthony",
        Gender = Gender.MAN
      };

      User jarne = new User()
      {
        FirstName = "Jarne",
        Gender = Gender.MAN
      };

      User yoni = new User()
      {
        FirstName = "Yoni",
        Gender = Gender.MAN
      };

      ctx.Users.Add(bryan);
      ctx.Users.Add(bryan);
      ctx.Users.Add(anthony);
      ctx.Users.Add(remi);
      ctx.Users.Add(yoni);
      ctx.Users.Add(jarne);

      ctx.SaveChanges();

    }
  }
}
