using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using PuppeteerSharp.Media;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/api/generate-pdf", async ([FromBody]GeneratePdfDto input) =>
{
    var launchOptions = new LaunchOptions { Headless = true, Args = ["--no-sandbox", "--disable-setuid-sandbox"] };

    await using var browser = await Puppeteer.LaunchAsync(launchOptions);
    await using var page = await browser.NewPageAsync();

    await page.SetContentAsync(input.ContentHtml);

    var pdf = await page.PdfStreamAsync(new PdfOptions
    {
        Format = PaperFormat.A4,
        PrintBackground = true
    });

    return pdf == null
        ? Results.NotFound()
        : Results.File(pdf, "application/pdf", $"{input.FileName}.pdf");
});

await app.RunAsync();