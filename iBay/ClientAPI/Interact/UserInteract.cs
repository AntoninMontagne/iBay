using Dal;
using System;
using System.Threading.Tasks;

namespace ClientAPI.Interact
{
    public class UserInteract
    {
        private static UserController userController;

        public UserInteract(string token)
        {
            userController = new UserController(token);
        }

        public async Task InteractWithUserRoute()
        {
            Console.WriteLine("Actions for User route:");
            Console.WriteLine("1. Get all users");
            Console.WriteLine("2. Get user by ID");
            Console.WriteLine("3. Create new user");
            Console.WriteLine("4. Update existing user");
            Console.WriteLine("5. Delete user");
            Console.WriteLine("6. Back");

            Console.Write("Enter your choice: ");
            string actionChoice = Console.ReadLine();
            Console.WriteLine();

            switch (actionChoice)
            {
                case "1":
                    await userController.GetUser();
                    break;
                case "2":
                    Console.Write("Enter user ID: ");
                    int userId = int.Parse(Console.ReadLine());
                    await userController.GetUserById(userId);
                    break;
                case "3":
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
                        await userController.PostUser(newUser);
                    }
                    else
                    {
                        Console.WriteLine("Invalid role. Please enter either 'seller' or 'buyer'.");
                    }
                    break;
                case "4":
                    Console.Write("Enter user ID: ");
                    int updatedUserId = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter updated user details:");
                    Console.Write("Email: ");
                    string updatedEmail = Console.ReadLine();
                    Console.Write("Pseudo: ");
                    string updatedPseudo = Console.ReadLine();
                    Console.Write("Password: ");
                    string updatedPassword = Console.ReadLine();
                    Console.Write("Role (seller or buyer): ");
                    string updatedRoleStr = Console.ReadLine();
                    UserRole updatedRole;
                    if (Enum.TryParse<UserRole>(updatedRoleStr, out updatedRole))
                    {
                        User updatedUser = new User
                        {
                            UserId = updatedUserId,
                            Email = updatedEmail,
                            Pseudo = updatedPseudo,
                            Password = updatedPassword,
                            Role = updatedRole
                        };
                        await userController.PutUser(updatedUserId, updatedUser);
                    }
                    else
                    {
                        Console.WriteLine("Invalid role. Please enter either 'seller' or 'buyer'.");
                    }
                    break;
                case "5":
                    Console.Write("Enter user ID: ");
                    int deletedUserId = int.Parse(Console.ReadLine());
                    await userController.DeleteUser(deletedUserId);
                    break;
                case "6":
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter a valid option.");
                    break;
            }
        }

        public void UpdateAccessToken(string newAccessToken)
        {
            userController.UpdateAccessToken(newAccessToken);
        }
    }
}
