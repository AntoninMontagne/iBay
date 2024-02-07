using Dal;
using System;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace ClientAPI
{
    internal class UserController
    {
        public static async Task GetUsers()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:5001");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("/api/User");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    List<User>? users = JsonConvert.DeserializeObject<List<User>>(content);
                    foreach (User? user in users)
                    {
                        Console.WriteLine($"Student: {user.Email} {user.Pseudo} {user.Role}");
                    }
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
        }

        public static async Task GetUser(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:5001");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                                   new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync($"/api/User/{id}");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    User? user = JsonConvert.DeserializeObject<User>(content);
                    Console.WriteLine($"Student: {user.Email} {user.Pseudo} {user.Role}");
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
        }

        public static async Task PostUser(User user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:5001");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                                                      new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PostAsJsonAsync("/api/User", user);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("User créé avec succès!");
                    string content = await response.Content.ReadAsStringAsync();
                    User createdUser = JsonConvert.DeserializeObject<User>(content);
                    Console.WriteLine($"ID du User créé : {createdUser.UserId}");
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
        }

        public static async Task PutUser(int id, User user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:5001");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                                                                         new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PutAsJsonAsync($"/api/User/{id}", user);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("User modifié avec succès!");
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
        }

        public static async Task DeleteUser(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:5001");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                                                                         new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.DeleteAsync($"/api/User/{id}");
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("User supprimé avec succès!");
                }
                else
                {
                    Console.WriteLine("Error");
                }
            }
        }
    }
}
