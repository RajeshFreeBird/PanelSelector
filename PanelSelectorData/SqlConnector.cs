using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PanelSelectorData
{
    public class SqlConnector
    {
        private string _connectionString;
        public SqlConnector(string connectionString)
        {
            _connectionString = connectionString;
        }
        public DataSet GetData()
        {
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(""))
            {
                SqlDataAdapter sda = new SqlDataAdapter("Select * from usermaster", _connectionString);
                sda.Fill(ds);
            }
            return ds;
        }

        public int ExecuteNonQuery(SqlCommand cmd)
        {
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    cmd.Connection = con;
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
       

        public int ExecuteNonQuery(SqlCommand cmd, Dictionary<string, string> parameters)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }                   
                    var affectedRows = cmd.ExecuteNonQuery();
                    return affectedRows;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable GetSqlResult(string sqlStatement)
        {
            DataTable dt = null;
            try
            {
               
                using (var conn = new SqlConnection(_connectionString))
                {
                    dt = new DataTable();
                    conn.Open();
                    var cmd = new SqlCommand(sqlStatement, conn);
                    var adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            

        }
        public DataTable GetSqlResult(SqlCommand sqlCommand)
        {
            DataTable dt = null;
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    dt = new DataTable();
                    conn.Open();
                    sqlCommand.Connection = conn;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    //var cmd = new SqlCommand()
                    var adapter = new SqlDataAdapter(sqlCommand);
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public DataTable GetSqlResult(SqlCommand sqlCommand, SqlParameterCollection parameters)
        {
            try
            {
                DataTable dt = null;
                using (var conn = new SqlConnection(_connectionString))
                {
                    dt = new DataTable();
                    conn.Open();
                    sqlCommand.Connection = conn;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    foreach (var param in parameters)
                    {
                        sqlCommand.Parameters.Add(param);
                    }
                    var adapter = new SqlDataAdapter(sqlCommand);
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

           
        }
    }
}
