using Dal;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ClientAPI
{
    public class ProductController
    {
        private static readonly HttpClient client = new HttpClient();

        public ProductController(String AccessToken)
        {
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
        }

        public async Task<List<Product>> GetProducts(string sort = "addedTime", string limit = "10")
        {
            HttpResponseMessage response = await client.GetAsync($"/api/Product?sortBy={sort}&limit={limit}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Product>>(content);
            }
            else
            {
                throw new Exception("Error");
            }
        }

        public async Task<Product> GetProduct(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"/api/Product/{id}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Product>(content);
            }
            else
            {
                throw new Exception("Error");
            }
        }

        public async Task PostProduct(Product product)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/Product", product);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }
        }

        public async Task PutProduct(int id, Product product)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync($"/api/Product/{id}", product);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }
        }

        public async Task DeleteProduct(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync($"/api/Product/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }
        }

        public async Task<List<Product>> SearchProducts(string keyword, string value)
        {
            HttpResponseMessage response = await client.GetAsync($"/api/Product/search?{keyword}={value}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Product>>(content);
            }
            else
            {
                throw new Exception("Error");
            }
        }
    }
}
