namespace Model.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Content")]
    public partial class Content
    {
        public long Id { get; set; }

        [StringLength(250)]
        [Display(Name= "Tiêu đề")]
        public string Name { get; set; }

        [StringLength(250)]
        public string MetaTitle { get; set; }

        [StringLength(500)]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [StringLength(250)]
        [Display(Name = "Ảnh bìa nội dung")]
        public string Image { get; set; }

        [Column(TypeName = "ntext")]
        [Display(Name = "Chi tiết bài viết")]
        public string Detail { get; set; }
        [Display(Name = "Danh mục bài viết")]
        public long? CategoryId { get; set; }

        public DateTime? CreateAt { get; set; }

        [StringLength(50)]
        public string CreateBy { get; set; }

        public DateTime? ModifyDate { get; set; }

        [StringLength(50)]
        public string ModifyBy { get; set; }

        [StringLength(100)]
        [Display(Name = "Keyword")]
        public string MetaKeyword { get; set; }

        public bool? TopHot { get; set; }

        public int? ViewCount { get; set; }

        [StringLength(500)]
        [Display(Name = "Tag")]
        public string Tag { get; set; }

        public bool Status { get; set; }
        public bool? Locked { get; set; }
    }
}
