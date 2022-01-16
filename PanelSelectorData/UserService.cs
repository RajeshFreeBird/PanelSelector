using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace PanelSelectorData
{
    public class UserService
    {
        private readonly string _connectionString;
        public UserService(string connectionString)
        {
            _connectionString = connectionString;
        }
        public DataTable ValidateUserAndGetDetails(string username, string passwordHash)
        {
            try
            {
                var sqlConnector = new SqlConnector(_connectionString);
                var cmd = new SqlCommand("validateLogin");
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@passwordhash", passwordHash);
                return sqlConnector.GetSqlResult(cmd);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           
        }

        public int UpdatePassword(string loginName, string oldPassword, string newPassword)
        {
            try
            {
                var sqlConnector = new SqlConnector(_connectionString);
                var sqlCommand = new SqlCommand("Dbo.UpdatePassword");
                //var cmd = new SqlCommand("UpdatePassword");
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@username", loginName);
                sqlCommand.Parameters.AddWithValue("@oldpasswordHash", oldPassword);
                sqlCommand.Parameters.AddWithValue("@newpasswordHash", newPassword);
                return sqlConnector.ExecuteNonQuery(sqlCommand);
            }
            catch (Exception ex)
            {
               
                throw ex;
            }
        }
        public DataTable GetAllUsers()
        {
            try
            {
                var sqlConnector = new SqlConnector(_connectionString);
                var cmd = new SqlCommand("GetAllUsers");
                return sqlConnector.GetSqlResult(cmd);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public int CreateUser(string loginName, string passwordHash, string displayName, string emailId, bool isAdmin, Guid createdBy)
        {
            try
            {
                var sqlConnector = new SqlConnector(_connectionString);
                var sqlCommand = new SqlCommand("Dbo.usp_CreateNewUser");
                //var cmd = new SqlCommand("UpdatePassword");
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Loginname", loginName);
                sqlCommand.Parameters.AddWithValue("@PasswordHash", passwordHash);
                sqlCommand.Parameters.AddWithValue("@DisplayName", displayName);
                sqlCommand.Parameters.AddWithValue("@EmailId", emailId);
                sqlCommand.Parameters.AddWithValue("@IsAdmin", isAdmin);
                sqlCommand.Parameters.AddWithValue("@CreatedBy", createdBy);
                return sqlConnector.ExecuteNonQuery(sqlCommand);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int UpdateUser(Guid Id, string loginName, string displayName, string emailId, bool isAdmin, bool IsActive, Guid updatedBy)
        {
            try
            {
                var sqlConnector = new SqlConnector(_connectionString);
                var sqlCommand = new SqlCommand("Dbo.usp_UpdateUser");
                //var cmd = new SqlCommand("UpdatePassword");
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Loginname", loginName);
                sqlCommand.Parameters.AddWithValue("@Id", Id);
                sqlCommand.Parameters.AddWithValue("@DisplayName", displayName);
                sqlCommand.Parameters.AddWithValue("@EmailId", emailId);
                sqlCommand.Parameters.AddWithValue("@IsAdmin", isAdmin);
                sqlCommand.Parameters.AddWithValue("@IsActive", IsActive);
                sqlCommand.Parameters.AddWithValue("@UpdatedBy", updatedBy);
                return sqlConnector.ExecuteNonQuery(sqlCommand);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public DataTable GetUserDetails(Guid id)
        {
            try
            {
                var sqlConnector = new SqlConnector(_connectionString);
                var cmd = new SqlCommand("usp_GetUserDetails");
                cmd.Parameters.AddWithValue("@Id", id);
                return sqlConnector.GetSqlResult(cmd);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public int DeleteUser(Guid id)
        {
            try
            {
                var sqlConnector = new SqlConnector(_connectionString);
                var sqlCommand = new SqlCommand("Dbo.usp_DeleteUser");
                //var cmd = new SqlCommand("UpdatePassword");
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@id", id);
                return sqlConnector.ExecuteNonQuery(sqlCommand);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
