using System.Net.Http.Headers;
using static ClientAPI.ProductController;
using static ClientAPI.CartController;
using static ClientAPI.UserController;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace ClientAPI
{
    public class Program
    {


        static async Task CreateEntities()
        {
            Console.WriteLine("Voulez-vous créer un student ? (y or n)");
            string entityType = Console.ReadLine()?.ToLowerInvariant();

            switch (entityType)
            {
                case "y":
                    await CreateStudent();
                    break;

                case "n":
                    Console.WriteLine("Fermeture de l'application...");
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Choix invalide. Veuillez choisir parmi students, teachers ou classrooms.");
                    break;
            }
        }

        static async Task<bool> Show()
        {
            Console.WriteLine("Que voulez-vous afficher : Users, Produits ou Paniers ?");
            string? choice = Console.ReadLine();
            if (choice == "User")
            {
                await GetUsers();
                return true;
            }
            else if (choice == "Produits")
            {
                await GetProducts();
                return true;
            }
            else if (choice == "Paniers")
            {
                await GetCarts();
                return true;
            }
            else if (choice == "Rien")
            {
                return false;
            }
            else
            {
                Console.WriteLine("Erreur");
                return true;
            }
        }


        public static async Task Main(string[] args)
        {
            bool show = true;
            while (show)
            {
                show = await Show();
            }
            Console.WriteLine("Voulez-vous créer une entitée :");


            await CreateEntities();

        }
    }
}
