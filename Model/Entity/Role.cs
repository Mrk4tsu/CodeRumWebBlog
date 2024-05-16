using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entity
{
    [Table("Role")]
    public partial class Role
    {
        [StringLength(50)]
        public string Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }
    }
}
