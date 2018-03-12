using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.BL.Domain.Data
{
  public class Record
  {
    public string[] hashtag { get; set; }
    public string[] words { get; set; }
    public DateTime? date { get; set; }
    public string[] politician { get; set; }
    public string geo { get; set; }
    public int? id { get; set; }
    public string user_id { get; set; }
    public double?[] sentiment { get; set; }
    public bool retweet { get; set; }
    public string source { get; set; }
    public string[] urls { get; set; }
    public string[] mentions { get; set; }
  }
}
