using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PuppeteerSharp;
using PuppeteerSharp.Media;

var builder = WebApplication.CreateBuilder(args);

var authority = builder.Configuration["AUTHORITY"];
var audience = builder.Configuration["AUDIENCE"];
var enableAuth = !string.IsNullOrEmpty(authority) && !string.IsNullOrEmpty(audience);

if (enableAuth)
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.Authority = authority;
            options.Audience = audience;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = authority,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
            };
        });
    
    builder.Services.AddAuthorization();
}

var app = builder.Build();

if (enableAuth)
{
    app.UseAuthentication();
    app.UseAuthorization();
}

var endpoint = app.MapPost("/api/generate-pdf", async ([FromBody] GeneratePdfDto input) =>
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

if (enableAuth)
{
    endpoint.RequireAuthorization();
}

await app.RunAsync();