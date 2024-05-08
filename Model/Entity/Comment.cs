namespace Model.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Comment")]
    public partial class Comment
    {
        public long Id { get; set; }

        [Column(TypeName = "ntext")]
        public string Content { get; set; }

        public long? ParentId { get; set; }

        [StringLength(50)]
        public string CreateBy { get; set; }

        public DateTime? CreateAt { get; set; }

        public DateTime? ModifyDate { get; set; }

        [StringLength(50)]
        public string ModifyBy { get; set; }

        public long? PostId { get; set; }

        public bool? Status { get; set; }
    }
}
