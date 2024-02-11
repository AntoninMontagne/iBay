namespace Dal
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public bool Available { get; set; }
        public DateTime AddedTime { get; set; }
        public int OwnerId { get; set; }
    }
}
