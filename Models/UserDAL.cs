using ChatNet.Utility;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ChatNet.Models
{
    public class UserDAL
    {
        string dbcs = ConnectionString.Dbcs;

        public User GetUser(User u)
        {
            User user = null;

            using(SqlConnection conn = new SqlConnection(dbcs))
            {
                SqlCommand cmd = new SqlCommand("spFetchUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", u.UserId);
                cmd.Parameters.AddWithValue("@userpassword", u.Password);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if(reader.Read())
                {
                    user = new User();
                    user.UserId = reader["userid"].ToString();
                    user.Name = reader["username"].ToString();
                    user.Email = reader["useremail"].ToString();
                }
            }

            return user;
        }

        public User CheckUserID(string userid)
        {
            User user = null;

            using (SqlConnection conn = new SqlConnection(dbcs))
            {
                SqlCommand cmd = new SqlCommand("spCheckUserId", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", userid);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if(reader.Read())
                {
                    user = new User();
                    user.UserId = reader["userid"].ToString();
                    user.Name = reader["username"].ToString();
                    user.Email = reader["useremail"].ToString();
                }
            }

            return user;
        }

        public User CheckUserEmail(string useremail)
        {
            User user = null;

            using (SqlConnection conn = new SqlConnection(dbcs))
            {
                SqlCommand cmd = new SqlCommand("spCheckUserEmail", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@useremail", useremail);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if(reader.Read())
                {
                    user = new User();
                    user.UserId = reader["userid"].ToString();
                    user.Name = reader["username"].ToString();
                    user.Email = reader["useremail"].ToString();
                }
            }

            return user;
        }

        public bool AddUser(User user)
        {
            using(SqlConnection conn = new SqlConnection(dbcs))
            {
                SqlCommand cmd = new SqlCommand("spAddUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", user.UserId);
                cmd.Parameters.AddWithValue("@username", user.Name);
                cmd.Parameters.AddWithValue("@useremail", user.Email);
                cmd.Parameters.AddWithValue("@userpassword", user.Password);

                conn.Open();
                int res = cmd.ExecuteNonQuery();

                if(res > 0)
                {
                    return true;
                } else
                {
                    return false;
                }
            }
        }

        public bool UpdateUser(User user)
        {
            using(SqlConnection conn = new SqlConnection(dbcs))
            {
                SqlCommand cmd = new SqlCommand("spUpdateUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", user.UserId);
                cmd.Parameters.AddWithValue("@username", user.Name);
                cmd.Parameters.AddWithValue("@useremail", user.Email);
                cmd.Parameters.AddWithValue("@userpassword", user.Password);

                conn.Open();
                int res = cmd.ExecuteNonQuery();

                if (res > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool DeleteUser(string userid)
        {
            using (SqlConnection conn = new SqlConnection(dbcs))
            {
                SqlCommand cmd = new SqlCommand("spDeleteUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", conn);

                conn.Open();
                int res = cmd.ExecuteNonQuery();

                if (res > 0)
                {
                    return true;
                } else
                {
                    return false;
                }
            }
        }
    }
}
