using Microsoft.AspNetCore.Mvc.Authorization;
using PuppeteerSharp;

var builder = WebApplication.CreateBuilder(args);

var authority = builder.Configuration["AUTHORITY"];
var enableAuth = !string.IsNullOrEmpty(authority);

builder.Services.AddControllers(options =>
{
    if (enableAuth)
    {
        options.Filters.Add(new AuthorizeFilter());
    }
});
if (enableAuth)
{
    builder.Services.AddAuthentication()
        .AddJwtBearer(options =>
        {
            options.Authority = authority;
            options.TokenValidationParameters.ValidateAudience = false;
        });

    builder.Services.AddAuthorization();
}

var app = builder.Build();

if (enableAuth)
{
    app.UseAuthentication();
    app.UseAuthorization();
}
app.MapControllers();

#if DEBUG
// Ensure Chromium exists for local dev BEFORE handling requests
var browserFetcher = new BrowserFetcher();
await browserFetcher.DownloadAsync();
#endif

await app.RunAsync();