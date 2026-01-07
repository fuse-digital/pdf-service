using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace FuseDigital.PdfService
{
    [Route("api")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        [HttpPost]
        [Route("generate-pdf")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GeneratePdfAsync([FromBody] GeneratePdfDto input)
        {
            var launchOptions = new LaunchOptions { Headless = true, Args = ["--no-sandbox", "--disable-setuid-sandbox"] };

            await using var browser = await Puppeteer.LaunchAsync(launchOptions);
            await using var page = await browser.NewPageAsync();

            await page.SetContentAsync(input.ContentHtml, new NavigationOptions
            {
                WaitUntil = [WaitUntilNavigation.Networkidle2],
                Timeout = 15000,
            });

            var pdf = await page.PdfDataAsync(new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true
            });

            return pdf == null
                ? NotFound()
                : File(pdf, "application/pdf", $"{input.FileName}.pdf");
        }
    }
}
