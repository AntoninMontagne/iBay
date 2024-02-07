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
    internal class CartController
    {
        public static async Task GetCarts()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:5001");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                                   new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("/api/Cart");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    List<Cart>? carts = JsonConvert.DeserializeObject<List<Cart>>(content);
                    foreach (Cart? cart in carts)
                    {
                        Console.WriteLine("Article dans le panier : ");
                        foreach (Product product in cart.Products)
                        {
                            Console.WriteLine($"Product: {product.Name} {product.Price}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
        }

        public static async Task GetCart(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:5001");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                                                                     new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync($"/api/Cart/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Cart? cart = JsonConvert.DeserializeObject<Cart>(content);
                    Console.WriteLine("Article dans le panier : ");
                    foreach (Product product in cart.Products)
                    {
                        Console.WriteLine($"Product: {product.Name} {product.Price}");
                    }
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
        }

        public static async Task PostCart(Cart cart)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:5001");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                                                                     new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PostAsJsonAsync("/api/Cart", cart);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Cart added");
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
        }

        public static async Task DeleteCart(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:5001");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                                                                                        new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.DeleteAsync($"/api/Cart/{id}");
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Cart deleted");
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
        }

        public static async Task PutCart(int id, Cart cart)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:5001");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                                                                                        new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PutAsJsonAsync($"/api/Cart/{id}", cart);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Cart updated");
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
        }
    }
}
