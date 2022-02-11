using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Formula1DataApplication.Startup))]
namespace Formula1DataApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
