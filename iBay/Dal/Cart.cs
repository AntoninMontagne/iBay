namespace Dal
{
    public class Cart
    {
        public int CartID { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public int OwnerId { get; set; }

        public Cart()
        {
            Products = new HashSet<Product>();
        }

    }
}
