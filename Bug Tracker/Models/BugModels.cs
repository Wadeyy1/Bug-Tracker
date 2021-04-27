using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bug_Tracker.Models
{
    public class Bug
    {
        public int BugID { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Assigned { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public List<BugComments> Comments { get; set; }

        public Bug()
        {
        }
    }

    public class BugComments
    {
        public int CommentID { get; set; }
        public string UserID { get; set; }
        public DateTime CommentDate { get; set; }
        public string Comment { get; set; }
    }

    public class Agents
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public Agents()
        {
        }
    }

    public class Status
    {
        public int StatusID { get; set; }
        public string StatusName { get; set; }

        public Status()
        {
        }
    }
}