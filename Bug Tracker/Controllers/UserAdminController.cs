using Bug_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bug_Tracker.Controllers
{
    public class UserAdminController : Controller
    {
        public ActionResult ViewUser(string UserId)
        {
            ViewUser model = new ViewUser
            {
                ID = UserId
            };
            return View(model);
        }
    }
}