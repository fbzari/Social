using Social.APi.Helpers;

namespace Social.APi.Models
{
    public class FriendRequest
    {
        public int Id { get; set; } // Primary Key

        // Foreign keys
        public string SenderId { get; set; }
        public User Sender { get; set; }

        public string ReceiverId { get; set; }
        public User Receiver { get; set; }

        public string Status { get; set; } // E.g., "Pending", "Accepted", "Rejected"
        public DateTime SentAt { get; set; } // Date the request was sent
    }

}
