namespace TinySitemapGenerator;

public sealed class SitemapUrl
{
    public string? Location { get; set; }
    public DateTime? LastModified { get; set; }
    public string? ChangeFrequency { get; set; }
    public string? Priority { get; set; }
}
