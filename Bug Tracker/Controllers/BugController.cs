using Bug_Tracker.DAL;
using Bug_Tracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bug_Tracker.Controllers
{
    public class BugController : Controller
    {
        [Authorize(Roles = "User, Admin")]
        public ActionResult CurrentBugs()
        {
            BugData dataService = new BugData();

            List<Bug> bugs = dataService.GetAllBugs();
            ViewBag.Data = dataService.GetAllAgentUsernames();
            return View("CurrentBugs", bugs);
        }

        [HttpPost]
        public ActionResult BugCreateForm(Models.Bug bu)
        {
            ViewBag.Summary = Request.Form["Summary"];
            ViewBag.Description = Request.Form["Description"];
            ViewBag.AssignedID = Request.Form["userAssigned"];

            BugData dataService = new BugData();
            dataService.CreateNewBug(ViewBag.AssignedID, ViewBag.Summary, ViewBag.Description);

            return RedirectToAction("CurrentBugs", "Bug");
        }

        public ActionResult ViewBug(int BugId)
        {
            BugData dataService = new BugData();
            Bug bug = dataService.GetSpecificBug(BugId);
            List<BugComments> comments = dataService.GetSpecificBugComments(BugId);
            bug.Comments = comments;
            ViewBag.Data = dataService.GetAllBugStatus();
            return View("ViewBug", bug);
        }
    }
}