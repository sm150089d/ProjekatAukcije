using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjekatAukcije.Startup))]
namespace ProjekatAukcije
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            app.MapSignalR();
            ConfigureAuth(app);
         
        }
    }
}
