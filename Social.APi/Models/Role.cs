namespace Social.APi.Models
{
    public class Role
    {
        public int Id { get; set; } // Primary Key
        public string Name { get; set; } // E.g., "Admin", "User"

        // Navigation properties
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }

}
