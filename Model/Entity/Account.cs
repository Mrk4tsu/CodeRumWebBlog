namespace Model.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Account")]
    public partial class Account
    {
        public long Id { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(100)]
        public string Password { get; set; }

        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        public DateTime? CreateAt { get; set; }

        public DateTime? ModifyDate { get; set; }

        [StringLength(1)]
        public string ModifyBy { get; set; }

        public bool Status { get; set; }
    }
}
