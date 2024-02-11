using Dal;

namespace ClientAPI.Interact
{
    public class ProductInteract
    {
        private static ProductController productController;
        public ProductInteract(String token) {
            productController = new ProductController(token);
        }
        public async Task InteractWithProductRoute()
        {
            Console.WriteLine("Actions for Product route:");
            Console.WriteLine("1. Get all products");
            Console.WriteLine("2. Get product by ID");
            Console.WriteLine("3. Create new product");
            Console.WriteLine("4. Update existing product");
            Console.WriteLine("5. Delete product");
            Console.WriteLine("6. Back");

            Console.Write("Enter your choice: ");
            string actionChoice = Console.ReadLine();

            switch (actionChoice)
            {
                case "1":
                    await GetProducts();
                    break;
                case "2":
                    await GetProduct();
                    break;
                case "3":
                    await CreateProduct("create");
                    break;
                case "4":
                    await UpdateProduct();
                    break;
                case "5":
                    await DeleteProduct();
                    break;
                case "6":
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please enter a valid option.");
                    break;
            }
        }

        public static async Task GetProducts()
        {
            await productController.GetProducts();
        }

        public static async Task GetProduct()
        {
            Console.Write("Enter product ID: ");
            int productId = int.Parse(Console.ReadLine());
            await productController.GetProduct(productId);
        }

        public static async Task<Product> CreateProduct(string action)
        {
            Console.WriteLine("Enter product details:");
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Price: ");
            decimal price = decimal.Parse(Console.ReadLine());
            Console.Write("Available (true or false): ");
            bool available = bool.Parse(Console.ReadLine());
            Console.Write("Owner ID: ");
            int ownerId = int.Parse(Console.ReadLine());
            Product newProduct = new Product
            {
                Name = name,
                Price = price,
                Available = available,
                AddedTime = DateTime.Now,
                OwnerId = ownerId,
                Image = ""
            };
            if (action == "create")
            {
                await productController.PostProduct(newProduct);
                return newProduct;
            }
            else
                return newProduct;
        }

        public static async Task UpdateProduct()
        {
            Console.Write("Enter product ID: ");
            int updatedProductId = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter updated product details:");
            Console.Write("Name: ");
            string updatedName = Console.ReadLine();
            Console.Write("Price: ");
            decimal updatedPrice = decimal.Parse(Console.ReadLine());
            Console.Write("Available (true or false): ");
            bool updatedAvailable = bool.Parse(Console.ReadLine());
            Console.Write("Owner ID: ");
            int ownerId = int.Parse(Console.ReadLine());

            Product updatedProduct = new Product
            {
                ProductId = updatedProductId,
                Name = updatedName,
                Price = updatedPrice,
                Available = updatedAvailable,
                AddedTime = DateTime.Now,
                OwnerId = ownerId,
                Image = ""
            };
            await productController.PutProduct(updatedProductId, updatedProduct);
        }

        public static async Task DeleteProduct()
        {
            Console.Write("Enter product ID: ");
            int deletedProductId = int.Parse(Console.ReadLine());
            await productController.DeleteProduct(deletedProductId);
        }



    }
}
