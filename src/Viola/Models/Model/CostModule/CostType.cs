using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Viola.Models
{
    public class CostType
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        public virtual ICollection<CostDetail> CostDetails { get; set; }
    }
}