using System.Net.Http.Headers;
using System.Net.Http.Json;
using Dal;
using Newtonsoft.Json;

namespace ClientAPI
{
    public class UserController
    {
        private static readonly HttpClient client = new HttpClient();
        private string accessToken;

        public UserController(string accessToken)
        {
            this.accessToken = accessToken;
            client.BaseAddress = new Uri("https://localhost:7129");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        }

        public async Task GetUser()
        {
            HttpResponseMessage response = await client.GetAsync("/api/User");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                List<User> users = JsonConvert.DeserializeObject<List<User>>(content);
                foreach (User user in users)
                {
                    Console.WriteLine($"Id : {user.UserId}, Name : {user.Pseudo}, Mail : {user.Email}, Role : {user.Role}");
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Error");
            }
        }

        public async Task GetUserById(int id)
        {
            HttpResponseMessage response = await client.GetAsync($"/api/User/{id}");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                User user =  JsonConvert.DeserializeObject<User>(content);
                Console.WriteLine($"Id : {user.UserId}, Name : {user.Pseudo}, Mail : {user.Email}, Role : {user.Role}");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Error");
            }
        }

        public async Task PostUser(User user)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/User", user);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error");
            }
        }

        public async Task DeleteUser(int id)
        {
            HttpResponseMessage response = await client.DeleteAsync($"/api/User/{id}");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("Error");
            }
        }

        public async Task PutUser(int id, User user)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync($"/api/User/{id}", user);
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
