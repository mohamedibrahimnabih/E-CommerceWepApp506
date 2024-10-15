namespace E_Commerce.Models
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Adress { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
