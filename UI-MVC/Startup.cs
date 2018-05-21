using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BAR.UI.MVC.Startup))]

namespace BAR.UI.MVC
{
  public partial class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      ConfigureAuth(app);
    }
  }
}
