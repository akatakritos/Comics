using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Comics.Web.Startup))]
namespace Comics.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

        }
    }
}
