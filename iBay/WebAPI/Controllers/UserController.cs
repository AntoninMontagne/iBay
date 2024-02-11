using Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly AppDBContext _context;

        public UserController(AppDBContext context)
        {
            _context = context;
        }

        // GET: api/User
        /// <summary>Get users</summary>
        [HttpGet]
        public ActionResult<List<User>> GetUser()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }

        // GET: api/User/{id}
        /// <summary>Get one user details</summary>
        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var user = _context.Users.FirstOrDefault(u => u.UserId == id);

            if (user == null)
            {
                return NotFound(); // Retourne un statut 404 si le user n'est pas trouvé
            }

            return Ok(user); // Retourne un statut 200 avec le user si elle est trouvé
        }

        // POST: api/User
        /// <summary>Create user</summary>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult<User> PostUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok(new { Message = "user created successfully", User = user });

        }

        /// <summary>Update user</summary>
        // PUT: api/User/{id}
        [HttpPut("{id}")]
        public IActionResult PutUser(int id, User updatedUser)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var existingUser = _context.Users.FirstOrDefault(u => u.UserId == id);

            if (existingUser == null)
            {
                return NotFound();
            }

            _context.Entry(existingUser).CurrentValues.SetValues(updatedUser);

            _context.SaveChanges();

            return Ok("user modified successfully");  // Retourne un statut 204 si le user est modifié
        }

        /// <summary>Delete user</summary>
        // DELETE: api/User/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {

            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var user = _context.Users.FirstOrDefault(u => u.UserId == id);

            if (user == null)
            {
                return NotFound(); // Retourne un statut 404 si le user n'est pas trouvé
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return Ok("user deleted successfully");  // Retourne un statut 204 si le user est supprimé
        }
    }
}