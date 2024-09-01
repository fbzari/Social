namespace Social.APi.Models
{
    public class FriendRequest
    {
        public int Id { get; set; } // Primary Key

        // Foreign keys
        public int SenderId { get; set; }
        public User Sender { get; set; }

        public int ReceiverId { get; set; }
        public User Receiver { get; set; }

        public string Status { get; set; } // E.g., "Pending", "Accepted", "Rejected"
        public DateTime SentAt { get; set; } // Date the request was sent
    }

}
