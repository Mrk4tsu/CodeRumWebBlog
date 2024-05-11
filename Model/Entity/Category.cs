namespace Model.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Globalization;

    [Table("Category")]
    public partial class Category
    {
        public long Id { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(250)]
        public string MetaTitle { get; set; }

        public long? ParentId { get; set; }

        public int? DisplayOrder { get; set; }

        [StringLength(100)]
        public string SeoTitle { get; set; }

        [StringLength(250)]
        public string MetaKeyword { get; set; }

        [StringLength(50)]
        public string CreateBy { get; set; }
        public DateTime? CreateAt { get; set; }

        public DateTime? ModifyDate { get; set; }

        [StringLength(1)]
        public string ModifyBy { get; set; }

        public bool? ShowOnHome { get; set; }

        public bool Status { get; set; }
    }
}
