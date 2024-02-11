using Dal;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace ClientAPI
{
    public class ProductController
    {
        private static readonly HttpClient client = new HttpClient();
        private string accessToken;

        public ProductController(String accessToken)
        {
            this.accessToken = accessToken;
            client.BaseAddress = new Uri("https://localhost:7129");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public async Task GetProducts(string sort = "addedTime", string limit = "10")
        {
            HttpResponseMessage response = await client.GetAsync($"/api/Product?sortBy={sort}&limit={limit}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                List<Product> products = JsonConvert.DeserializeObject<List<Product>>(content);
                foreach (Product product in products)
                {
                    Console.WriteLine($"Id : {product.ProductId}, Product name : {product.Name}, Price : {product.Price}, Product owner : {product.OwnerId}");
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Error");
            }
        }

        public async Task GetProduct(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"/api/Product/{id}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Product product = JsonConvert.DeserializeObject<Product>(content);
                Console.WriteLine($"Id : {product.ProductId}, Product name : {product.Name}, Price : {product.Price}, Product owner : {product.OwnerId}");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Error");
            }
        }

        public async Task PostProduct(Product product)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/Product", product);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error");
            }
        }

        public async Task PutProduct(int id, Product product)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync($"/api/Product/{id}", product);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error");
            }
        }

        public async Task DeleteProduct(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync($"/api/Product/{id}");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error");
            }
        }

        public async Task SearchProducts(string keyword, string value)
        {
            HttpResponseMessage response = await client.GetAsync($"/api/Product/search?{keyword}={value}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                List<Product> products = JsonConvert.DeserializeObject<List<Product>>(content);
                foreach (Product product in products)
                {
                    Console.WriteLine($"Nom du produit : {product.Name}, Prix du produit : {product.Price}, Propriétaire du produit : {product.OwnerId}");
                }
                Console.WriteLine();
            }
            else
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
