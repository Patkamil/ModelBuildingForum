using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PatrykKamińskiProjektPZ.Startup))]
namespace PatrykKamińskiProjektPZ
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
