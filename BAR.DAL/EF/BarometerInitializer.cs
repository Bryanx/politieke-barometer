using BAR.BL.Domain.Data;
using BAR.BL.Domain.Users;
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
      GenerateUsers(ctx);


      //Write properties to Set
      //Property hashtag = new Property
      //{
      //    Name = "Hashtag"
      //};
      //Property word = new Property
      //{
      //    Name = "Word"
      //};
      //Property date = new Property
      //{
      //    Name = "Date"
      //};
      //Property politician = new Property
      //{
      //    Name = "Politician"
      //};
      //Property geo = new Property
      //{
      //    Name = "Geo"
      //};
      //Property postId = new Property
      //{
      //    Name = "PostId"
      //};
      //Property userId = new Property
      //{
      //    Name = "UserId"
      //};
      //Property sentiment = new Property
      //{
      //    Name = "Sentiment"
      //};
      //Property retweet = new Property
      //{
      //    Name = "Retweet"
      //};
      //Property url = new Property
      //{
      //    Name = "Url"
      //};
      //Property mention = new Property
      //{
      //    Name = "Mention"
      //};

      //ctx.Properties.Add(hashtag);
      //ctx.Properties.Add(word);
      //ctx.Properties.Add(date);
      //ctx.Properties.Add(politician);
      //ctx.Properties.Add(geo);
      //ctx.Properties.Add(postId);
      //ctx.Properties.Add(userId);
      //ctx.Properties.Add(sentiment);
      //ctx.Properties.Add(retweet);
      //ctx.Properties.Add(url);
      //ctx.Properties.Add(mention);
      //ctx.SaveChanges();

      //TODO: implement logic
      //string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.json");
      //string json = File.ReadAllText(path);
      //dynamic deserializedJson = JsonConvert.DeserializeObject(json);

      //for (int i = 0; i < deserializedJson.records.Count; i++)
      //{
      //    PropertyValue propertyValue;
      //    Information information = new Information();
      //    information.PropertieValues = new List<PropertyValue>();

      //    for (int j = 0; j < deserializedJson.records[i].hashtags.Count; j++)
      //    {
      //        propertyValue = new PropertyValue
      //        {
      //            Property = ctx.Properties.Find(1),
      //            Value = deserializedJson.records[i].hashtags[j]
      //        };
      //        information.PropertieValues.Add(propertyValue);
      //    }
      //    ctx.Informations.Add(information);

      //}
    }

    private void GenerateUsers(BarometerDbContext ctx)
    {
      User bryan = new User()
      {
        UserId = 1,
        FirstName = "Bryan",
        Gender = Gender.MAN
      };

      User maarten = new User()
      {
        UserId = 2,
        FirstName = "Maarten",
        Gender = Gender.MAN
      };

      User remi = new User()
      {
        UserId = 2,
        FirstName = "Remi",
        Gender = Gender.MAN
      };

      User anthony = new User()
      {
        UserId = 2,
        FirstName = "Anthony",
        Gender = Gender.MAN
      };

      User jarne = new User()
      {
        UserId = 2,
        FirstName = "Jarne",
        Gender = Gender.MAN
      };

      User yoni = new User()
      {
        UserId = 2,
        FirstName = "Yoni",
        Gender = Gender.MAN
      };

      ctx.Users.Add(bryan);
      ctx.Users.Add(maarten);
      ctx.Users.Add(anthony);
      ctx.Users.Add(remi);
      ctx.Users.Add(yoni);
      ctx.Users.Add(jarne);

      ctx.SaveChanges();

    }
  }
}
