using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entity
{
    [Table("UserGroup")]
    public partial class UserGroup
    {
        [StringLength(20)]
        public string Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
    }
}
