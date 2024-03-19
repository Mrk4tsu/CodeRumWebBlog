using System;

namespace Model.ViewModel
{
    public class ContentViewModel
    {
        public long Id { set; get; }
        public string Images { set; get; }
        public string Name { set; get; }
        public decimal? Price { set; get; }
        public string CateName { set; get; }
        public string CateMetaTitle { set; get; }
        public string MetaTitle { set; get; }
        public DateTime? CreatedDate { set; get; }
    }
}
