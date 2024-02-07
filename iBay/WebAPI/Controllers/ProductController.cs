using Dal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppContext _context;

        public ProductController(AppContext context)
        {
            _context = context;
        }

        // GET: api/Product
        /// <summary>Get products</summary>
        [HttpGet]
        public ActionResult<List<Product>> GetProduct()
        {
            var products = _context.Products.ToList();
            return Ok(products);
        }

        // GET: api/Product/{id}
        /// <summary>Get one product details</summary>
        [HttpGet("{id}")]
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
            _context.Products.Add(product);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
        }

        /// <summary>Update product</summary>
        // PUT: api/Product/{id}
        [HttpPut("{id}")]
        public IActionResult PutProduct(int id, Product updatedProduct)
        {
            if (id != updatedProduct.ProductId)
            {
                return BadRequest();
            }

            var existingProduct = _context.Products.FirstOrDefault(p => p.ProductId == id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            _context.Entry(existingProduct).CurrentValues.SetValues(updatedProduct);

            _context.SaveChanges();

            return NoContent(); // Retourne un statut 204 si le product est modifié
        }

        /// <summary>Delete product</summary>
        // DELETE: api/Product/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound(); // Retourne un statut 404 si le product n'est pas trouvé
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return NoContent(); // Retourne un statut 204 si le product est supprimé
        }
    }
}
