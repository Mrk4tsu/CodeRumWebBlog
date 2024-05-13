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
        public ActionResult Feed()
        {
            var rssFeedService = new RssFeedService();
            rssFeedService.CreateRssFeed();
            var feed = rssFeedService.ReadRssFeed();
            var rssFormatter = new Rss20FeedFormatter(feed, false);

            using (var memoryStream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(memoryStream))
                {
                    rssFormatter.WriteTo(xmlWriter);
                }

                memoryStream.Position = 0; // Reset the position of the memory stream to be able to read it from the start

                using (var reader = new StreamReader(memoryStream))
                {
                    var rssOutput = reader.ReadToEnd();
                    return Content(rssOutput, "application/xml");
                }
            }
        }
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