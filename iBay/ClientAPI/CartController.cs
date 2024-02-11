using System.Net.Http.Headers;
using System.Net.Http.Json;
using Dal;
using Newtonsoft.Json;

namespace ClientAPI
{
    public class CartController
    {
        private static readonly HttpClient client = new HttpClient();
        private string accessToken;

        public CartController(string accessToken)
        {
            this.accessToken = accessToken;
            client.BaseAddress = new Uri("https://localhost:7129");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public static async Task GetCarts()
        {
            HttpResponseMessage response = await client.GetAsync("/api/Cart");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                List<Cart> carts = JsonConvert.DeserializeObject<List<Cart>>(content);
                foreach (Cart cart in carts)
                {
                    Console.WriteLine($"Id : {cart.CartID}, Owner ID: {cart.OwnerId}");
                }
            }
            else
            {
                Console.WriteLine("Error");
            }
        }

        public static async Task<Cart> GetCart(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"/api/Cart/{id}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Cart cart = JsonConvert.DeserializeObject<Cart>(content);
                Console.WriteLine($"Id : {cart.CartID}, Owner ID: {cart.OwnerId}, List of products : ");
                Console.WriteLine();
                foreach (Product product in cart.Products)
                {
                    Console.WriteLine($"Product ID: {product.ProductId}, Product Name: {product.Name}, Product Price: {product.Price}");
                }
                Console.WriteLine();
                return cart;
            }
            else
            {
                Console.WriteLine("Error");
                return null;
            }

        }

        public static async Task PostCart(Cart cart)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/Cart", cart);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error");
            }
        }

        public static async Task DeleteCart(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync($"/api/Cart/{id}");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error");
            }
        }

        public static async Task PutCart(int id, Cart cart)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync($"/api/Cart/{id}", cart);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error");
            }
        }

        public static async Task AddProductToCart(int cartId, List<Product> products)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync($"/api/Cart/AddProducts/{cartId}", products);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(cartId);
                foreach (Product product in products)
                {
                    Console.WriteLine(product.ProductId);
                }
                Console.WriteLine("Error");
            }
        }

        public static async Task RemoveProductFromCart(int cartId, List<Product> products)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync($"/api/Cart/RemoveProducts/{cartId}", products);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error");
            }
        }

        public void UpdateAccessToken(string newAccessToken)
        {
            accessToken = newAccessToken;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newAccessToken);
        }
    }
}
