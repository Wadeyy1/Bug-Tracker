using Bug_Tracker.Models;
using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Bug_Tracker.DAL
{
    public class BugData
    {
        public List<Bug> GetAllBugs()
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                return db.Query<Bug>
                 ("SELECT db.BugID, db.Summary, db.Description, anu.UserName as [Assigned], rbs.Description as [Status], db.CreatedDate, db.ClosedDate, rbp.PriorityName FROM Dat_Bug db INNER JOIN Ref_Bug_Status rbs ON db.StatusID = rbs.StatusID LEFT JOIN AspNetUsers anu ON db.AssignedID = anu.Id INNER JOIN Ref_Bug_Priority rbp on db.PriorityID = rbp.PriorityID").ToList();
            }
        }

        public List<Bug> GetAllUsersBugs(string UserName)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@UserName", UserName, DbType.String);
                return db.Query<Bug>
                 ("SELECT db.BugID, db.Summary, db.Description, anu.UserName as [Assigned], rbs.Description as [Status], db.CreatedDate, db.ClosedDate, rbp.PriorityName FROM Dat_Bug db INNER JOIN Ref_Bug_Status rbs ON db.StatusID = rbs.StatusID LEFT JOIN AspNetUsers anu ON db.AssignedID = anu.Id INNER JOIN Ref_Bug_Priority rbp on db.PriorityID = rbp.PriorityID WHERE db.CreatedBy = @UserName", parameter).ToList();
            }
        }

        public Bug GetSpecificBug(int BugID)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@BugID", BugID, DbType.Int32);
                return db.QueryFirstOrDefault<Bug>
                 ("SELECT db.BugID, db.Summary, db.Description, anu.UserName as [Assigned], rbs.Description as [Status], db.CreatedDate, db.ClosedDate, db.CreatedBy, rbp.PriorityName FROM Dat_Bug db INNER JOIN Ref_Bug_Status rbs ON db.StatusID = rbs.StatusID LEFT JOIN AspNetUsers anu ON db.AssignedID = anu.Id INNER JOIN Ref_Bug_Priority rbp on db.PriorityID = rbp.PriorityID WHERE db.BugID = @BugID", parameter);
            }
        }

        public List<BugComments> GetSpecificBugComments(int BugID)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@BugID", BugID, DbType.Int32);
                return db.Query<BugComments>
                 ("SELECT dbc.CommentID, anu.UserName, dbc.CommentDate, dbc.Comment FROM Dat_Bug db INNER JOIN Lnk_Bug_Comment lbc on db.BugID = lbc.BugID INNER JOIN Dat_Bug_Comment dbc on lbc.CommentID = dbc.CommentID LEFT JOIN AspNetUsers anu ON dbc.UserID = anu.Id WHERE db.BugID = @BugID ORDER BY dbc.CommentDate desc", parameter).ToList();
            }
        }

        public List<Agents> GetAllAgentUsernames()
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                return db.Query<Agents>
                    ("SELECT anu.Id,anu.UserName FROM AspNetUsers anu INNER JOIN AspNetUserRoles anur on anu.Id = anur.UserId INNER JOIN AspNetRoles anr on anur.RoleId = anr.Id").ToList();
            }
        }

        public List<Status> GetAllBugStatus()
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                return db.Query<Status>
                    ("SELECT rbs.StatusID, rbs.Description AS [StatusName] FROM Ref_Bug_Status rbs").ToList();
            }
        }

        public List<Priority> GetAllBugPriority()
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                return db.Query<Priority>
                    ("SELECT rbp.PriorityID, rbp.PriorityName FROM Ref_Bug_Priority rbp").ToList();
            }
        }

        public void CreateNewBug(string AssignedID, string Summary, string Description, string CreatedBy, int PriorityID)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@UserAssignedID", AssignedID, DbType.String);
                parameter.Add("@Summary", Summary, DbType.String);
                parameter.Add("@Description", Description, DbType.String);
                parameter.Add("@CreatedBy", CreatedBy, DbType.String);
                parameter.Add("@PriorityID", PriorityID, DbType.Int64);
                db.Execute("prc_CreateNewBug", parameter, commandType: CommandType.StoredProcedure);
            }
        }

        public void CreateNewBugComment(string UserName, string Comment, int BugID)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@UserName", UserName, DbType.String);
                parameter.Add("@Comment", Comment, DbType.String);
                parameter.Add("@BugID", BugID, DbType.Int64);
                db.Execute("prc_CreateNewBugComment", parameter, commandType: CommandType.StoredProcedure);
            }
        }

        public void SaveSpecificBug(int BugID, string Summary, string Description, int StatusID, string AssignedID, string UpdateUserName, int PriorityID)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@BugID", BugID, DbType.Int64);
                parameter.Add("@Summary", Summary, DbType.String);
                parameter.Add("@Description", Description, DbType.String);
                parameter.Add("@StatusID", StatusID, DbType.Int64);
                parameter.Add("@AssignedID", AssignedID, DbType.String);
                parameter.Add("@UpdateUserName", UpdateUserName, DbType.String);
                parameter.Add("@PriorityID", PriorityID, DbType.Int64);
                db.Execute("prc_UpdateSpecificBug", parameter, commandType: CommandType.StoredProcedure);
            }
        }

        public void DeleteSpecificBug(int BugID)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@BugID", BugID, DbType.Int64);
                db.Execute("prc_DeleteSpecificBug", parameter, commandType: CommandType.StoredProcedure);
            }
        }
    }
}