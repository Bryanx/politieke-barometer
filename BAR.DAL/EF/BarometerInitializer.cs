using BAR.BL.Domain.Data;
using BAR.BL.Domain.Items;
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
  internal class BarometerInitializer : DropCreateDatabaseIfModelChanges<BarometerDbContext>
  {
    /// <summary>
    /// Dummy data from the json file will be generated
    /// in this file for the first wave of information.
    /// </summary>
    protected override void Seed(BarometerDbContext ctx)
    {
      GenerateSources(ctx);
      GenerateProperties(ctx);
      GenerateAreas(ctx);
    }

    /// <summary>
    /// Generates all of the sources we get our information from.
    /// </summary>
    private void GenerateSources(BarometerDbContext ctx)
    {
      Source source = new Source
      {
        Name = "Twitter",
        SourceLine = "twitter.com"
      };
      ctx.Sources.Add(source);
      ctx.SaveChanges();
    }

    /// <summary>
    /// Performance gain if you put all items in a list
    /// and then in the context with addRange();
    /// </summary>
    private void GenerateProperties(BarometerDbContext ctx)
    {
      List<Property> propertiesList = new List<Property>();

      Property gender = new Property
      {
        Name = "Gender"
      };
      Property age = new Property
      {
        Name = "Age"
      };
      Property education = new Property
      {
        Name = "Education"
      };
      Property language = new Property
      {
        Name = "Language"
      };
      Property personality = new Property
      {
        Name = "Personality"
      };
      Property word = new Property
      {
        Name = "Word"
      };
      Property sentiment = new Property
      {
        Name = "Sentiment"
      };
      Property hashtag = new Property
      {
        Name = "Hashtag"
      };
      Property mention = new Property
      {
        Name = "Mention"
      };
      Property url = new Property
      {
        Name = "Url"
      };
      Property date = new Property
      {
        Name = "Date"
      };
      Property geo = new Property
      {
        Name = "Geo"
      };
      Property postId = new Property
      {
        Name = "PostId"
      };     
      Property retweet = new Property
      {
        Name = "Retweet"
      };

      propertiesList.Add(gender);
      propertiesList.Add(age);
      propertiesList.Add(education);
      propertiesList.Add(language);
      propertiesList.Add(personality);
      propertiesList.Add(word);
      propertiesList.Add(sentiment);
      propertiesList.Add(hashtag);
      propertiesList.Add(mention);
      propertiesList.Add(url);
      propertiesList.Add(date);
      propertiesList.Add(geo);
      propertiesList.Add(postId);
      propertiesList.Add(retweet);

      ctx.Properties.AddRange(propertiesList);
      ctx.SaveChanges();
    }

    /// <summary>
    /// Reads all Belgium cities from Json.
    /// </summary>
    /// <param name="ctx"></param>
    private void GenerateAreas(BarometerDbContext ctx)
    {
      string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "zipcode-belgium.json");
      string json = File.ReadAllText(path);
      dynamic deserializedJson = JsonConvert.DeserializeObject(json);

      for (int i = 0; i < deserializedJson.Count; i++)
      {
        Area area = new Area
        {
          Country = "België",
          Residence = deserializedJson[i].city
        };
        ctx.Areas.Add(area);
      }
      ctx.SaveChanges();
    }
  }
}

