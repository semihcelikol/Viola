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
    public class Task
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

        [DisplayName("Created datetime")]
        public DateTime? ModifiedDatetime { get; set; }

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


        // fk
        public virtual Company Company { get; set; }
        public virtual Project Project { get; set; }
        public virtual User CreatedUser { get; set; }
        public virtual User ModifiedUser { get; set; }

        // child relations
        public virtual ICollection<Effort> Efforts { get; set; }
        public virtual ICollection<TaskAssignedUser> TaskAssingedUsers { get; set; }
        //scelikol cost modules
        public virtual ICollection<CostDetail> CostDetails { get; set; }
        //



        // varsayýlan deðerler
        public Task()
        {
            Priority = TaskPriority.Normal;
        }


        public void InitCreateValue()
        {
            CompanyId = Company.GetCurrentCompanyId();
            CreatedDatetime = DateTime.Now.ToUniversalTime();
            CreatedUserId = User.GetCurrentUserId();
        }


        public void InitFromViewModel(TaskViewModel viewModel)
        {
            Id = viewModel.Id;
            Name = viewModel.Name;
            Description = viewModel.Description;
            ProjectId = viewModel.ProjectId;
            StartDate = viewModel.StartDate;
            DueDate = viewModel.DueDate;
            EstimatedEffortSize = viewModel.EstimatedEffortSize;
            Priority = viewModel.Priority;
            Status = viewModel.Status;
            ExternalLink = viewModel.ExternalLink;
            ExternalRef = viewModel.ExternalRef;
        }

        public static IQueryable<Task> GetTasksByRole()
        {
            IQueryable<Task> ret;
            var db = new ViolaContext();
            var curUser = Viola.Models.User.GetCurrentUser();

            // bütün tasklar
            if (curUser.UserRole == UserRole.Admin)
            {
                ret = db.Tasks.Where(x => x.CompanyId == curUser.CompanyId);
            }
            // user kendi oluþturduðu ve atandýðý projelere ait tasklarý görebilir
            else
            {
                ret = from p in db.Projects
                      join t in db.Tasks on p.Id equals t.ProjectId

                      // mevcut kullanýcý proje ekibinde yer alýyormu kontrol et
                      let pt = from pt in db.ProjectTeams
                               where pt.ProjectId == p.Id && pt.UserId == curUser.Id
                               select pt.UserId

                      where p.CompanyId == curUser.CompanyId
                        && (p.CreatedUserId == curUser.Id
                            || pt.Contains(curUser.Id))
                      orderby t.Id descending
                      select t;
            }

            return ret;
        }


        public static Task Find(int taskId)
        {
            var db = new ViolaContext();
            return db.Tasks.Where(x => x.Id == taskId).FirstOrDefault();
        }


        public static IQueryable<Task> GetTasksByProjectId(int projectId)
        {
            var db = new ViolaContext();
            var curUser = Viola.Models.User.GetCurrentUser();

            return from t in db.Tasks
                   where t.CompanyId == curUser.CompanyId
                      && t.ProjectId == projectId
                   orderby t.Name
                   select t;
        }
    }
}