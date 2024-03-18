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
        public string Name { get; set; }

        [StringLength(250)]
        public string MetaTitle { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(250)]
        public string Image { get; set; }

        [Column(TypeName = "ntext")]
        public string Detail { get; set; }

        public long? CategoryId { get; set; }

        public DateTime? CreateAt { get; set; }

        [StringLength(50)]
        public string CreateBy { get; set; }

        public DateTime? ModifyDate { get; set; }

        [StringLength(1)]
        public string ModifyBy { get; set; }

        [StringLength(100)]
        public string MetaKeyword { get; set; }

        public bool? TopHot { get; set; }

        public int? ViewCount { get; set; }

        [StringLength(500)]
        public string Tag { get; set; }

        public bool? Status { get; set; }
    }
}
