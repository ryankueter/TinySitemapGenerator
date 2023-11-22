using System.Xml.Linq;

namespace TinySitemapGenerator;

public static class SitemapOptionalNamespace
{
    public static readonly XAttribute Xhtml = new XAttribute(XNamespace.Xmlns + "xhtml", "http://www.w3.org/1999/xhtml");
    public static readonly XAttribute Video = new XAttribute(XNamespace.Xmlns + "video", "http://www.google.com/schemas/sitemap-video/1.1");
    public static readonly XAttribute News = new XAttribute(XNamespace.Xmlns + "news", "http://www.google.com/schemas/sitemap-news/1.1");
    public static readonly XAttribute Image = new XAttribute(XNamespace.Xmlns + "image", "http://www.google.com/schemas/sitemap-image/1.1");
    public static readonly XAttribute Mobile = new XAttribute(XNamespace.Xmlns + "mobile", "http://www.google.com/schemas/sitemap-mobile/1.1");
}
