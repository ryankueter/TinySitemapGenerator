# TinySitemapGenerator

Author: Ryan Kueter  
Updated: November, 2023

## About

**TinySitemapGenerator** is a free .NET library available from the [NuGet Package Manager](https://www.nuget.org/packages/TinySitemapGenerator) that provides a simple way to create a sitemap or a sitemap index for your website.

### Targets:
- .NET 8

## Introduction

The TinySitemapGenerator provides the following options for automatically generating an XML sitemap.

```csharp
// Create a new sitemap
var sitemap = new Sitemap();

// Optionally add the xsd reference for validation
sitemap.ValidateMap = true;

// Specify the file path
sitemap.Filepath = @"C:\wwwroot";

// Add optional namespaces
sitemap.SitemapOptionalNamespaces.Add(SitemapOptionalNamespace.News);

// Add Sitemap Urls
sitemap.SitemapUrls.Add(
    new SitemapUrl { 
        Location = "http://www.mysite.com/article/tutorial.html",
        LastModified = DateTime.Now,
        Priority = SitemapPriorities.Nine,
        ChangeFrequency = SitemapChangeFrequencies.Never
    }
);

// Generate a standard XML sitemap
sitemap.SaveSitemap();

// Generate a XML sitemap index because
// you have more than 50,000 urls or your
// files exceed 50MB
sitemap.SaveSitemapIndex();

// Generate a standard XML sitemap
await sitemap.SaveSitemapAsync();

// Generate a XML sitemap index because
// you have more than 50,000 urls or your
// files exceed 50MB
await sitemap.SaveSitemapIndexAsync();

// Get the file bytes
var bytes = await sitemap.GetSitemapBytesAsync();
await File.WriteAllBytesAsync(Path.Combine(@"C:\wwwroot", "sitemap.xml"), bytes);

// Generate a XML sitemap index because
// you have more than 50,000 urls or your
// files exceed 50MB
var bytes = await sitemap.GetSitemapIndexBytesAsync();
await File.WriteAllBytesAsync(Path.Combine(@"C:\wwwroot", "sitemap.xml"), bytes);
``` 

###
## Contributions

This project is being developed for free by me, Ryan Kueter, in my spare time. So, if you would like to contribute, please submit your ideas on the Github project page.