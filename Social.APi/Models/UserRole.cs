namespace Social.APi.Models
{
    public class UserRole
    {
        public int Id { get; set; } // Primary Key

        // Foreign keys
        public int UserId { get; set; }
        public User User { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
    }

}
