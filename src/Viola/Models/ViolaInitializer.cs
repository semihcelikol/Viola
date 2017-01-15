using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Viola.Models;

namespace Viola.DAL
{
    public class ViolaInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ViolaContext>
    {
        protected override void Seed(ViolaContext context)
        {
            // ilk şirket oluşturulur
            var company = new Company { Name = "Viola Varsayılan Şirket" };
            company.InitCreateValue();
            context.Companies.Add(company);
            context.SaveChanges();


            // ilk kullanıcı oluşturulur
            var manager = new UserManager<User>(new UserStore<User>(context));

            // email adresinin userName olarak kaydedilmesi için
            manager.UserValidator = new UserValidator<User>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
            };

            var adminUser = new User()
            {
                UserName = "user@viola.com",
                Email = "user@viola.com",
                EmailConfirmed = true,
                FullName = "Admin",
                CompanyId = company.Id,
                UserRole = UserRole.Admin,
                IsCompanyAdmin = true,
                LanguageId = Language.GetDefault().Id
            };
            adminUser.InitCreateValue();
            var adminresult = manager.Create(adminUser, "Admin12.");

            // admin kullanıcısına admin rolü atanır.
            if (adminresult.Succeeded)
            {
                //manager.AddToRoles(adminUser.Id, "Admin");
            }
            else
            {
                throw new Exception(adminresult.Errors.Single().ToString());
            }

            
        }
    }
}