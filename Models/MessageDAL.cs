using ChatNet.Utility;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ChatNet.Models
{
    public class MessageDAL
    {
        string dbcs = ConfigurationHelper.GetConnectionString("DefaultConnection");

        public List<Message> GetMessages(string conversationid, DateTime mtimestamp)
        {
            List<Message> messages = new List<Message>();

            using(SqlConnection conn = new SqlConnection(dbcs))
            {
                SqlCommand cmd = new SqlCommand("spFetchMessages", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@conversationid", conversationid);
                cmd.Parameters.AddWithValue("@mtimestamp", mtimestamp);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Message message = new Message();
                    message.MessageID = reader["messageid"].ToString();
                    message.Sender = reader["sender"].ToString();
                    message.Receiver = reader["receiver"].ToString();
                    message.Msg = reader["usermessage"].ToString();
                    message.mTimestamp = Convert.ToDateTime(reader["mtimestamp"]);

                    messages.Add(message);
                }
            }

            return messages;
        }

        public List<Message> GetNewMessages(string conversationid, DateTime mtimestamp)
        {
            List<Message> newMessages = new List<Message>();

            using (SqlConnection conn = new SqlConnection(dbcs))
            {
                SqlCommand cmd = new SqlCommand("spFetchNewMessages", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@conversationid", conversationid);
                cmd.Parameters.AddWithValue("@mtimestamp", mtimestamp);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Message message = new Message();
                    message.MessageID = reader["messageid"].ToString();
                    message.Sender = reader["sender"].ToString();
                    message.Receiver = reader["receiver"].ToString();
                    message.Msg = reader["usermessage"].ToString();
                    message.mTimestamp = Convert.ToDateTime(reader["mtimestamp"]);

                    newMessages.Add(message);
                }
            }

            return newMessages;
        }

        public bool AddMessage(Message m)
        {
            using(SqlConnection conn = new SqlConnection(dbcs))
            {
                SqlCommand cmd = new SqlCommand("spAddMessage", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@messageid", m.MessageID);
                cmd.Parameters.AddWithValue("@conversationid", m.ConversationID);
                cmd.Parameters.AddWithValue("@sender", m.Sender);
                cmd.Parameters.AddWithValue("@receiver", m.Receiver);
                cmd.Parameters.AddWithValue("@usermessage", m.Msg);
                cmd.Parameters.AddWithValue("@mtimestamp", m.mTimestamp);

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

        public int CheckMessages(string conversationid, DateTime mtimestamp)
        {
            int newMessages = 0;

            using(SqlConnection conn = new SqlConnection(dbcs))
            {
                SqlCommand cmd = new SqlCommand("spCheckMessages", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@conversationid", conversationid);
                cmd.Parameters.AddWithValue("@mtimestamp", mtimestamp);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if(reader.Read())
                {
                    newMessages = Convert.ToInt32(reader["newmessages"]);
                }
            }

            return newMessages;
        }
    }
}
