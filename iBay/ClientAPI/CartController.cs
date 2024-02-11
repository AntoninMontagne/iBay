using System.Net.Http.Headers;
using System.Net.Http.Json;
using Dal;
using Newtonsoft.Json;

namespace ClientAPI
{
    public class CartController
    {
        private static readonly HttpClient client = new HttpClient();

        public CartController(string AccessToken)
        {
            client.BaseAddress = new Uri("https://localhost:7129");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
        }

        public static async Task<List<Cart>> GetCarts()
        {
            HttpResponseMessage response = await client.GetAsync("/api/Cart");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Cart>>(content);
            }
            else
            {
                throw new Exception("Error");
            }
        }

        public static async Task<Cart> GetCart(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"/api/Cart/{id}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Cart>(content);
            }
            else
            {
                throw new Exception("Error");
            }
        }

        public static async Task PostCart(Cart cart)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/Cart", cart);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }
        }

        public static async Task DeleteCart(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync($"/api/Cart/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }
        }

        public static async Task PutCart(int id, Cart cart)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync($"/api/Cart/{id}", cart);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }
        }

        public static async Task AddProductToCart(int cartId, List<Product> products)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync($"/api/Cart/{cartId}/AddProduct/{cartId}", products);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }
        }

        public static async Task RemoveProductFromCart(int cartId, List<Product> products)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync($"/api/Cart/RemoveProducts/{cartId}", products);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }
        }
    }
}
