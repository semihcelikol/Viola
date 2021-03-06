﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Viola.Models
{
    public class CostDetailViewModels
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("User")]
        public string UserId { get; set; }

        [Required]
        [Column(TypeName = "date")]
        [DisplayName("Date")]
        public DateTime TransDate { get; set; }

        [Required]
        [DisplayName("Cost Type")]
        public int CostTypeId { get; set; }

        [DisplayName("Project")]
        public int ProjectId { get; set; }

        [DisplayName("Task")]
        public int TaskId { get; set; }

        [StringLength(700)]
        [DataType(DataType.MultilineText)]
        [DisplayName("Project description")]
        public string Description { get; set; }

        [DisplayName("TaxNum")]
        public TaxNum TaxNum { get; set; }

        [Required]
        [DisplayName("Amount")]
        public float Amount { get; set; }

        public CostDetailViewModels()
        {
            UserId = User.GetCurrentUserId();
            TransDate = DateTime.Now.ToUniversalTime();
        }
    }
}