using Dal;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            var cart = _context.Carts
                   .Include(c => c.Products)
                   .FirstOrDefault(c => c.CartID == id);

            if (cart == null)
            {
                return NotFound();
            }

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != cart.OwnerId.ToString())
            {
                return Unauthorized();
            }

            return Ok(cart);
        }

        // POST: api/Cart/{id}
        /// <summary>Create cart</summary>
        [HttpPost]
        public ActionResult<Cart> PostCart(Cart cart)
        {
            var list = new List<Product>();
            foreach (var product in cart.Products)
            {
                var productInDb = _context.Products.Find(product.ProductId);
                if (productInDb != null)
                {
                    list.Add(productInDb);
                }
            }

            cart.Products = list;

            var user = _context.Users.Find(cart.OwnerId);
            if (user == null)
            {
                return NotFound("User not found");
            }
            cart.OwnerId = user.UserId;
            _context.Carts.Add(cart);
            _context.SaveChanges();

            return Ok(new { Message = "cart created successfully", Cart = cart });
        }

        // PUT: api/Product/{id}
        /// <summary>Update cart</summary>
        [HttpPut("{id}")]
        public IActionResult PutCart(int id, Cart updatedCart)
        {
            // Recherchez le panier existant dans la base de données
            var existingCart = _context.Carts.Include(c => c.Products).FirstOrDefault(c => c.CartID == id);

            if (existingCart == null)
            {
                return NotFound("Cart not found");
            }

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != existingCart.OwnerId.ToString())
            {
                return Unauthorized();
            }

            var updatedProductIds = updatedCart.Products.Select(p => p.ProductId).ToList();
            existingCart.Products.Clear();
            foreach (var productId in updatedProductIds)
            {
                var productInDb = _context.Products.Find(productId);
                if (productInDb != null)
                {
                    existingCart.Products.Add(productInDb);
                }
            }

            var user = _context.Users.Find(updatedCart.OwnerId);
            if (user == null)
            {
                return NotFound("User not found");
            }
            existingCart.OwnerId = user.UserId;

            _context.SaveChanges();

            return Ok(new { Message = "cart modified successfully", Cart = existingCart });
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

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != cart.OwnerId.ToString())
            {
                return Unauthorized();
            }

            foreach (var product in products)
            {
                var productInDb = _context.Products.Find(product.ProductId);
                if (productInDb != null)
                {
                    cart.Products.Add(productInDb);
                }
            }

            _context.SaveChanges();

            return Ok(new { Message = "product added successfully", Cart = cart });
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

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != cart.OwnerId.ToString())
            {
                return Unauthorized();
            }

            foreach (var product in products)
            {
                var productInDb = _context.Products.Find(product.ProductId);
                if (productInDb != null)
                {
                    cart.Products.Remove(productInDb);
                }
            }

            _context.SaveChanges();

            return Ok(new { Message = "product removed successfully", Cart = cart });
        }

        // DELETE: api/Product/{id}
        /// <summary>Delete cart</summary>
        [HttpDelete("{id}")]
        public IActionResult DeleteCart(int id)
        {
            Cart? cart = _context.Carts
                   .Include(c => c.Products)
                   .FirstOrDefault(c => c.CartID == id);


            if (cart == null)
            {
                return NotFound();
            }

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != cart.OwnerId.ToString())
            {
                return Unauthorized();
            }

            _context.Carts.Remove(cart);
            _context.SaveChanges();

            return Ok(new { Message = "cart deleted successfully", Cart = cart });
        }

        [HttpGet("Pay/{id}")]
        public ActionResult<int> ActionResult(int id)
        {
            var cart = _context.Carts
                   .Include(c => c.Products)
                   .FirstOrDefault(c => c.CartID == id);

            if (cart == null)
            {
                return NotFound();
            }

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != cart.OwnerId.ToString())
            {
                return Unauthorized();
            }

            decimal sum = 0;

            foreach (var product in cart.Products)
            {
                sum += product.Price;
            }

            return Ok(sum);
        }
    }
}