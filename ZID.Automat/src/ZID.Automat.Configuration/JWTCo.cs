namespace ZID.Automat.Configuration
{
    public record JWTCo
    {
        public string JWTSecret { get; init; } = string.Empty;
        public float JWTExpireTime { get; init; }
    }
}