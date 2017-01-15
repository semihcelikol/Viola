using AutoMapper;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Viola.Models
{
    public class CompanyViewModel
    {
        [Required]
        [StringLength(100)]
        [DisplayName("Company name")]
        public string Name { get; set; }


        
    }
}