namespace SoftDesignBack.Domain.Dtos
{
    public class LoginResponseDto
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? Error { get; set; }
    }
}