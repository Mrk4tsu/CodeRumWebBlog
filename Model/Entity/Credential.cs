using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entity
{
    [Table("Credential")]
    [Serializable]
    public partial class Credential
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserGroupId { get; set; }
        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string RoleId { get; set; }
    }
}
