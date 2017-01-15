using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Viola.Library;
using Viola.Models;

namespace Viola.Library
{
    public class EntityAuthorization
    {
        public static bool ProjectEditDelete(Project project)
        {
            bool ret = true;
            User curUser = Viola.Models.User.GetCurrentUser();

            // company kontrolü
            if (project.CompanyId != curUser.CompanyId)
            {
                ret = false;
            }

            // user sadece kendi oluşturduğu projeyi düzenleyebilir
            if (ret
                && curUser.UserRole < UserRole.Admin
                && project.CreatedUserId != curUser.Id)
            {
                ret = false;
            }

            return ret;
        }


        public static bool TaskEdit(Task task)
        {
            bool ret = true;
            User curUser = Viola.Models.User.GetCurrentUser();

            // company kontrolü
            if (task.CompanyId != curUser.CompanyId)
            {
                ret = false;
            }

            // user kendi oluşturduğu ve atandığı projelerin tasklarını düzenleyebilir
            if (ret
                && curUser.UserRole < UserRole.Admin
                && task.Project.CreatedUserId != curUser.Id
                && !ProjectTeam.Exist(task.ProjectId, curUser.Id))
            {
                ret = false;
            }

            return ret;
        }



        public static bool TaskDelete(Task task)
        {
            bool ret = true;
            User curUser = Viola.Models.User.GetCurrentUser();

            // company kontrolü
            if (task.CompanyId != curUser.CompanyId)
            {
                ret = false;
            }

            // user kendi oluşturduğu taskları silebilir
            if (ret
                && curUser.UserRole < UserRole.Admin
                && task.CreatedUserId != curUser.Id)
            {
                ret = false;
            }

            return ret;
        }


        public static bool EffortCreate(EffortViewModel viewModel)
        {
            bool ret = true;

            // kullanıcı eforunu sadece yetkisi olduğu tasklara girebilir
            if (!Task.GetTasksByRole().Where(x => x.Id == viewModel.TaskId).Any())
            {
                ret = false;
            }

            // kullanıcılar sadece yetkileri olduğu kişilerin eforlarını düzenleyebilir
            // admin: herkesi
            // user: sadece kendisini
            if (!Viola.Models.User.GetUsersForEffort().Where(x => x.Id == viewModel.UserId).Any())
            {
                ret = false;
            }

            return ret;
        }

        public static bool EffortEdit(Effort effort)
        {
            bool ret = true;
            User curUser = Viola.Models.User.GetCurrentUser();

            // company kontrolü
            if (effort.CompanyId != curUser.CompanyId)
            {
                ret = false;
            }

            // kullanıcı eforunu sadece yetkisi olduğu tasklara girebilir
            if (!Task.GetTasksByRole().Where(x => x.Id == effort.TaskId).Any())
            {
                ret = false;
            }

            // kullanıcılar sadece yetkileri olduğu kişilerin eforlarını düzenleyebilir
            // admin: herkesi
            // user: sadece kendisini
            if (!Viola.Models.User.GetUsersForEffort().Where(x => x.Id == effort.UserId).Any())
            {
                ret = false;
            }

            return ret;
        }


        public static bool EffortDelete(Effort effort)
        {
            bool ret = true;
            User curUser = Viola.Models.User.GetCurrentUser();

            // company kontrolü
            if (effort.CompanyId != curUser.CompanyId)
            {
                ret = false;
            }

            // kullanıcılar sadece yetkileri olduğu kişilerin eforlarını silebilir
            // admin: herkesi
            // user: sadece kendisini
            if (!Viola.Models.User.GetUsersForEffort().Where(x => x.Id == effort.UserId).Any())
            {
                ret = false;
            }

            return ret;
        }
    }
}