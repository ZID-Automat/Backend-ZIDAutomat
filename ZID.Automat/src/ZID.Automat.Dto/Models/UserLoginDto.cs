namespace ZID.Automat.Dto.Models
{
    public record UserLoginDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
