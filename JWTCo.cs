namespace ZID.Automat.Configuration.Model
{
    public record JWTConfigurationCo
    {
        public string JWTSecret { get; init; }
        public DateTime JWTExpireTime { get; init; }
    }
}