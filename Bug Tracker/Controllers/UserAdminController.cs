using Bug_Tracker.DAL;
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
            UserAdminData dataService = new UserAdminData();
            ViewUser UserData = dataService.GetSpecificUser(UserId);

            return View(UserData);
        }

        [HttpPost]
        public ActionResult UserForm(Models.ViewUser vu)
        {
            ViewBag.ID = Request.Form["ID"];
            ViewBag.Email = Request.Form["InputEmail"];
            ViewBag.AdditionalInfo = Request.Form["AdditionalInfo"];
            ViewBag.Role = (Request.Form["Role"] == "User") ? 1 : 2;
            ViewBag.CrudPermission = (Request.Form["CrudPermission"] == "on") ? 1 : 0;

            UserAdminData dataService = new UserAdminData();
            dataService.SaveSpecificUser(vu.ID, ViewBag.Email, ViewBag.AdditionalInfo, ViewBag.Role, ViewBag.CrudPermission);

            return RedirectToAction("Index", "Home");
        }
    }
}