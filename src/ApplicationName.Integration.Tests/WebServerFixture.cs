using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using PlaywrightSharp;
using Xunit;

namespace Bookland.Integration.Tests
{
    public class WebServerFixture : IAsyncLifetime, IDisposable
    {
        private readonly IWebHost host;

        private IPlaywright? _playwright { get; set; }

        public IBrowser Browser { get; private set; } = null!;

        public IPage Page { get; private set; } = null!;

        public string BaseUrl { get; } = $"http://localhost:{GetRandomUnusedPort()}";

        public WebServerFixture()
        {
            var outputDirectory = TestHelpers.GetOutputDirectory();

            var hostBuilder = new WebHostBuilder().Configure(
                app =>
                {
                    var fileProvider = new PhysicalFileProvider(outputDirectory);
                    app.UseFileServer(
                        new FileServerOptions()
                        {
                            FileProvider = fileProvider,
                            // EnableDirectoryBrowsing = true,
                            // DirectoryBrowserOptions = { FileProvider = fileProvider}
                        });
                    // app.UseDefaultFiles(
                    //         new DefaultFilesOptions
                    //         {
                    //             FileProvider = fileProvider
                    //         })
                    //     .UseStaticFiles(
                    //         new StaticFileOptions
                    //         {
                    //             FileProvider = fileProvider,
                    //         });
                });

            host = hostBuilder.UseKestrel().UseUrls(BaseUrl).Build();
        }

        public async Task InitializeAsync()
        {
            await PlaywrightHelpers.InstallAsync();
            _playwright = await Playwright.CreateAsync();
            Browser = await _playwright.Chromium.LaunchAsync(
                new LaunchOptions
                {
                    // Headless = false,
                    IgnoreHTTPSErrors = true,
                });

            await host.StartAsync();

            Page = await Browser.NewPageAsync();
        }

        public async Task DisposeAsync()
        {
            await host.StopAsync();
            host?.Dispose();
            _playwright?.Dispose();
        }

        public void Dispose()
        {
            host?.Dispose();
            _playwright?.Dispose();
        }

        private static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Any, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
    }
}
