using BAR.BL.Domain;
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
  internal class BarometerInitializer : DropCreateDatabaseAlways<BarometerDbContext>
  {
    /// <summary>
    /// Dummy data from the json file will be generated
    /// in this file for the first wave of information.
    /// </summary>
    protected override void Seed(BarometerDbContext ctx)
    {
      GenerateSubPlatforms(ctx);
      GenerateSources(ctx);
      GenerateProperties(ctx);
      GenerateInformations(ctx);
      GenerateAreas(ctx);
      GenerateK3(ctx);
    }

    /// <summary>
    /// Generates test data for K3 subplatform
    /// </summary>
    /// <param name="ctx"></param>
    private void GenerateK3(BarometerDbContext ctx)
    {
      Person Marthe = new Person()
      {
        Name = "Marthe De Pillecyn",
        CreationDate = DateTime.Now,
        Baseline = 0,
        TrendingPercentage = 0,
        SubPlatform = ctx.SubPlatforms.Where(sp => sp.Name.Equals("K3")).SingleOrDefault()

      };

      ctx.Items.Add(Marthe);
    }

    /// <summary>
    /// Generates some subplatforms.
    /// </summary>
    /// <param name="ctx"></param>
    private void GenerateSubPlatforms(BarometerDbContext ctx)
    {
      SubPlatform subPlatform1 = new SubPlatform
      {
        Name = "K3",
        CreationDate = DateTime.Now
      };
      ctx.SubPlatforms.Add(subPlatform1);

      SubPlatform subPlatform2 = new SubPlatform
      {
        Name = "politiek",
        CreationDate = DateTime.Now
      };
      ctx.SubPlatforms.Add(subPlatform2);

      ctx.SaveChanges();
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

      Property hashtag = new Property
      {
        Name = "Hashtag"
      };
      Property word = new Property
      {
        Name = "Word"
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

      propertiesList.Add(hashtag);
      propertiesList.Add(word);
      propertiesList.Add(geo);
      propertiesList.Add(postId);
      propertiesList.Add(userId);
      propertiesList.Add(sentiment);
      propertiesList.Add(retweet);
      propertiesList.Add(url);
      propertiesList.Add(mention);

      ctx.Properties.AddRange(propertiesList);
      ctx.SaveChanges();
    }

    /// <summary>
    /// Generates information objects based on the
    /// JSON-file.
    /// </summary>
    private void GenerateInformations(BarometerDbContext ctx)
    {
      string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.json");
      string json = File.ReadAllText(path);
      dynamic deserializedJson = JsonConvert.DeserializeObject(json);

      for (int i = 0; i < 150; i++)
      {
        PropertyValue propertyValue;
        Information information = new Information
        {
          PropertieValues = new List<PropertyValue>()
        };
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
        //Read geo
        propertyValue = new PropertyValue
        {
          Property = ctx.Properties.Where(x => x.Name.Equals("Geo")).SingleOrDefault(),
          Value = deserializedJson.records[i].geo,
          Confidence = 1
        };
        information.PropertieValues.Add(propertyValue);
        //Read postId
        propertyValue = new PropertyValue
        {
          Property = ctx.Properties.Where(x => x.Name.Equals("PostId")).SingleOrDefault(),
          Value = deserializedJson.records[i].id,
          Confidence = 1
        };
        information.PropertieValues.Add(propertyValue);
        //Read userId
        propertyValue = new PropertyValue
        {
          Property = ctx.Properties.Where(x => x.Name.Equals("UserId")).SingleOrDefault(),
          Value = deserializedJson.records[i].user_id,
          Confidence = 1
        };
        information.PropertieValues.Add(propertyValue);
        //Read sentiment
        propertyValue = new PropertyValue
        {
          Property = ctx.Properties.Where(x => x.Name.Equals("Sentiment")).SingleOrDefault(),
          Value = deserializedJson.records[i].sentiment[0],
          Confidence = deserializedJson.records[i].sentiment[1]
        };
        information.PropertieValues.Add(propertyValue);
        //Read retweet
        propertyValue = new PropertyValue
        {
          Property = ctx.Properties.Where(x => x.Name.Equals("Retweet")).SingleOrDefault(),
          Value = deserializedJson.records[i].retweet,
          Confidence = 1
        };
        information.PropertieValues.Add(propertyValue);
        //Read urls
        for (int j = 0; j < deserializedJson.records[i].urls.Count; j++)
        {
          propertyValue = new PropertyValue
          {
            Property = ctx.Properties.Where(x => x.Name.Equals("Url")).SingleOrDefault(),
            Value = deserializedJson.records[i].urls[j],
            Confidence = 1
          };
          information.PropertieValues.Add(propertyValue);
        }
        //Read mentions
        for (int j = 0; j < deserializedJson.records[i].mentions.Count; j++)
        {
          propertyValue = new PropertyValue
          {
            Property = ctx.Properties.Where(x => x.Name.Equals("Mention")).SingleOrDefault(),
            Value = deserializedJson.records[i].mentions[j],
            Confidence = 1
          };
          information.PropertieValues.Add(propertyValue);
        }
        //Add source
        string source = Convert.ToString(deserializedJson.records[i].source);
        information.Source = ctx.Sources.Where(s => s.Name.ToLower().Equals(source)).SingleOrDefault();
        //Add connection to Item (Person)
        string personFullName = String.Format("{0} {1}", deserializedJson.records[i].politician[0], deserializedJson.records[i].politician[1]);
        information.Item = GeneratePeople(personFullName, ctx);
        //Add date
        string datum = Convert.ToString(deserializedJson.records[i].date);
        DateTime myInfoDate = DateTime.ParseExact(datum, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
        information.CreatetionDate = myInfoDate;

        //Add information object to the DbSet
        ctx.Informations.Add(information);
      }
      ctx.SaveChanges();
    }

    /// <summary>
    /// Will return the ID of the people
    /// If the person does not exist, he will be created.
    /// </summary>
    private Item GeneratePeople(string personFullName, BarometerDbContext ctx)
    {

      Item person = ctx.Items.Where(i => i.Name.Equals(personFullName)).SingleOrDefault();

      if (person == null)
      {
        person = new Person()
        {
          Name = personFullName,
          CreationDate = DateTime.Now,
          Baseline = 0,
          TrendingPercentage = 0
        };
        ctx.Items.Add(person);
        ctx.SaveChanges();
      }

      return person;
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

