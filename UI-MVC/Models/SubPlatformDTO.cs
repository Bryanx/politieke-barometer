using System;
using System.ComponentModel.DataAnnotations;
using BAR.UI.MVC.App_GlobalResources;

namespace BAR.UI.MVC.Models
{
  public class SubPlatformDTO
  {
    public int SubPlatformId { get; set; }
    [Display(Name = "Name", ResourceType = typeof(Resources))]
    public string Name { get; set; }
    [Display(Name = "NumberOfUsers", ResourceType = typeof(Resources))]
    public int NumberOfUsers { get; set; }
    [Display(Name = "CreationDate", ResourceType = typeof(Resources))]
    public DateTime CreationDate { get; set; }
  }
}