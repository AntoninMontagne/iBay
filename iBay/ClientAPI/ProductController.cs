using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;

using Dal;

namespace ClientAPI
{
    internal class ProductController
    {
        public static async Task GetProducts()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:5001");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                                   new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("/api/Product");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    List<Product>? products = JsonConvert.DeserializeObject<List<Product>>(content);
                    foreach (Product? product in products)
                    {
                        Console.WriteLine($"Product: {product.Name} {product.Price} {product.Available}");
                    }
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
        }

        public static async Task GetProduct(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:5001");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                                                      new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync($"/api/Product/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Product? product = JsonConvert.DeserializeObject<Product>(content);
                    Console.WriteLine($"Product: {product.Name} {product.Price} {product.Available}");
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
        }

        public static async Task PostProduct(Product product)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:5001");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                                                                         new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PostAsJsonAsync("/api/Product", product);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Product added");
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
        }

        public static async Task PutProduct(int id, Product product)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:5001");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                                                                                            new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PutAsJsonAsync($"/api/Product/{id}", product);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Product updated");
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
        }

        public static async Task DeleteProduct(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:5001");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                                                                                                              new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.DeleteAsync($"/api/Product/{id}");
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Product deleted");
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
        }
    }
}
