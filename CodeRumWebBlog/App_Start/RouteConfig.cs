using System.Web.Mvc;
using System.Web.Routing;
using System.Xml.Linq;

namespace CodeRumWebBlog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("{*botdetect}",
            new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            routes.MapRoute(
                name: "Feed",
                url: "feed",
                defaults: new 
                { 
                    controller = "Feeds",
                    action = "Feed" 
                });

            routes.MapRoute(
               name: "Register",
               url: "dang-ky",
               defaults: new
               {
                   controller = "Auths",
                   action = "Register"
               });

            routes.MapRoute(
                name: "List Blog",
                url: "danh-sach-bai-viet",
                defaults: new
                {
                    controller = "Blog",
                    action = "Index"
                });
            routes.MapRoute(
                name: "Detail Blog",
                url: "chi-tiet-{metatitle}-{id}",
                defaults: new
                {
                    controller = "Blog",
                    action = "Detail",
                    id = UrlParameter.Optional,
                    metatitle = UrlParameter.Optional
                });

            routes.MapRoute(
                name: "Detail Profile",
                url: "trang-ca-nhan/{name}#{id}",
                defaults: new
                {
                    controller = "Account",
                    action = "Detail",
                    name = UrlParameter.Optional
                });
            routes.MapRoute(
                name: "List Blog With Tag",
                url: "tag-{tagId}",
                defaults: new
                {
                    controller = "Blog",
                    action = "Tag",
                    id = UrlParameter.Optional
                });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "CodeRumWebBlog.Controllers" }
            );
        }
    }
}
