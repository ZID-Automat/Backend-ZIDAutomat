namespace ZID.Automat.Configuration.Model
{
    public record JWTCo
    {
        public string JWTSecret { get; init; } = string.Empty;
        public float JWTExpireTime { get; init; }
    }
}