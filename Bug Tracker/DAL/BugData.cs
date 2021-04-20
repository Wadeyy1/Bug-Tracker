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
                 ("SELECT db.BugID, db.Summary, db.Description, anu.UserName as [Assigned], rbs.Description as [Status], db.CreatedDate, db.ClosedDate FROM Dat_Bug db INNER JOIN Ref_Bug_Status rbs ON db.StatusID = rbs.StatusID LEFT JOIN AspNetUsers anu ON db.AssignedID = anu.Id").ToList();
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

        public void CreateNewBug(string AssignedID, string Summary, string Description)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@UserAssignedID", AssignedID, DbType.String);
                parameter.Add("@Summary", Summary, DbType.String);
                parameter.Add("@Description", Description, DbType.String);
                db.Execute("prc_CreateNewBug", parameter, commandType: CommandType.StoredProcedure);
            }
        }
    }
}