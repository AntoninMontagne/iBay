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

            // Tri par date, nom ou prix
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

            // Limite le nombre de produits retournés
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

            //return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
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

           /* var currentUserId = (int)HttpContext.Items["UserId"];
            var currentUserRole = (string)HttpContext.Items["UserRole"];

            if (currentUserRole != "seller" || existingProduct.OwnerId != currentUserId)
            {
                return Forbid();
            }*/

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

            return Ok("product deleted successfully");
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public ActionResult<List<Product>> SearchProducts(string? name, decimal? price, int? ownerId)
        {
            var query = _context.Products.AsQueryable();

            // Filtrer par nom si le paramètre name est spécifié
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            // Filtrer par prix si le paramètre price est spécifié
            if (price.HasValue)
            {
                query = query.Where(p => p.Price == price.Value);
            }

            // Filtrer par propriétaire si le paramètre ownerId est spécifié
            if (ownerId.HasValue)
            {
                query = query.Where(p => p.OwnerId == ownerId.Value);
            }

            // Exécuter la requête et renvoyer les résultats
            var matchingProducts = query.ToList();
            return Ok(matchingProducts);
        }

    }
}