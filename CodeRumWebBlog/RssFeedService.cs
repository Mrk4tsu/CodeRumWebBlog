using Model.DAO;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using System.Xml;

namespace CodeRumWebBlog
{
    public class RssFeedService
    {
        private readonly ContentDAO _contentDao;

        private const string pathRssFeed = @"d:\DZHosts\LocalUser\mrkatsu2212\www.mrkatsu.somee.com\rss.xml";
        public RssFeedService()
        {
            _contentDao = new ContentDAO();
        }
        public SyndicationFeed ReadRssFeed()
        {
            using (var reader = XmlReader.Create(pathRssFeed))
            {
                var feed = SyndicationFeed.Load(reader);
                return feed;
            }
        }
        public void CreateRssFeed()
        {
            var items = new List<SyndicationItem>();
            var contents = _contentDao.GetRecentPosts();

            foreach (var content in contents)
            {
                var item = new SyndicationItem(
                    content.Name,
                    content.Description,
                    new Uri("https://gatapchoi.id.vn/chi-tiet-" + content.MetaTitle + "-" + content.Id), // Replace with the URL of the content
                    content.Id.ToString(),
                    content.CreateAt.GetValueOrDefault());

                items.Add(item);
            }

            var feed = new SyndicationFeed("MrK4tsuBlog", "Bài viết mới của Katsu", new Uri("https://gatapchoi.id.vn"), items)
            {
                LastUpdatedTime = DateTimeOffset.Now
            };

            //var path = @"d:\DZHosts\LocalUser\mrkatsu2212\www.mrkatsu.somee.com\rss.xml";
            using (var writer = XmlWriter.Create(pathRssFeed))
            {
                var formatter = new Rss20FeedFormatter(feed);
                formatter.WriteTo(writer);
            }
        }
        //public void CreateRssFeed()
        //{
        //    var items = new List<SyndicationItem>();
        //    var contents = _contentDao.GetRecentPosts();

        //    foreach (var content in contents)
        //    {
        //        var item = new SyndicationItem(
        //            content.Name,
        //            content.Description,
        //            new Uri("http://localhost:44326/" + "chi-tiet-" + content.MetaTitle + content.Id), // Replace with the URL of the content
        //            content.Id.ToString(),
        //            content.CreateAt.GetValueOrDefault());

        //        items.Add(item);
        //    }

        //    var feed = new SyndicationFeed("Blog's MrK4tsu", "Description", new Uri("http://localhost:44326/"), items)
        //    {
        //        LastUpdatedTime = DateTimeOffset.Now
        //    };

        //    using (var writer = XmlWriter.Create("rss.xml"))
        //    {
        //        var formatter = new Rss20FeedFormatter(feed);
        //        formatter.WriteTo(writer);
        //    }
        //}
    }
}