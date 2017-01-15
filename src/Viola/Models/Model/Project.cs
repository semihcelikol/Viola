using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using AutoMapper;
using Viola.DAL;
using System.Linq;


namespace Viola.Models
{
    public class Project
    {
        [Required]
        public int Id { get; set; }

        [DisplayName("Company")]
        public int CompanyId { get; set; }

        [DisplayName("Created user")]
        public string CreatedUserId { get; set; }

        [DisplayName("Created datetime")]
        public DateTime CreatedDatetime { get; set; }

        [DisplayName("Modified user")]
        public string ModifiedUserId { get; set; }

        [DisplayName("Modified datetime")]
        public DateTime? ModifiedDatetime { get; set; }

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



        // parent relations
        public virtual Company Company { get; set; }
        public virtual User CreatedUser { get; set; }
        public virtual User ModifiedUser { get; set; }
        public virtual User ManagerUser { get; set; }

        // child relations
        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<ProjectTeam> ProjectTeams { get; set; }


        // varsayýlan deðerler
        public void InitCreateValue()
        {
            CompanyId = Company.GetCurrentCompanyId();
            CreatedDatetime = DateTime.Now.ToUniversalTime();
            CreatedUserId = User.GetCurrentUserId();
        }

        public void InitFromViewModel(ProjectViewModel viewModel)
        {
            Id = viewModel.Id;
            Name = viewModel.Name;
            Description = viewModel.Description;
            StartDate = viewModel.StartDate;
            EndDate = viewModel.EndDate;
            Status = viewModel.Status;
            ManagerUserId = viewModel.ManagerUserId;
        }


        public static IQueryable<Project> GetProjectsByRole()
        {
            IQueryable<Project> ret;
            var db = new ViolaContext();
            var curUser = Viola.Models.User.GetCurrentUser();

            // bütün projeler
            if (curUser.UserRole == UserRole.Admin)
            {
                ret = db.Projects.Where(x => x.CompanyId == curUser.CompanyId);
            }
            // user kendi oluþturduðu ve atandýðý projeleri görebilir
            else
            {
                ret = from p in db.Projects

                      // mevcut kullanýcý proje ekibinde yer alýyormu kontrol et
                      let pt = from pt in db.ProjectTeams
                               where pt.ProjectId == p.Id && pt.UserId == curUser.Id
                               select pt.UserId

                      where p.CompanyId == curUser.CompanyId
                        && (p.CreatedUserId == curUser.Id
                            || pt.Contains(curUser.Id))
                      orderby p.Name
                      select p;
            }

            return ret;
        }



    }
}
