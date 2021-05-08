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
                 ("SELECT anu.ID, anu.UserName, anu.Email, anr.Name AS [Role] FROM AspNetUsers anu LEFT JOIN AspNetUserRoles anur on anu.Id = anur.UserId LEFT JOIN AspNetRoles anr on anur.RoleId = anr.Id WHERE UserName <> 'System'").ToList();
            }
        }

        public ViewUser GetSpecificUser(string ID)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@ID", ID, DbType.String);
                return db.QueryFirstOrDefault<ViewUser>("SELECT anu.ID, anu.UserName, anu.Email, ISNULL(anr.Name,'') AS [Role], ISNULL(lup.PermissionGranted,'') AS [PermissionGranted], ISNULL(anu.AdditionalInfo,'') FROM AspNetUsers anu LEFT JOIN AspNetUserRoles anur on anu.Id = anur.UserId LEFT JOIN AspNetRoles anr on anur.RoleId = anr.Id LEFT JOIN Lnk_User_Permissions lup on anu.Id = lup.Id LEFT JOIN Ref_User_Permissions rup on lup.PermissionID = rup.PermissionID AND rup.PermissionName = 'CRUD - Bug' WHERE anu.ID = @ID", parameter);
            }
        }

        public string GetSpecificUserASPRole(string UserName)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@UserName", UserName, DbType.String);
                return db.QueryFirstOrDefault<string>("SELECT ISNULL(anr.Name,'') FROM AspNetUsers anu LEFT JOIN AspNetUserRoles anur on anu.Id = anur.UserId LEFT JOIN AspNetRoles anr on anur.RoleId = anr.Id WHERE anu.UserName = @UserName", parameter);
            }
        }

        public void SaveSpecificUser(string ID, string Email, string AdditionalInfo, int? Role, int CrudPermission)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@ID", ID, DbType.String);
                parameter.Add("@Email", Email, DbType.String);
                parameter.Add("@AdditionalInfo", AdditionalInfo, DbType.String);
                parameter.Add("@Role", Role, DbType.Int32);
                parameter.Add("@CrudPermission", CrudPermission, DbType.Int32);
                db.Execute("prc_UpdateSpecificUser", parameter, commandType: CommandType.StoredProcedure);
            }
        }

        public void DeleteSpecificUser(string ID)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@UserID", ID, DbType.String);
                db.Execute("prc_DeleteSpecificUser", parameter, commandType: CommandType.StoredProcedure);
            }
        }
    }
}