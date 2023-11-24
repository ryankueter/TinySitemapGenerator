using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Xml;
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
    public string Filepath { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

    /// <summary>
    /// Save Sitemap - Synchronous
    /// </summary>
    public void SaveSitemap() => new XDocument(GenerateSitemap()).Save(Path.Combine(Filepath, "sitemap.xml"));

    /// <summary>
    /// Download Sitemap - Synchronous
    /// </summary>
    [Obsolete("DownloadSitemap is obsolete. Use SaveSitemap instead.")]
    public void DownloadSitemap() => new XDocument(GenerateSitemap()).Save(Path.Combine(Filepath, "sitemap.xml"));
    
    /// <summary>
    /// Save Sitemap - Asynchronous
    /// </summary>
    public async Task SaveSitemapAsync() => await SaveAsync(new XDocument(GenerateSitemap()));

    /// <summary>
    /// Download Sitemap - Asynchronous
    /// </summary>
    [Obsolete("DownloadSitemapAsync is obsolete. Use SaveSitemapAsync instead.")]
    public async Task DownloadSitemapAsync() => await SaveAsync(new XDocument(GenerateSitemap()));

    /// <summary>
    /// Download Sitemap Index - Asynchronous
    /// </summary>
    public async Task<byte[]> GetSitemapBytesAsync() => await GetBytesAsync(new XDocument(GenerateSitemap()));

    /// <summary>
    /// Save Sitemap Index - Synchronous
    /// </summary>
    public void SaveSitemapIndex() => new XDocument(GenerateSitemapIndex()).Save(Path.Combine(Filepath, "sitemap.xml"));

    /// <summary>
    /// Download Sitemap Index - Synchronous
    /// </summary>
    [Obsolete("DownloadSitemapIndex is obsolete. Use SaveSitemapIndex instead.")]
    public void DownloadSitemapIndex() => new XDocument(GenerateSitemapIndex()).Save(Path.Combine(Filepath, "sitemap.xml"));

    /// <summary>
    /// Save Sitemap Index - Asynchronous
    /// </summary>
    public async Task SaveSitemapIndexAsync() => await SaveAsync(new XDocument(GenerateSitemapIndex()));

    /// <summary>
    /// Download Sitemap Index - Asynchronous
    /// </summary>
    [Obsolete("DownloadSitemapIndexAsync is obsolete. Use SaveSitemapIndexAsync instead.")]
    public async Task DownloadSitemapIndexAsync() => await SaveAsync(new XDocument(GenerateSitemapIndex()));

    /// <summary>
    /// Download Sitemap Index - Asynchronous
    /// </summary>
    public async Task<byte[]> GetSitemapIndexBytesAsync() => await GetBytesAsync(new XDocument(GenerateSitemapIndex()));


    private async Task SaveAsync(XDocument sitemap)
    {
        // Write the file to disk
        using var stream = new FileStream(Path.Combine(Filepath, "sitemap.xml"), FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);
        await sitemap.SaveAsync(stream, SaveOptions.None, default);
    }

    /// <summary>
    /// Download async
    /// </summary>
    /// <returns></returns>
    private async Task<byte[]> GetBytesAsync(XDocument sitemap)
    {
        // Create a memory stream
        using MemoryStream memoryStream = new MemoryStream();

        // Use asynchronous method to write the XML content to the MemoryStream
        await sitemap.SaveAsync(memoryStream, SaveOptions.None, default);

        // Return the byte array
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Generates a standard sitemap
    /// </summary>
    private XElement GenerateSitemap()
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
                links.LastModified is not null && links.LastModified != default ? new XElement(ns + "lastmod", String.Format("{0:yyyy-MM-dd}", links.LastModified)) : null,
                links.ChangeFrequency is not null && links.ChangeFrequency != String.Empty ? new XElement(ns + "changefreq", links.ChangeFrequency) : null,
                links.Priority is not null && links.Priority != String.Empty ? new XElement(ns + "priority", links.Priority) : null
            ));
        }
        return urlset;
    }

    /// <summary>
    /// Generates a sitemap index
    /// This is used if you need to split your sitemap 
    /// into multiple files because they exceed 50,000 or 50MB.
    /// </summary>
    private XElement GenerateSitemapIndex()
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
                links.LastModified is not null && links.LastModified != default ? new XElement(ns + "lastmod", String.Format("{0:yyyy-MM-dd}", links.LastModified)) : null
            ));
        }
        return urlset;
    }
}