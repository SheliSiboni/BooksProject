using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BooksReview.Startup))]
namespace BooksReview
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
