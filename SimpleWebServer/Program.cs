
using System.Diagnostics;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;

// Clear Chrome Cache
// Directory.GetFiles(@"%LOCALAPPDATA%\Google\Chrome\User Data\Default\Cache", "*", SearchOption.AllDirectories).ToList().ForEach(x => File.Delete(x));

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

// Configuring options
var fileExtensions = new FileExtensionContentTypeProvider();
fileExtensions.Mappings[".data"] = "application/octet-stream";
fileExtensions.Mappings[".bundle"] = "application/octet-stream";

var serverOptions = new FileServerOptions();
serverOptions.FileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory());
serverOptions.StaticFileOptions.ContentTypeProvider = fileExtensions;

// Configuring Port and Content Root
var builder = WebApplication.CreateSlimBuilder(args);
builder.WebHost.UseContentRoot(Directory.GetCurrentDirectory());
builder.WebHost.ConfigureKestrel(serverOptions => serverOptions.ListenAnyIP(port));

// Running the webserver
var app = builder.Build();
app.UseFileServer(serverOptions);
app.Run();
