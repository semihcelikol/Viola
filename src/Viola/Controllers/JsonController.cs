using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Viola.DAL;
using Viola.Models;

namespace Viola.Controllers
{
    public class JsonController : Controller
    {
        private ViolaContext db = new ViolaContext();
        private User curUser = Viola.Models.User.GetCurrentUser();


        public JsonResult GetUsersAssignedToProject(int projectId)
        {
            var users = Viola.Models.User.GetUsersAssignedToProject(projectId);

            return Json(
                users.Select(x => new {
                    id = x.Id,
                    text = x.FullName
                }), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTasksByProjectId(int projectId)
        {
            var tasks = Viola.Models.Task.GetTasksByProjectId(projectId);

            return Json(
                tasks.Select(x => new {
                    id = x.Id,
                    text = x.Name
            }), JsonRequestBehavior.AllowGet);
        }
    }
}