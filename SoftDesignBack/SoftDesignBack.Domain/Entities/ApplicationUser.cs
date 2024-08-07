namespace SoftDesignBack.Domain.Entities
{
    using Microsoft.AspNetCore.Identity;
    
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Book> Books { get; } = new List<Book>();
    }
}