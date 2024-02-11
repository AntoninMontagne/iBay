using Dal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly AppDBContext _context;

        public ProductController(AppDBContext context)
        {
            _context = context;
        }

        // GET: api/Product
        /// <summary>Get products</summary>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult<List<Product>> GetProduct()
        {
            var products = _context.Products.ToList();
            return Ok(products);
        }

        // GET: api/Product/{id}
        /// <summary>Get one product details</summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<Product> GetProductById(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound(); // Retourne un statut 404 si le product n'est pas trouvé
            }

            return Ok(product); // Retourne un statut 200 avec le product si elle est trouvé
        }

        // POST: api/Product
        /// <summary>Create product</summary>
        [HttpPost]
        public ActionResult<Product> PostProduct(Product product)
        {
            if (User.FindFirst(ClaimTypes.Role).Value != "seller")
            {
                return Unauthorized();
            }

            _context.Products.Add(product);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
        }

        /// <summary>Update product</summary>
        // PUT: api/Product/{id}
        [HttpPut("{id}")]
        public IActionResult PutProduct(int id, Product updatedProduct)
        {
            if (!(User.FindFirst(ClaimTypes.Role)?.Value == "seller" && updatedProduct.OwnerId == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)))
            {
                return Unauthorized();
            }


            if (id != updatedProduct.ProductId)
            {
                return BadRequest();
            }

            var existingProduct = _context.Products.FirstOrDefault(p => p.ProductId == id);

            if (existingProduct == null)
            {
                return NotFound();
            }

           /* var currentUserId = (int)HttpContext.Items["UserId"];
            var currentUserRole = (string)HttpContext.Items["UserRole"];

            if (currentUserRole != "seller" || existingProduct.OwnerId != currentUserId)
            {
                return Forbid();
            }*/

            _context.Entry(existingProduct).CurrentValues.SetValues(updatedProduct);

            _context.SaveChanges();

            return NoContent(); // Retourne un statut 204 si le product est modifié
        }

        /// <summary>Delete product</summary>
        // DELETE: api/Product/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {

            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound(); // Retourne un statut 404 si le product n'est pas trouvé
            }

            if (!(User.FindFirst(ClaimTypes.Role)?.Value == "seller" && product.OwnerId == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)))
            {
                return Unauthorized();
            }

            /*var currentUserId = (int)HttpContext.Items["UserId"];
            var currentUserRole = (string)HttpContext.Items["UserRole"];

            if (currentUserRole != "seller" || product.OwnerId != currentUserId)
            {
                return Forbid();
            }*/

            _context.Products.Remove(product);
            _context.SaveChanges();

            return NoContent(); // Retourne un statut 204 si le product est supprimé
        }
    }
}