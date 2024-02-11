using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Dal;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase

    {
        private readonly AppDBContext _context;

        public LoginController(AppDBContext context)
        {
            _context = context;
        }

        private string CreateJWT(User user)
        {
            var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("396B5DD9-CC75-411C-9311-5B6E1F391B89")); // NOTE: USE THE SAME KEY AS USED IN THE PROGRAM.CS OR STARTUP.CS FILE
            var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);

            var claims = new[] // NOTE: could also use List<Claim> here
			{
                new Claim(ClaimTypes.Name, user.Email), // NOTE: this will be "User.Identity.Name" value; this could also specify the email address of the user as many sites use for the user name
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()), // Ajouter l'ID de l'utilisateur
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(issuer: "localhost", audience: "localhost", claims: claims, expires: DateTime.Now.AddMinutes(60), signingCredentials: credentials); // NOTE: USE THE REAL DOMAIN NAME
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User Authenticate(User login)
        {
            // Requête pour trouver l'utilisateur dans la base de données
            var user = _context.Users.FirstOrDefault(u => u.Email == login.Email && u.Password == login.Password);

            // Si l'utilisateur est trouvé, le retourner
            if (user != null)
                return user;

            return null;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User login)
        {
            return await Task.Run(() =>
            {
                IActionResult response = Unauthorized();

                User user = Authenticate(login);

                if (user != null)
                    response = Ok(new { token = CreateJWT(user) });

                return response;
            });
        }
    }
}
