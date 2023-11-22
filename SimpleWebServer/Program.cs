
using System.Diagnostics;
using System.IO.Compression;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;

// Getting the port from the command line
short port = 9999;

if (short.TryParse(Environment.GetCommandLineArgs().LastOrDefault(), out short newPort))
{
    port = newPort;
}

// Opening up website to our webserver
Process.Start(new ProcessStartInfo
{
    FileName = $"http://localhost:{port}",
    UseShellExecute = true,
});

// Configuring FileServer
var fileServerOptions = new FileServerOptions();
fileServerOptions.EnableDefaultFiles = true;
fileServerOptions.FileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
fileServerOptions.StaticFileOptions.ContentTypeProvider = new FileExtensionContentTypeProvider();
fileServerOptions.StaticFileOptions.ServeUnknownFileTypes = true;

// Overriding Response Headers
fileServerOptions.StaticFileOptions.OnPrepareResponse = ctx =>
{
    // Making sure nothing we send out is ever cached
    ctx.Context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";

    // Unity Specific extentions
    Modify(ctx.File.Name, ".data",         "application/octet-stream");
    Modify(ctx.File.Name, ".bundle",       "application/octet-stream");
    Modify(ctx.File.Name, ".symbols.json", "application/octet-stream");
    Modify(ctx.File.Name, ".js",           "application/javascript");
    Modify(ctx.File.Name, ".wasm",         "application/wasm");

    void Modify(string fileName, string extension, string contentType)
    {
        if (fileName.EndsWith(extension) || fileName.EndsWith(extension + ".gz") || fileName.EndsWith(extension + ".br"))
        {
            ctx.Context.Response.Headers["Content-Type"] = contentType;

            if (fileName.EndsWith(".gz"))
            {
                ctx.Context.Response.Headers["Content-Encoding"] = "gzip";
            }
            else if (fileName.EndsWith(".br"))
            {
                ctx.Context.Response.Headers["Content-Encoding"] = "br";
            }
        }
    }
};

// Creating Builder
var builder = WebApplication.CreateSlimBuilder(args);
builder.WebHost.ConfigureKestrel(serverOptions => serverOptions.ListenAnyIP(port));
builder.Services.AddResponseCompression(options => 
{
    options.Providers.Add<GzipCompressionProvider>();
    options.Providers.Add<BrotliCompressionProvider>();
});

builder.Services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

// Running the Web Server
var app = builder.Build();
app.UseResponseCompression();
app.UseFileServer(fileServerOptions);
app.Run();
