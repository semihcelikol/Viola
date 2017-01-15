using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Viola.DAL;
using Viola.Library;
using Viola.Models;

namespace Viola.Controllers
{
    [Authorize]
    public class TasksController : Controller
    {
        private ViolaContext db = new ViolaContext();
        private User curUser = Viola.Models.User.GetCurrentUser();

        // GET: Tasks
        public ActionResult Index()
        {
            return View(Task.GetTasksByRole().ToList());
        }

        // GET: Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }

            // company ve yetki kontrolü
            if (!Task.GetTasksByRole().Where(x => x.Id == id).Any())
            {
                return HttpNotFound();
            }

            return View(task);
        }


        // GET: Tasks/Create
        [AuthorizeLevel(UserRole.User)]
        public ActionResult Create()
        {
            ViewBag.ProjectId = new SelectList(Project.GetProjectsByRole(), "Id", "Name");
            ViewBag.UserIdMulti = Enumerable.Empty<SelectListItem>();
            return View(new TaskViewModel());
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AuthorizeLevel(UserRole.User)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaskViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // kullanıcı sadece yetkisi olan projeler arasından seçim yapabilir
                if (!Project.GetProjectsByRole().Where(x => x.Id == viewModel.ProjectId).Any())
                {
                    return HttpNotFound();
                }

                var task = new Task();
                task.InitFromViewModel(viewModel);
                task.InitCreateValue();

                db.Tasks.Add(task);
                db.SaveChanges();
                TaskAssignedUser.Create(task.Id, Request.Form.GetValues("UserIdMulti"));

                return RedirectToAction("Details", "Tasks", new { id = task.Id });
            }

            ViewBag.ProjectId = new SelectList(Project.GetProjectsByRole(), "Id", "Name", viewModel.ProjectId);
            ViewBag.UserIdMulti = new MultiSelectList(Viola.Models.User.GetUsers(), "Id", "FullName");
            return View(viewModel);
        }

        // GET: Tasks/Edit/5
        [AuthorizeLevel(UserRole.User)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }

            if (!EntityAuthorization.TaskEdit(task))
            {
                return HttpNotFound();
            }

            ViewBag.ProjectId = new SelectList(Project.GetProjectsByRole(), "Id", "Name", task.ProjectId);
            ViewBag.UserIdMulti = new MultiSelectList(Viola.Models.User.GetUsersAssignedToProject(task.ProjectId), "Id", "FullName", TaskAssignedUser.UserIdSelection(task.Id));
            return View(Mapper.Map<Task, TaskViewModel>(task));
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AuthorizeLevel(UserRole.User)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TaskViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // kullanıcı sadece yetkisi olan projeler arasından seçim yapabilir
                if (!Project.GetProjectsByRole().Where(x => x.Id == viewModel.ProjectId).Any())
                {
                    return HttpNotFound();
                }

                var task = db.Tasks.Single(x => x.Id == viewModel.Id);

                if (!EntityAuthorization.TaskEdit(task))
                {
                    return HttpNotFound();
                }

                task.InitFromViewModel(viewModel);
                task.ModifiedUserId = Viola.Models.User.GetCurrentUserId();
                task.ModifiedDatetime = DateTime.Now.ToUniversalTime();

                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                TaskAssignedUser.Create(task.Id, Request.Form.GetValues("UserIdMulti"));

                return RedirectToAction("Details", "Tasks", new { id = task.Id });
            }

            ViewBag.ProjectId = new SelectList(Project.GetProjectsByRole(), "Id", "Name", viewModel.ProjectId);
            ViewBag.UserIdMulti = new MultiSelectList(Viola.Models.User.GetUsersAssignedToProject(viewModel.ProjectId), "Id", "FullName", TaskAssignedUser.UserIdSelection(viewModel.Id));
            return View(viewModel);
        }

        // GET: Tasks/Delete/5
        [AuthorizeLevel(UserRole.User)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }

            if (!EntityAuthorization.TaskDelete(task))
            {
                return HttpNotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [AuthorizeLevel(UserRole.User)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Task task = db.Tasks.Find(id);

            if (!EntityAuthorization.TaskDelete(task))
            {
                return HttpNotFound();
            }

            db.Tasks.Remove(task);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
