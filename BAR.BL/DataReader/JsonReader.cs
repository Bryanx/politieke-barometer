using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.DataReader
{
  public class JsonReader
  {
    public void ReadJson()
    {
      string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.json");
      string json = File.ReadAllText(path);
      dynamic deserializedJson = JsonConvert.DeserializeObject(json);

      Console.WriteLine("JSONREADER FINISHED");
    }
  }
}
