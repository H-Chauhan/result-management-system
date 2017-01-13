using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RMS.Startup))]
namespace RMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
