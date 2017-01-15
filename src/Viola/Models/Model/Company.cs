using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AutoMapper;
using Viola.DAL;

namespace Viola.Models
{
    public class Company
    {
        [Required]
        public int Id { get; set; }

        [DisplayName("Created datetime")]
        public DateTime CreatedDatetime { get; set; }

        [DisplayName("Modified datetime")]
        public DateTime? ModifiedDatetime { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Company name")]
        public string Name { get; set; }


        // fk
        // ...

        // child relations
        public virtual ICollection<Effort> Efforts { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<TaskAssignedUser> TaskAssingedUsers { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<ProjectTeam> ProjectTeams { get; set; }


        // varsayılan değerler
        public void InitCreateValue()
        {
            CreatedDatetime = DateTime.Now.ToUniversalTime();
        }


        public static int GetCurrentCompanyId()
        {
            return User.GetCurrentUser().CompanyId;
        }

        public static Company GetCurrentCompany(ViolaContext context = null)
        {
            return User.GetCurrentUser(context).Company;
        }

        public void InitFromViewModel(CompanyViewModel viewModel)
        {
            Name = viewModel.Name;
        }
    }
}