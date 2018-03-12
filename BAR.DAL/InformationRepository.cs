using BAR.BL.Domain.Data;
using Newtonsoft.Json;
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
    private List<Information> information;
    private List<Property> properties;

    public InformationRepository()
    {
      information = new List<Information>();
      properties = new List<Property>();
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
      properties.Add(hashtag);
      properties.Add(word);
      properties.Add(date);
      properties.Add(politician);
      properties.Add(geo);
      properties.Add(postId);
      properties.Add(userId);
      properties.Add(sentiment);
      properties.Add(retweet);
      properties.Add(url);
      properties.Add(mention);
    }

    //public static void ReadJson()
    //{
    //  JsonTextReader reader = new JsonTextReader(new StringReader());
    //  while (reader.Read())
    //  {
    //    if (reader.Value != null)
    //    {
    //      Console.WriteLine("Token: {0}, Value: {1}", reader.TokenType, reader.Value);
    //    }
    //    else
    //    {
    //      Console.WriteLine("Token: {0}", reader.TokenType);
    //    }
    //  }

    //  Console.WriteLine();
    //  Console.ReadKey();
    //}
  }
}
