using AutoMapper;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Viola.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(100)]
        [DisplayName("Full name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DisplayName("Role")]
        public UserRole UserRole { get; set; }

        [Required]
        [DisplayName("Date format")]
        public DateFormat DateFormat { get; set; }

        [Required]
        [DisplayName("Date seperator")]
        public DateSeperator DateSeperator { get; set; }

        [Required]
        [DisplayName("Time zone")]
        public string TimeZoneId { get; set; }

        [Required]
        [StringLength(5, MinimumLength = 2)]
        [DisplayName("Language")]
        public string LanguageId { get; set; }
    }

    public class EditViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Full name")]
        public string FullName { get; set; }

        [Required]
        [DisplayName("Role")]
        public UserRole UserRole { get; set; }

        [DisplayName("Is lockout ?")]
        public bool IsLockout { get; set; }

        [Required]
        [DisplayName("Date format")]
        public DateFormat DateFormat { get; set; }

        [Required]
        [DisplayName("Date seperator")]
        public DateSeperator DateSeperator { get; set; }

        [Required]
        [DisplayName("Time zone")]
        public string TimeZoneId { get; set; }

        [Required]
        [StringLength(5, MinimumLength = 2)]
        [DisplayName("Language")]
        public string LanguageId { get; set; }
    }
}