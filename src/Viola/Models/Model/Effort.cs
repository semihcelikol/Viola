using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Globalization;
using AutoMapper;
using Viola.DAL;
using System.Linq;

namespace Viola.Models
{
    public class Effort
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



        // fk
        public virtual Company Company { get; set; }
        public virtual Task Task { get; set; }
        public virtual User User { get; set; }
        public virtual User CreatedUser { get; set; }
        public virtual User ModifiedUser { get; set; }


        // varsayýlan deðerler
        public Effort()
        {
            TransDate = DateTime.Now.ToUniversalTime();
            Billable = true;
        }

        public void InitCreateValue()
        {
            CompanyId = Company.GetCurrentCompanyId();
            CreatedDatetime = DateTime.Now.ToUniversalTime();
            CreatedUserId = User.GetCurrentUserId();
        }



        public void InitFromViewModel(EffortViewModel viewModel)
        {
            Id = viewModel.Id;
            TaskId = viewModel.TaskId;
            UserId = viewModel.UserId;
            TransDate = viewModel.TransDate;
            Description = viewModel.Description;
            Size = viewModel.Size;
            Billable = viewModel.Billable;
        }



        public static decimal SumSizeByTransDate(DateTime datetime)
        {
            var db = new ViolaContext();
            string curUserId = Viola.Models.User.GetCurrentUserId();

            var result = from e in db.Efforts
                       where e.UserId == curUserId
                         && e.TransDate == datetime
                       select e;

            return result.AsEnumerable().Sum(o => o.Size);
        }


        public static IQueryable<Effort> GetEffortsByRole()
        {
            IQueryable<Effort> ret;
            var db = new ViolaContext();
            var curUser = Viola.Models.User.GetCurrentUser();

            // bütün eforlar
            if (curUser.UserRole == UserRole.Admin)
            {
                return db.Efforts.Where(x => x.CompanyId == curUser.CompanyId).OrderByDescending(x => x.TransDate);
            }
            // user kendi girdiði eforlarý görür
            else
            {
                ret = from e in db.Efforts
                      where e.CompanyId == curUser.CompanyId
                         && e.UserId == curUser.Id
                      orderby e.TransDate descending
                      select e;
            }

            return ret;
        }
    }
}