using Bug_Tracker.Models;
using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Bug_Tracker.DAL
{
    public class UserAdminData
    {
        public List<UserAdminModel> GetAllUsers()
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                return db.Query<UserAdminModel>
                 ("SELECT anu.ID, anu.UserName, anu.Email, anr.Name AS [Role] FROM AspNetUsers anu INNER JOIN AspNetUserRoles anur on anu.Id = anur.UserId INNER JOIN AspNetRoles anr on anur.RoleId = anr.Id").ToList();
            }
        }
    }
}