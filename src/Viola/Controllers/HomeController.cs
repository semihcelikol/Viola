using System;
using System.Web.Mvc;
using Viola.DAL;
using System.Linq;
using System.Data.Entity;

namespace Viola.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ViolaContext db = new ViolaContext();

        public ActionResult Index()
        {
            string curUserId = Viola.Models.User.GetCurrentUserId();

            var todayEfforts = db.Efforts.Where(x => x.UserId == curUserId
                                                && DbFunctions.TruncateTime(x.TransDate) == DbFunctions.TruncateTime(DateTime.Now));

            return View(todayEfforts);
        }

        public string Test()
        {
            var dt = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            var dtend = DateTime.Now.EndOfWeek(DayOfWeek.Monday);

            return "";
        }
    }
}