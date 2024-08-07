namespace SoftDesignBack.Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Author { get; set; }
        public string? TenantId { get; set; }
        public ApplicationUser? Tenant { get; set; }
    }
}