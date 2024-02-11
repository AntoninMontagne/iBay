using Dal;

namespace ClientAPI.Interact
{
    public class LoginInteract
    {
        public LoginInteract() { }

        public static async Task<string> InteractWithLoginRoute()
        {
            Console.WriteLine("Enter user details:");
            Console.Write("Email: ");
            string email = Console.ReadLine();
            Console.Write("Pseudo: ");
            string pseudo = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();
            Console.Write("Role (seller or buyer): ");
            string roleStr = Console.ReadLine();
            UserRole role;
            if (Enum.TryParse<UserRole>(roleStr, out role))
            {
                User newUser = new User
                {
                    Email = email,
                    Pseudo = pseudo,
                    Password = password,
                    Role = role
                };
                return await LoginController.Login(newUser);
            }
            else
            {
                Console.WriteLine("Invalid role. Please enter either 'seller' or 'buyer'.");
                return null;
            }
        }
    }
}
