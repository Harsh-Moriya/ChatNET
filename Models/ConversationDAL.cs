using ChatNet.Utility;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ChatNet.Models
{
    public class ConversationDAL
    {
        string dbcs = ConnectionString.Dbcs;

        public List<Conversation> GetConversations(string userid)
        {
            List<Conversation> conversations = new List<Conversation>();

            using(SqlConnection conn = new SqlConnection(dbcs))
            {
                SqlCommand cmd = new SqlCommand("spFetchConversations", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@userid", userid);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    Conversation conversation = new Conversation();
                    conversation.ConversationID = reader["conversationid"].ToString();
                    conversation.UserOne = reader["userone"].ToString();
                    conversation.UserOneName = reader["useronename"].ToString();
                    conversation.UserTwo = reader["usertwo"].ToString();
                    conversation.UserTwoName = reader["usertwoname"].ToString();

                    conversations.Add(conversation);
                }
            }

            return conversations;
        }

        public bool CreateConversation(string conversationid, User userone, User usertwo, DateTime lmtimestamp)
        {
            using (SqlConnection conn = new SqlConnection(dbcs))
            {
                SqlCommand cmd = new SqlCommand("spCreateConversation", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@conversationid", conversationid);
                cmd.Parameters.AddWithValue("@userone", userone.UserId);
                cmd.Parameters.AddWithValue("@useronename", userone.Name);
                cmd.Parameters.AddWithValue("@usertwo", usertwo.UserId);
                cmd.Parameters.AddWithValue("@usertwoname", usertwo.Name);
                cmd.Parameters.AddWithValue("@lmtimestamp", lmtimestamp);

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
