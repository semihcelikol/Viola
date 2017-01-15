using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Viola.Startup))]
namespace Viola
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
