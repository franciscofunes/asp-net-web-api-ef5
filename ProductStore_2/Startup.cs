using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProductStore_2.Startup))]
namespace ProductStore_2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
