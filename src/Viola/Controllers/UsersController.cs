using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Viola.Models;
using Viola.DAL;
using Microsoft.AspNet.Identity.Owin;
using AutoMapper;

namespace Viola.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        public UserManager<User> UserManager { get; private set; }
        public RoleManager<IdentityRole> RoleManager { get; private set; }
        public ViolaContext context { get; private set; }

        
        public UsersController()
        {
            context = new ViolaContext();
            UserManager = new UserManager<User>(new UserStore<User>(context));
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // email adresinin userName olarak kaydedilmesi için
            UserManager.UserValidator = new UserValidator<User>(UserManager)
            {
                AllowOnlyAlphanumericUserNames = false
            };
        }

        public UsersController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }


        //
        // GET: /Users/
        [AuthorizeLevel(UserRole.User)]
        public async Task<ActionResult> Index()
        {
            return View(await Viola.Models.User.GetUsers().ToListAsync());
        }

        //
        // GET: /Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            if (user.CompanyId != Company.GetCurrentCompanyId())
            {
                return HttpNotFound();
            }

            return View(user);
        }

        //
        // GET: /Users/Create
        [AuthorizeUser(UserRole.Admin)]
        public ActionResult Create()
        {
            ViewBag.TimeZoneId = new SelectList(TimeZoneInfo.GetSystemTimeZones(), "Id", "DisplayName", TimeZoneInfo.Local.Id);
            ViewBag.LanguageId = new SelectList(Language.GetList(), "Id", "DisplayName", Language.GetDefault().Id);
            return View();
        }

        //
        // POST: /Users/Create
        [HttpPost]
        [AuthorizeUser(UserRole.Admin)]
        public async Task<ActionResult> Create(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // view model'den istediğimiz alanlar manuel map edilir.
                var user = new User
                {
                    CompanyId = Company.GetCurrentCompanyId(),
                    FullName = viewModel.FullName,
                    UserName = viewModel.Email,
                    Email = viewModel.Email,
                    UserRole = viewModel.UserRole,
                    DateFormat = viewModel.DateFormat,
                    DateSeperator = viewModel.DateSeperator,
                    TimeZoneId = viewModel.TimeZoneId,
                    LanguageId = viewModel.LanguageId
                };
                user.InitCreateValue();

                var result = await UserManager.CreateAsync(user, viewModel.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Details", "Users", new { id = user.Id });
                }

                AddErrors(result);
            }

            ViewBag.TimeZoneId = new SelectList(TimeZoneInfo.GetSystemTimeZones(), "Id", "DisplayName", viewModel.TimeZoneId);
            ViewBag.LanguageId = new SelectList(Language.GetList(), "Id", "DisplayName", viewModel.LanguageId);
            return View(viewModel);
        }

        
        //
        // GET: /Users/Edit/1
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            if (user.CompanyId != Company.GetCurrentCompanyId())
            {
                return HttpNotFound();
            }

            // yetki kontrolleri
            var curUser = Viola.Models.User.GetCurrentUser();

            // company admin'i sadece kendisi güncelleyebilir
            if (!curUser.IsCompanyAdmin
                && user.IsCompanyAdmin)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // login olan kullanıcı admin den alt seviyede ise sadece kendisini güncelleyebilir
            if (curUser.UserRole < UserRole.Admin
                && curUser.Id != user.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // kullanıcı kendisini güncellerken kendi yetkisini değiştiremez, kendisini kilitleyemez.
            ViewBag.ShowAdvFields = curUser.Id != user.Id;

            ViewBag.TimeZoneId = new SelectList(TimeZoneInfo.GetSystemTimeZones(), "Id", "DisplayName", user.TimeZoneId);
            ViewBag.LanguageId = new SelectList(Language.GetList(), "Id", "DisplayName", user.LanguageId);
            return View(Mapper.Map<User, EditViewModel>(user));
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditViewModel viewModel)
        {
            if (viewModel.Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await UserManager.FindByIdAsync(viewModel.Id);
            if (user == null)
            {
                return HttpNotFound();
            }

            if (user.CompanyId != Company.GetCurrentCompanyId())
            {
                return HttpNotFound();
            }

            // yetki kontrolleri
            var curUser = Viola.Models.User.GetCurrentUser();

            // company admin'i sadece kendisi güncelleyebilir
            if (!curUser.IsCompanyAdmin
                && user.IsCompanyAdmin)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // login olan kullanıcı admin den alt seviyede ise sadece kendisini güncelleyebilir
            if (curUser.UserRole < UserRole.Admin
                && curUser.Id != user.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // kullanıcı kendisini güncellerken kendi yetkisini değiştiremez, kendisini kilitleyemez.
            ViewBag.ShowAdvFields = curUser.Id != user.Id;


            if (ModelState.IsValid)
            {
                user.ModifiedDatetime = DateTime.Now.ToUniversalTime();
                user.FullName = viewModel.FullName;
                user.UserName = viewModel.Email;
                user.Email = viewModel.Email;
                user.DateFormat = viewModel.DateFormat;
                user.DateSeperator = viewModel.DateSeperator;
                user.TimeZoneId = viewModel.TimeZoneId;
                user.LanguageId = viewModel.LanguageId;

                
                if (ViewBag.ShowAdvFields)
                {
                    user.UserRole = viewModel.UserRole;

                    if (viewModel.IsLockout)
                    {
                        user.LockoutEnabled = true;
                        user.LockoutEndDateUtc = DateTime.MaxValue;
                    }
                    else
                    {
                        user.LockoutEnabled = false;
                        user.LockoutEndDateUtc = null;
                    }
                }

                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Details", "Users", new { id = user.Id });
                }

                AddErrors(result);
            }
            
            ViewBag.TimeZoneId = new SelectList(TimeZoneInfo.GetSystemTimeZones(), "Id", "DisplayName", user.TimeZoneId);
            ViewBag.LanguageId = new SelectList(Language.GetList(), "Id", "DisplayName", user.LanguageId);
            return View();
        }


        [AuthorizeLevel(UserRole.Admin)]
        public async Task<ActionResult> ChangePassword(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            if (user.CompanyId != Company.GetCurrentCompanyId())
            {
                return HttpNotFound();
            }

            ViewBag.Id = id;
            return View();
        }

        [AuthorizeLevel(UserRole.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(SetPasswordViewModel model, string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            if (user.CompanyId != Company.GetCurrentCompanyId())
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                var resultRemove = await UserManager.RemovePasswordAsync(user.Id);
                if (resultRemove.Succeeded)
                {
                    var result = await UserManager.AddPasswordAsync(user.Id, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    AddErrors(result);
                }
                AddErrors(resultRemove);
            }

            ViewBag.Id = id;
            return View();
        }
    }
}