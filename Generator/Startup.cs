using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Generator.Startup))]
namespace Generator
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
