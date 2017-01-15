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
    [AuthorizeLevel(UserRole.User)]
    public class EffortsController : Controller
    {
        private ViolaContext db = new ViolaContext();
        private User curUser = Viola.Models.User.GetCurrentUser();

        // GET: Efforts
        public ActionResult Index()
        {
            return View(Effort.GetEffortsByRole().ToList());
        }

        // GET: Efforts/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(Viola.Models.User.GetUsersForEffort(), "Id", "FullName", Viola.Models.User.GetCurrentUserId());
            ViewBag.TaskId = new SelectList(Task.GetTasksByRole(), "Id", "Name");
            return View(new EffortViewModel());
        }

        // POST: Efforts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EffortViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (!EntityAuthorization.EffortCreate(viewModel))
                {
                    return HttpNotFound();
                }

                var effort = new Effort();
                effort.InitFromViewModel(viewModel);
                effort.InitCreateValue();

                db.Efforts.Add(effort);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(Viola.Models.User.GetUsersForEffort(), "Id", "FullName", viewModel.UserId);
            ViewBag.TaskId = new SelectList(Task.GetTasksByRole(), "Id", "Name", viewModel.TaskId);
            return View(viewModel);
        }

        // GET: Efforts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Effort effort = db.Efforts.Find(id);
            if (effort == null)
            {
                return HttpNotFound();
            }

            if (!EntityAuthorization.EffortEdit(effort))
            {
                return HttpNotFound();
            }


            ViewBag.UserId = new SelectList(Viola.Models.User.GetUsersForEffort(), "Id", "FullName", effort.UserId);
            ViewBag.TaskId = new SelectList(Task.GetTasksByRole(), "Id", "Name", effort.TaskId);
            return View(Mapper.Map<Effort, EffortViewModel>(effort));
        }

        // POST: Efforts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EffortViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var effort = db.Efforts.Single(x => x.Id == viewModel.Id);

                if (!EntityAuthorization.EffortEdit(effort))
                {
                    return HttpNotFound();
                }

                effort.InitFromViewModel(viewModel);
                effort.ModifiedUserId = Viola.Models.User.GetCurrentUserId();
                effort.ModifiedDatetime = DateTime.Now.ToUniversalTime();

                db.Entry(effort).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(Viola.Models.User.GetUsersForEffort(), "Id", "FullName", viewModel.UserId);
            ViewBag.TaskId = new SelectList(Task.GetTasksByRole(), "Id", "Name", viewModel.TaskId);
            return View(viewModel);
        }

        // GET: Efforts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Effort effort = db.Efforts.Find(id);
            if (effort == null)
            {
                return HttpNotFound();
            }

            if (!EntityAuthorization.EffortDelete(effort))
            {
                return HttpNotFound();
            }

            return View(effort);
        }

        // POST: Efforts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Effort effort = db.Efforts.Find(id);

            if (!EntityAuthorization.EffortDelete(effort))
            {
                return HttpNotFound();
            }

            db.Efforts.Remove(effort);
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
