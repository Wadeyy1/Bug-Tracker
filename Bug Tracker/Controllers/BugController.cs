﻿using Bug_Tracker.DAL;
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

            UserAdminData userDataService = new UserAdminData();
            List<UserAdminModel> Agents = userDataService.GetAllUsers();

            List<BugComments> comments = dataService.GetSpecificBugComments(BugId);
            bug.Comments = comments;
            ViewBag.Data = dataService.GetAllBugStatus();
            ViewBag.Agents = Agents;
            return View("ViewBug", bug);
        }

        [HttpPost]
        public ActionResult ViewBug(Models.Bug bu)
        {
            ViewBag.Description = Request.Form["Description"];
            ViewBag.StatusID = Request.Form["Status"];
            ViewBag.AssignedID = (Request.Form["Assigned"] == "" ? null : Request.Form["Assigned"]);
            ViewBag.Summary = Request.Form["Summary"];

            Int32.TryParse(ViewBag.StatusID, out int StatusID);

            BugData dataService = new BugData();
            dataService.SaveSpecificBug(bu.BugID, ViewBag.Summary, ViewBag.Description, StatusID, ViewBag.AssignedID);

            return ViewBug(bu.BugID);
        }

        [HttpPost]
        public ActionResult Comment(Models.Bug bu)
        {
            ViewBag.Comment = Request.Form["CommentText"];
            ViewBag.UserName = User.Identity.Name;

            BugData dataService = new BugData();
            dataService.CreateNewBugComment(ViewBag.UserName, ViewBag.Comment, bu.BugID);

            return ViewBug(bu.BugID);
        }

        public ActionResult DeleteBug(int BugID)
        {
            BugData dataService = new BugData();
            dataService.DeleteSpecificBug(BugID);

            return RedirectToAction("CurrentBugs", "Bug");
        }
    }
}