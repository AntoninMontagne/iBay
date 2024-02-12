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
        public ActionResult<List<Product>> GetProduct([FromQuery] string sortBy = "addedTime", [FromQuery] int limit = 10)
        {
            IQueryable<Product> query = _context.Products;

            switch (sortBy.ToLower())
            {
                case "name":
                    query = query.OrderBy(p => p.Name);
                    break;
                case "price":
                    query = query.OrderBy(p => p.Price);
                    break;
                default:
                    query = query.OrderBy(p => p.AddedTime);
                    break;
            }

            query = query.Take(limit);

            var products = query.ToList();
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
                return NotFound();
            }

            return Ok(product);
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

            var user = _context.Users.FirstOrDefault(u => u.UserId == product.OwnerId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            _context.Products.Add(product);
            _context.SaveChanges();

            return Ok(new { Message = "product created successfully", Product = product });
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

            var user = _context.Users.FirstOrDefault(u => u.UserId == updatedProduct.OwnerId);
            if (user == null)
            {
                   return NotFound("User not found");
            }

            _context.Entry(existingProduct).CurrentValues.SetValues(updatedProduct);

            _context.SaveChanges();

            return Ok(new { Message = "product modified successfully", Product = updatedProduct});
        }

        /// <summary>Delete product</summary>
        // DELETE: api/Product/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {

            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            if (!(User.FindFirst(ClaimTypes.Role)?.Value == "seller" && product.OwnerId == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)))
            {
                return Unauthorized();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return Ok("product deleted successfully");
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public ActionResult<List<Product>> SearchProducts(string? name, decimal? price, int? ownerId)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            if (price.HasValue)
            {
                query = query.Where(p => p.Price == price.Value);
            }

            if (ownerId.HasValue)
            {
                query = query.Where(p => p.OwnerId == ownerId.Value);
            }

            var matchingProducts = query.ToList();
            return Ok(matchingProducts);
        }

    }
}