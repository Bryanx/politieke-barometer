using BAR.DAL;
using BAR.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAR.UI.CA
{
  /// <summary>
  /// In this class we test te logic of generating the alerts
  /// in two waves. The second wave needs to generate new alerts
  /// </summary>
  public class Program
  {
    public static void Main(string[] args)
    {
      //wave 1
      //Are setup is based on the json-dump data.
      //TODO: implement logic
      //InformationRepository informationRepository = new InformationRepository();
      //informationRepository.PrintInformationList();
      //informationRepository.FilterProperty("Politician");
      BarometerDbContext ctx = new BarometerDbContext();
      //wave 2
    }//Needs to generate new alerts when HC data is added.
     //TOCO: implement logic
  }
}

