namespace Dal
{
    public enum UserRole
    {
        seller,
        buyer
    }
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Pseudo { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
