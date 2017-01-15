using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Viola.Models;

namespace Viola
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // AutoMapper
            AutoMapperWebConfiguration.Configure();
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            try
            {
                // culture ne olursa olsun bazı ayarları kullanıcının parametrelerinden alması sağlandı.
                if (Viola.Models.User.IsAuthenticated())
                {
                    var curUser = Viola.Models.User.GetCurrentUser();
                    //CultureInfo newCulture = Thread.CurrentThread.CurrentCulture.Clone() as CultureInfo;

                    // dil
                    var newCulture = new CultureInfo(curUser.LanguageId);

                    // tarih formatları
                    newCulture.DateTimeFormat.ShortDatePattern = curUser.DateFormatString();
                    newCulture.DateTimeFormat.ShortTimePattern = "HH:mm";
                    newCulture.DateTimeFormat.LongDatePattern = curUser.DateFormatString();
                    newCulture.DateTimeFormat.LongTimePattern = "HH:mm";

                    // decimal tipindeki alanların ondalık ayıracı
                    newCulture.NumberFormat.NumberDecimalSeparator = ".";

                    Thread.CurrentThread.CurrentCulture = newCulture;

                    Thread.CurrentThread.CurrentUICulture = newCulture;
                }
            }
            catch
            {

            }
        }
    }
}
