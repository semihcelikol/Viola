using AutoMapper;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Viola.Models
{
    public class ProjectViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [DisplayName("Project name")]
        public string Name { get; set; }

        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        [DisplayName("Project description")]
        public string Description { get; set; }

        [Column(TypeName = "date")]
        [DisplayName("Start date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        [DisplayName("End date")]
        public DateTime? EndDate { get; set; }

        [Required]
        [DisplayName("Status")]
        public ProjectStatus Status { get; set; }

        [Required]
        [DisplayName("Project manager")]
        public string ManagerUserId { get; set; }



      
    }
}
