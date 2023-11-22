using System.Xml.Linq;

namespace TinySitemapGenerator;

/// <summary>
/// https://www.sitemaps.org/protocol.html
/// </summary>
public sealed class Sitemap
{
    /// <summary>
    /// Turn off xml validation
    /// </summary>
    public bool ValidateMap = false;

    /// <summary>
    /// A list of sitemap urls
    /// </summary>
    public IList<SitemapUrl> SitemapUrls = new List<SitemapUrl>();

    /// <summary>
    /// A list of Sitemap Optional Namespaces
    /// Some sample namespaces are in the SitemapOptionalNamespace static class
    /// </summary>
    public IList<XAttribute> SitemapOptionalNamespaces = new List<XAttribute>();

    /// <summary>
    /// Where to save your file
    /// </summary>
    public string? Filepath { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

    /// <summary>
    /// Generates a standard sitemap
    /// </summary>
    public void GenerateSitemap()
    {
        XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        var urlset = new XElement(ns + "urlset");
        foreach (var ans in SitemapOptionalNamespaces)
        {
            urlset.Add(ans);
        }

        if (ValidateMap)
        {
            XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
            urlset.Add(new XAttribute(XNamespace.Xmlns + "xsi", xsi));
            urlset.Add(new XAttribute(xsi + "schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd"));
        }

        foreach (var links in SitemapUrls)
        {
            urlset.Add(
                new XElement(ns + "url",
                new XElement(ns + "loc", links.Location),
                links.LastModified is not null ? new XElement(ns + "lastmod", String.Format("{0:yyyy-MM-dd}", links.LastModified)) : null,
                links.ChangeFrequency is not null ? new XElement(ns + "changefreq", links.ChangeFrequency) : null,
                links.Priority is not null ? new XElement(ns + "priority", links.Priority) : null
            ));
        }

        new XDocument(urlset).Save($@"{Filepath}\sitemap.xml");
    }

    /// <summary>
    /// Generates a sitemap index
    /// This is used if you need to split your sitemap 
    /// into multiple files because they exceed 50,000 or 50MB.
    /// </summary>
    public void GenerateSitemapIndex()
    {
        XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        var urlset = new XElement(ns + "sitemapindex");
        foreach (var ans in SitemapOptionalNamespaces)
        {
            urlset.Add(ans);
        }

        if (ValidateMap)
        {
            XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
            urlset.Add(new XAttribute(XNamespace.Xmlns + "xsi", xsi));
            urlset.Add(new XAttribute(xsi + "schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/siteindex.xsd"));
        }

        foreach (var links in SitemapUrls)
        {
            urlset.Add(
                new XElement(ns + "sitemap",
                new XElement(ns + "loc", links.Location),
                links.LastModified is not null ? new XElement(ns + "lastmod", String.Format("{0:yyyy-MM-dd}", links.LastModified)) : null
            ));
        }

        new XDocument(urlset).Save($@"{Filepath}\sitemap.xml");
    }
}