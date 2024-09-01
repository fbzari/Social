namespace Social.APi.Models
{
    public class Friendship
    {
        public int Id { get; set; } // Primary Key

        // Foreign keys
        public int UserId1 { get; set; }
        public User User1 { get; set; }

        public int UserId2 { get; set; }
        public User User2 { get; set; }

        public DateTime EstablishedAt { get; set; } // Date the friendship was established
    }

}
