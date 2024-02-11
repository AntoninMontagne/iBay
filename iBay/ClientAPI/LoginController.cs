using Dal;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ClientAPI
{
    public class LoginController
    {
        public static async Task<string> Login(User user)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7129");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                                                      new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PostAsJsonAsync("/api/Login", user);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    JObject jsonResponse = JObject.Parse(content);

                    // Extraire la valeur du token
                    string token = (string)jsonResponse["token"]; 
                    return token;
                }
                else
                {
                    throw new Exception("Failed to login"); // Gérer les erreurs
                }
            }
        }
    }

}
