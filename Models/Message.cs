namespace ChatNet.Models
{
    public class Message
    {
        public string MessageID { get; set; }
        public string ConversationID { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Msg { get; set; }
        public DateTime mTimestamp { get; set; }
    }
}
