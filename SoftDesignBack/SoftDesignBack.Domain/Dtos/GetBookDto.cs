namespace SoftDesignBack.Domain.Dtos
{
    public class GetBookDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public GetBookTenantDto? Tenant { get; set; }
    }

    public class GetBookTenantDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
    }
}