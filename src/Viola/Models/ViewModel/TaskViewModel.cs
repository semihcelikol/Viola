using AutoMapper;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Viola.Models
{
    public class TaskViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [DisplayName("Task name")]
        public string Name { get; set; }

        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        [DisplayName("Description")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Project")]
        public int ProjectId { get; set; }

        [Column(TypeName = "date")]
        [DisplayName("Start date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        [DisplayName("Due date")]
        public DateTime? DueDate { get; set; }

        [DisplayName("Estimated effort size")]
        public int? EstimatedEffortSize { get; set; }

        [Required]
        [DisplayName("Priority")]
        public TaskPriority Priority { get; set; }

        [Required]
        [DisplayName("Status")]
        public TaskStatus Status { get; set; }

        [StringLength(255)]
        [DisplayName("External link")]
        public string ExternalLink { get; set; }

        [StringLength(255)]
        [DisplayName("External ref")]
        public string ExternalRef { get; set; }



        public TaskViewModel()
        {
            Priority = TaskPriority.Normal;
        }


       
    }
}