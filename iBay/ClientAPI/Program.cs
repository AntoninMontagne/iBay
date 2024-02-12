using static ClientAPI.Interact.LoginInteract;
using ClientAPI.Interact;

namespace ClientAPI
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            bool exit = false;
            string token = null;

            ProductInteract productInteract = new ProductInteract(token);
            CartInteract cartInteract = new CartInteract(token);
            UserInteract userInteract = new UserInteract(token);

            while (!exit)
            {
                Console.WriteLine("Choose route to interact with:");
                Console.WriteLine("1. Product");
                Console.WriteLine("2. Cart");
                Console.WriteLine("3. User");
                Console.WriteLine("4. Login");
                Console.WriteLine("5. Exit");

                Console.Write("Enter your choice: ");
                string routeChoice = Console.ReadLine();
                Console.WriteLine();

                switch (routeChoice)
                {
                    case "1":
                        await productInteract.InteractWithProductRoute();
                        break;
                    case "2":
                        if (token == null)
                        {
                            Console.WriteLine("Please login first.");
                            break;
                        }
                        await cartInteract.InteractWithCartRoute();
                        break;
                    case "3":
                        await userInteract.InteractWithUserRoute();
                        break;
                    case "4":
                        token = await InteractWithLoginRoute();
                        productInteract.UpdateAccessToken(token);
                        cartInteract.UpdateAccessToken(token);
                        userInteract.UpdateAccessToken(token);
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a valid option.");
                        break;
                }
            }
        }
    }
}
