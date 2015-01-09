using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GamePoolWeb2.Startup))]
namespace GamePoolWeb2
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
