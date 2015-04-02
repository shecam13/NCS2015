using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NetPressAssignment.Startup))]
namespace NetPressAssignment
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
