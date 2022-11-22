namespace ZID.Automat.Dto.Models
{
    public record ItemDisplayDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SubName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public bool Available { get; set; }

    }
}
