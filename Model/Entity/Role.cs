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
        public string Id { get; set; }

        [StringLength(150)]
        public string Name { get; set; }
    }
}
