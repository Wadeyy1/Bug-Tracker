using Bug_Tracker.DAL;
using Bug_Tracker.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Bug_Tracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult UserAdmin()
        {
            UserAdminData dataService = new UserAdminData();
            List<UserAdminModel> users = dataService.GetAllUsers();

            return View("UserAdmin", users);
        }
    }
}