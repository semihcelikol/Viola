using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Viola.DAL;
using Viola.Models;

namespace Viola.Controllers
{
    [AuthorizeUser(UserRole.Admin)]
    public class CompaniesController : Controller
    {
        private ViolaContext db = new ViolaContext();

        // GET: Companies/Edit
        public ActionResult Index()
        {
            Company company = Company.GetCurrentCompany();
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(Mapper.Map<Company, CompanyViewModel>(company));
        }

        // POST: Companies/Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(CompanyViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var company = Company.GetCurrentCompany(db);


                company.InitFromViewModel(viewModel);
                company.ModifiedDatetime = DateTime.Now.ToUniversalTime();

                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
            }
            return View(viewModel);
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
