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
    public class ProjectsController : Controller
    {
        private ViolaContext db = new ViolaContext();
        private User curUser = Viola.Models.User.GetCurrentUser();

        // GET: Projects
        public ActionResult Index()
        {
            return View(Project.GetProjectsByRole().ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            // company ve yetki kontrolü
            if (!Project.GetProjectsByRole().Where(x => x.Id == id).Any())
            {
                return HttpNotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        [AuthorizeLevel(UserRole.User)]
        public ActionResult Create()
        {
            ViewBag.ManagerUserId = new SelectList(Viola.Models.User.GetUsers(), "Id", "FullName");
            ViewBag.UserIdMulti = new MultiSelectList(Viola.Models.User.GetUsers(), "Id", "FullName");
            return View(new ProjectViewModel());
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AuthorizeLevel(UserRole.User)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProjectViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var project = new Project();
                project.InitFromViewModel(viewModel);
                project.InitCreateValue();

                db.Projects.Add(project);
                db.SaveChanges();
                ProjectTeam.Create(project.Id, Request.Form.GetValues("UserIdMulti"));
                ProjectTeam.AddProjectManagerToTeam(project);

                return RedirectToAction("Details", "Projects", new { id = project.Id });
            }

            ViewBag.ManagerUserId = new SelectList(Viola.Models.User.GetUsers(), "Id", "FullName", viewModel.ManagerUserId);
            ViewBag.UserIdMulti = new MultiSelectList(Viola.Models.User.GetUsers(), "Id", "FullName");
            return View(viewModel);
        }

        // GET: Projects/Edit/5
        [AuthorizeLevel(UserRole.User)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            if (!EntityAuthorization.ProjectEditDelete(project))
            {
                return HttpNotFound();
            }

            ViewBag.ManagerUserId = new SelectList(Viola.Models.User.GetUsers(), "Id", "FullName", project.ManagerUserId);
            ViewBag.UserIdMulti = new MultiSelectList(Viola.Models.User.GetUsers(), "Id", "FullName", ProjectTeam.UserIdSelection(project.Id));
            return View(Mapper.Map<Project, ProjectViewModel>(project));
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AuthorizeLevel(UserRole.User)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProjectViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var project = db.Projects.Single(x => x.Id == viewModel.Id);

                if (!EntityAuthorization.ProjectEditDelete(project))
                {
                    return HttpNotFound();
                }

                project.InitFromViewModel(viewModel);
                project.ModifiedUserId = Viola.Models.User.GetCurrentUserId();
                project.ModifiedDatetime = DateTime.Now.ToUniversalTime();

                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                ProjectTeam.Create(project.Id, Request.Form.GetValues("UserIdMulti"));
                ProjectTeam.AddProjectManagerToTeam(project);

                return RedirectToAction("Details", "Projects", new { id = project.Id });
            }
            ViewBag.ManagerUserId = new SelectList(Viola.Models.User.GetUsers(), "Id", "FullName", viewModel.ManagerUserId);
            ViewBag.UserIdMulti = new MultiSelectList(Viola.Models.User.GetUsers(), "Id", "FullName", ProjectTeam.UserIdSelection(viewModel.Id));
            return View(viewModel);
        }

        // GET: Projects/Delete/5
        [AuthorizeLevel(UserRole.User)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            if (!EntityAuthorization.ProjectEditDelete(project))
            {
                return HttpNotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [AuthorizeLevel(UserRole.User)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);

            if (!EntityAuthorization.ProjectEditDelete(project))
            {
                return HttpNotFound();
            }

            db.Projects.Remove(project);
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
