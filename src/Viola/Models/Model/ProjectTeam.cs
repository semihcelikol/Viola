using System;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Viola.DAL;
using System.Collections.Generic;

namespace Viola.Models
{
    public class ProjectTeam
    {
        [Key]
        [Required]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DisplayName("Project")]
        public int ProjectId { get; set; }

        [Key]
        [Required]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DisplayName("User")]
        public string UserId { get; set; }

        [DisplayName("Company")]
        public int CompanyId { get; set; }

        [DisplayName("Created user")]
        public string CreatedUserId { get; set; }

        [DisplayName("Created datetime")]
        public DateTime CreatedDatetime { get; set; }

        // child relations
        public virtual Project Project { get; set; }
        public virtual User User { get; set; }
        public virtual User CreatedUser { get; set; }
        public virtual Company Company { get; set; }


        // varsayýlan deðerler
        public void InitCreateValue()
        {
            CompanyId = Company.GetCurrentCompanyId();
            CreatedDatetime = DateTime.Now.ToUniversalTime();
            CreatedUserId = User.GetCurrentUserId();
        }


        public static void Create(int projectId, string[] selection)
        {
            using (var db = new ViolaContext())
            {
                // mevcut iliþkiler silinir.
                foreach (var row in db.ProjectTeams.Where(x => x.ProjectId == projectId))
                {
                    db.ProjectTeams.Remove(row);
                }

                // yeni iliþkiler kaydedilir
                if (selection != null)
                {
                    foreach (var id in selection)
                    {
                        var row = new ProjectTeam
                        {
                            ProjectId = projectId,
                            UserId = id
                        };
                        row.InitCreateValue();

                        db.ProjectTeams.Add(row);
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<string> UserIdSelection(int projectId)
        {
            var ret = new List<string>();

            using (var db = new ViolaContext())
            {
                var rows = db.ProjectTeams.Where(x => x.ProjectId == projectId);
                foreach (var row in rows)
                {
                    ret.Add(row.UserId);
                }
            }

            return ret;
        }


        public static bool Exist(int projectId, string userId)
        {
            var db = new ViolaContext();
            return db.ProjectTeams.Where(x => x.ProjectId == projectId && x.UserId == userId).Any();
        }

        public static void AddProjectManagerToTeam(Project project)
        {
            var db = new ViolaContext();

            if (!ProjectTeam.Exist(project.Id, project.ManagerUserId))
            {
                var pt = new ProjectTeam()
                {
                    ProjectId = project.Id,
                    UserId = project.ManagerUserId
                };

                pt.InitCreateValue();

                db.ProjectTeams.Add(pt);
                db.SaveChanges();
            }
        }
    }
}
