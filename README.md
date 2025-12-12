# Pdf Service

A Dockerized .NET Web API service that leverages Puppeteer and pre-installed Chrome to efficiently generate PDFs from HTML via REST endpoints.

## Features
- Convert HTML to PDF using PuppeteerSharp.
- Pre-installed Google Chrome in the Docker image for optimized load times and PDF generation.
- Exposes a REST API for PDF creation.
- Automated build and publish to GitHub Container Registry (GHCR) via GitHub Actions.

## Prerequisites
- Docker installed on your machine.
- .NET SDK 9.0 (for local development/building).

## Building Locally
1. Clone the repository:
   ```
   git clone https://github.com/fuse-digital/pdf-service.git
   cd pdf-service
   ```

2. Publish the .NET project:
   ```
   dotnet publish ./src/FuseDigital.PdfService/FuseDigital.PdfService.csproj --output dist --configuration Release
   ```

3. Build the Docker image:
   ```
   docker build -t pdf-service:latest .
   ```

## Usage
### Running the Docker Image
Pull the latest image from GHCR (replace `your-username` with the repository owner):
```
docker pull ghcr.io/fuse-digital/pdf-service:latest
```

Run the container:
```
docker run -d -p 8080:8080 --name pdf-service ghcr.io/fuse-digital/pdf-service:latest
```

Access the API at `http://localhost:8080`. (API endpoints depend on the implementation; e.g., POST to `/api/pdf` with HTML payload.)

### API Example
Send a POST request to generate a PDF:
- Endpoint: `/api/generate-pdf` (adjust based on actual implementation)
- Body: `{ "fileName": "sample", "contentHtml": "<html><body>Hello World</body></html>" }`
- Response: PDF binary.
