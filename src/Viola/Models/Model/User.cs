using System.Security.Claims;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Collections.Generic;
using System;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Viola.DAL;
using System.Linq;

namespace Viola.Models
{
    public class User : IdentityUser
    {
        [DisplayName("Company admin ?")]
        public bool IsCompanyAdmin { get; set; }

        [DisplayName("Created datetime")]
        public DateTime CreatedDatetime { get; set; }

        [DisplayName("Modified datetime")]
        public DateTime? ModifiedDatetime { get; set; }

        [Required]
        [StringLength(100)]
        [DisplayName("Full name")]
        public string FullName { get; set; }

        [DisplayName("Company")]
        public int CompanyId { get; set; }

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



        // fk
        public virtual Company Company { get; set; }


        // child relations
        public virtual ICollection<Project> CreatedProjects { get; set; }
        public virtual ICollection<Project> ModifiedProjects { get; set; }
        public virtual ICollection<Project> ManagerProjects { get; set; }

        public virtual ICollection<Effort> CreatedEfforts { get; set; }
        public virtual ICollection<Effort> ModifiedEfforts { get; set; }
        public virtual ICollection<Effort> Efforts { get; set; }

        public virtual ICollection<Task> CreatedTasks { get; set; }
        public virtual ICollection<Task> ModifiedTasks { get; set; }

        public virtual ICollection<TaskAssignedUser> TaskAssignedUsers { get; set; }
        public virtual ICollection<TaskAssignedUser> CreatedTaskAssignedUsers { get; set; }

        public virtual ICollection<ProjectTeam> ProjectTeams { get; set; }
        public virtual ICollection<ProjectTeam> CreatedProjectTeams { get; set; }




        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        // varsayılan değerler
        public User()
        {
            DateFormat = DateFormat.DMY;
            DateSeperator = DateSeperator.Dot;
            TimeZoneId = TimeZoneInfo.Local.Id;
        }

        public void InitCreateValue()
        {
            CreatedDatetime = DateTime.Now.ToUniversalTime();
        }



        public static bool IsAuthenticated()
        {
            return System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
        }

        public static string GetCurrentUserId()
        {
            return System.Web.HttpContext.Current.User.Identity.GetUserId();
        }

        public static User GetUser(string userId, ViolaContext context = null)
        {
            if (context == null)
            {
                context = new ViolaContext();
            }

            var userManager = new UserManager<User>(new UserStore<User>(context));
            return userManager.FindById(userId);
        }

        public static User GetCurrentUser(ViolaContext context = null)
        {
            return GetUser(GetCurrentUserId(), context);
        }

        // standart olarak 80px by 80px döner.
        public string GravatarUrl()
        {
            string ret = "";
            string email = this.Email.Trim().ToLower();

            email = Viola.Library.ViolaHelper.MD5Hash(email);

            ret = String.Format("https://www.gravatar.com/avatar/{0}.jpg", email);

            return ret;
        }


        public string DateFormatString()
        {
            string seperator = "";
            string format = "";

            switch (this.DateFormat)
            {
                case DateFormat.DMY:
                    format = "dd/MM/yyyy";
                    break;

                case DateFormat.MDY:
                    format = "MM/dd/yyyy";
                    break;

                case DateFormat.YMD:
                    format = "yyyy/MM/dd";
                    break;

                case DateFormat.YDM:
                    format = "yyyy/dd/MM";
                    break;
            }

            switch (this.DateSeperator)
            {
                case DateSeperator.Dot:
                    seperator = ".";
                    break;

                case DateSeperator.Dash:
                    seperator = "-";
                    break;

                case DateSeperator.Slash:
                    seperator = "'/'";
                    break;
            }

            return format.Replace("/", seperator);
        }


        public string DatepickerFormatString()
        {
            string seperator = "";
            string format = "";

            switch (this.DateFormat)
            {
                case DateFormat.DMY:
                    format = "DD/MM/YYYY";
                    break;

                case DateFormat.MDY:
                    format = "MM/DD/YYYY";
                    break;

                case DateFormat.YMD:
                    format = "YYYY/MM/DD";
                    break;

                case DateFormat.YDM:
                    format = "YYYY/DD/MM";
                    break;
            }

            switch (this.DateSeperator)
            {
                case DateSeperator.Dot:
                    seperator = ".";
                    break;

                case DateSeperator.Dash:
                    seperator = "-";
                    break;

                case DateSeperator.Slash:
                    seperator = "/";
                    break;
            }

            return format.Replace("/", seperator);
        }


        public Language Language()
        {
            return Viola.Models.Language.GetList().Single(x => x.Id == this.LanguageId);
        }


        public static IQueryable<User> GetUsers()
        {
            var db = new ViolaContext();
            var curUser = Viola.Models.User.GetCurrentUser();

            return db.Users.Where(x => x.CompanyId == curUser.CompanyId).OrderBy(x => x.FullName);
        }

        public static IQueryable<User> GetUsersForEffort()
        {
            var db = new ViolaContext();
            var curUser = Viola.Models.User.GetCurrentUser();

            if (curUser.UserRole == UserRole.Admin)
            {
                return GetUsers();
            }
            else
            {
                return GetUsers().Where(x => x.Id == curUser.Id);
            }
        }

        public static IQueryable<User> GetUsersAssignedToProject(int projectId)
        {
            var db = new ViolaContext();
            var curUser = Viola.Models.User.GetCurrentUser();

            return from pt in db.ProjectTeams
                   join u in db.Users on pt.UserId equals u.Id
                   where u.CompanyId == curUser.CompanyId
                        && pt.ProjectId == projectId
                    orderby u.FullName
                   select u;
        }
    }
}