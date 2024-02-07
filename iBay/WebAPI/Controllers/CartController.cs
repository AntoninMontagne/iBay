using Dal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly AppDBContext _context;

        public CartController(AppDBContext context)
        {
            _context = context;
        }

        // GET: api/Cart
        /// <summary>Get carts</summary>
        [HttpGet]
        public ActionResult<List<Cart>> GetCarts()
        {
            return Ok(_context.Carts.ToList());
        }

        // GET: api/Cart/{id}
        /// <summary>Get cart details</summary>
        [HttpGet("{id}")]
        public ActionResult<Cart> GetCartById(int id)
        {
            var cart = _context.Carts.Find(id);

            if (cart == null)
            {
                return NotFound();
            }

            return Ok(cart);
        }

        // POST: api/Cart/{id}
        /// <summary>Create cart</summary>
        [HttpPost]
        public ActionResult<Cart> PostCart(Cart cart)
        {
            _context.Carts.Add(cart);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetCartById), new { id = cart.CartID }, cart);
        }

        // PUT: api/Product/{id}
        /// <summary>Update cart</summary>
        [HttpPut("{id}")]
        public IActionResult PutCart(int id, Cart cart)
        {
            if (id != cart.CartID)
            {
                return BadRequest();
            }

            _context.Entry(cart).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // PUT: api/Cart/AddProducts/{id}
        /// <summary>Add products to cart</summary>
        [HttpPut("AddProducts/{id}")]
        public IActionResult AddProductsToCart(int id, List<Product> products)
        {
            var cart = _context.Carts.Include(c => c.Products).FirstOrDefault(c => c.CartID == id);

            if (cart == null)
            {
                return NotFound();
            }

            foreach (var product in products)
            {
                cart.Products.Add(product);
            }

            _context.SaveChanges();

            return Ok(cart);
        }

        // PUT: api/Cart/RemoveProducts/{id}
        /// <summary>Remove products to cart</summary>
        [HttpPut("RemoveProducts/{id}")]
        public IActionResult RemoveProductsFromCart(int id, List<Product> products)
        {
            var cart = _context.Carts.Include(c => c.Products).FirstOrDefault(c => c.CartID == id);

            if (cart == null)
            {
                return NotFound();
            }

            foreach (var product in products)
            {
                var productToRemove = cart.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
                if (productToRemove != null)
                {
                    cart.Products.Remove(productToRemove);
                }
            }

            _context.SaveChanges();

            return Ok(cart);
        }

        // DELETE: api/Product/{id}
        /// <summary>Delete cart</summary>
        [HttpDelete("{id}")]
        public IActionResult DeleteCart(int id)
        {
            var cart = _context.Carts.Find(id);

            if (cart == null)
            {
                return NotFound();
            }

            _context.Carts.Remove(cart);
            _context.SaveChanges();

            return Ok(cart);
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.CartID == id);
        }
    }
}