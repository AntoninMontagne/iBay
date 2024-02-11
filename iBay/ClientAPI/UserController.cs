using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Dal;
using Newtonsoft.Json;

namespace ClientAPI
{
    public class UserController
    {
        private static readonly HttpClient client = new HttpClient();

        public UserController(string AccessToken)
        {
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);

        }

        public async Task<List<User>> GetUser()
        {
            HttpResponseMessage response = await client.GetAsync("/api/User");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<User>>(content);
            }
            else
            {
                throw new Exception("Error");
            }
        }

        public async Task<User> GetUserById(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"/api/User/{id}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(content);
            }
            else
            {
                throw new Exception("Error");
            }
        }

        public async Task PostUser(User user)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/User", user);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }
        }

        public async Task DeleteUser(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync($"/api/User/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }
        }

        public async Task PutUser(int id, User user)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync($"/api/User/{id}", user);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error");
            }
        }
    }
}
