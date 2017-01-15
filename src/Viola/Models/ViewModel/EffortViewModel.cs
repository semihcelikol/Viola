using AutoMapper;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Viola.Models
{
    public class EffortViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [DisplayName("Task")]
        public int TaskId { get; set; }

        [Required]
        [DisplayName("User")]
        public string UserId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Date")]
        public DateTime TransDate { get; set; }

        [StringLength(500)]
        [DataType(DataType.MultilineText)]
        [DisplayName("Description")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Size")]
        public decimal Size { get; set; }

        [DisplayName("Is billable?")]
        public bool Billable { get; set; }


        public EffortViewModel()
        {
            TransDate = DateTime.Now.ToUniversalTime();
            Billable = true;
        }


       
    }
}