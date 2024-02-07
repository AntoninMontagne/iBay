﻿using Dal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppContext _context;

        public UserController(AppContext context)
        {
            _context = context;
        }

        // GET: api/Person
        /// <summary>Get users</summary>
        [HttpGet]
        public ActionResult<List<User>> GetUser()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }

        // GET: api/Person/{id}
        /// <summary>Get one user details</summary>
        [HttpGet("{id}")]
        public ActionResult<User> GetUserById(int id)
        {
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
        public ActionResult<User> PostUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
        }

        /// <summary>Update user</summary>
        // PUT: api/User/{id}
        [HttpPut("{id}")]
        public IActionResult PutUser(int id, User updatedUser)
        {
            if (id != updatedUser.UserId)
            {
                return BadRequest();
            }

            var existingUser = _context.Users.FirstOrDefault(u => u.UserId == id);

            if (existingUser == null)
            {
                return NotFound();
            }

            _context.Entry(existingUser).CurrentValues.SetValues(updatedUser);

            _context.SaveChanges();

            return NoContent(); // Retourne un statut 204 si le user est modifié
        }

        /// <summary>Delete user</summary>
        // DELETE: api/User/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);

            if (user == null)
            {
                return NotFound(); // Retourne un statut 404 si le user n'est pas trouvé
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return NoContent(); // Retourne un statut 204 si le user est supprimé
        }
    }
}
