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
        public ActionResult ViewUser()
        { //, u.ID string ID
            //ViewBag.Message = ID;
            return View();
        }
    }
}