using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace CodeRumWebBlog.Controllers
{
    public class FeedsController : Controller
    {
        // GET: Feeds
        public ActionResult Index()
        {
            var service = new RssFeedService();

            var feed = service.ReadRssFeed();

            var settings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                Indent = true,
                NewLineHandling = NewLineHandling.Entitize,
                NewLineOnAttributes = true
            };
            using (var stream = new MemoryStream())
            {
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    var formatter = new Rss20FeedFormatter(feed);
                    formatter.WriteTo(writer);
                }

                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    var content = reader.ReadToEnd();
                    return Content(content, "application/rss+xml", Encoding.UTF8);
                }
            }
        }
    }
}