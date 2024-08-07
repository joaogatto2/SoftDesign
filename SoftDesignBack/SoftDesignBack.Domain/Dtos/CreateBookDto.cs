namespace SoftDesignBack.Domain.Dtos
{
    public class CreateBookDto
    {
        public required string Name { get; set; }
        public required string Author { get; set; }
    }
}