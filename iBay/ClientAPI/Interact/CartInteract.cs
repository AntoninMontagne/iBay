using Dal;

namespace ClientAPI.Interact
{
    public class CartInteract
    {
        private static CartController cartController;
        public CartInteract(string token) {
            cartController = new CartController(token);
        }

        public async Task InteractWithCartRoute()
        {
            Console.WriteLine("Actions for Cart route:");
            Console.WriteLine("1. Get all carts");
            Console.WriteLine("2. Get cart by ID");
            Console.WriteLine("3. Create new cart");
            Console.WriteLine("4. Update existing cart");
            Console.WriteLine("5. Delete cart");
            Console.WriteLine("6. Add products to cart");
            Console.WriteLine("7. Remove products from cart");
            Console.WriteLine("8. Pay cart");
            Console.WriteLine("9. Back");

            Console.Write("Enter your choice: ");
            string actionChoice = Console.ReadLine();
            Console.WriteLine();

            switch (actionChoice)
            {
                case "1":
                    await GetCarts();
                    break;
                case "2":
                    await GetCart();
                    break;
                case "3":
                    await CreatCart();
                    break;
                case "4":
                    await UpdateCart();
                    break;
                case "5":
                    await DeleteCart();
                    break;
                case "6":
                    await AddProductToCart();
                    break;
                case "7":
                   await RemoveProductsFromCart();
                    break;
                case "8":
                    await PayCart();
                    break;
                case "9":
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter a valid option.");
                    break;
            }
        }
        
        public static async Task GetCarts()
        {
            await CartController.GetCarts();
        }

        public static async Task GetCart()
        {
            Console.Write("Enter cart ID: ");
            int cartId = int.Parse(Console.ReadLine());
            await CartController.GetCart(cartId);
        }

        public static async Task CreatCart()
        {
            Console.WriteLine("Creating a new cart:");
            Cart newCart = new Cart();
            Console.Write("Enter OwnerId: ");
            newCart.OwnerId = int.Parse(Console.ReadLine());
            bool newCartCreated = true;
            List<Product> products = new List<Product>();
            while (newCartCreated)
            {
                Console.WriteLine("Do you want to add products to the cart? (yes or no)");
                string addProductChoice = Console.ReadLine();
                if (addProductChoice == "yes")
                {
                    Product product = await ProductInteract.CreateProduct("ask");
                    products.Add(product);
                }
                else if (addProductChoice == "no")
                {
                    newCartCreated = false;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please enter a valid option.");
                }
            }
            newCart.Products = products;
            await CartController.PostCart(newCart);
        }

        public static async Task UpdateCart()
        {
            Console.Write("Enter cart ID: ");
            int cartId = int.Parse(Console.ReadLine());
            Cart cart = await CartController.GetCart(cartId);
            Console.WriteLine("Enter updated cart details:");
            Console.Write("OwnerId: ");
            cart.OwnerId = int.Parse(Console.ReadLine());
            bool newCartCreated = true;
            List<Product> products = new List<Product>();
            while (newCartCreated)
            {
                Console.WriteLine("Do you want to add products to the cart? (yes or no)");
                string addProductChoice = Console.ReadLine();
                if (addProductChoice == "yes")
                {
                    Product product = await ProductInteract.CreateProduct("ask");
                    products.Add(product);
                }
                else if (addProductChoice == "no")
                {
                    newCartCreated = false;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please enter a valid option.");
                }
            }
            cart.Products = products;
            await CartController.PutCart(cartId, cart);
        }

        public static async Task AddProductToCart()
        {
            Console.Write("Enter cart ID: ");
            int cartId = int.Parse(Console.ReadLine());
            bool newCartCreated = true;
            List<Product> products = new List<Product>();
            while (newCartCreated)
            {
                Console.WriteLine("Do you want to add products to the cart? (yes or no)");
                string addProductChoice = Console.ReadLine();
                if (addProductChoice == "yes")
                {
                    Product product = await ProductInteract.CreateProduct("ask");
                    products.Add(product);
                }
                else if (addProductChoice == "no")
                {
                    newCartCreated = false;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please enter a valid option.");
                }
            }
            await CartController.AddProductToCart(cartId, products);
        }

        public static async Task RemoveProductsFromCart()
        {
            Console.Write("Enter cart ID: ");
            int cartId = int.Parse(Console.ReadLine());
            bool newCartCreated = true;
            List<Product> products = new List<Product>();
            while (newCartCreated)
            {
                Console.WriteLine("Do you want to remove products to the cart? (yes or no)");
                string addProductChoice = Console.ReadLine();
                if (addProductChoice == "yes")
                {
                    Product product = await ProductInteract.CreateProduct("ask");
                    products.Add(product);
                }
                else if (addProductChoice == "no")
                {
                    newCartCreated = false;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please enter a valid option.");
                }
            }
            await CartController.RemoveProductFromCart(cartId, products);
        }

        public static async Task DeleteCart()
        {
            Console.Write("Enter cart ID to delete: ");
            int deletedCartId = int.Parse(Console.ReadLine());
            await CartController.DeleteCart(deletedCartId);
        }

        public static async Task PayCart()
        {
            Console.Write("Enter cart ID to pay: ");
            int cartId = int.Parse(Console.ReadLine());
            await CartController.PayCart(cartId);
        }

        public void UpdateAccessToken(string newAccessToken)
        {
            cartController.UpdateAccessToken(newAccessToken);
        }




    }
}
