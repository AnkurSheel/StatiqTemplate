using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.FileProviders;

namespace Bookland.Integration.Tests
{
    public class HttpServerFixture : IDisposable
    {
        public HttpClient? Client { get; private set; }

        private TestServer? _server;

        private readonly string _outputDirectory;

        public HttpServerFixture()
        {
            _outputDirectory = GetOutputDirectory();
            CreateClient();
        }

        public void Dispose()
        {
            Client?.Dispose();
            _server?.Dispose();
        }

        public static string GetOutputDirectory()
        {
            var directory = Directory.GetCurrentDirectory();

            while (directory != null && !Directory.GetFiles(directory, "*.sln").Any())
            {
                directory = Directory.GetParent(directory)?.FullName;
            }

            return directory != null
                ? Path.Combine(directory, "output")
                : "";
        }

        private  HttpClient CreateClient()
        {
            var hostBuilder = new WebHostBuilder().Configure(
                app =>
                {
                    var fileProvider = new PhysicalFileProvider(_outputDirectory);
                    app.UseDefaultFiles(
                            new DefaultFilesOptions
                            {
                                FileProvider = fileProvider
                            })
                        .UseStaticFiles(
                            new StaticFileOptions
                            {
                                FileProvider = fileProvider,
                            });
                });

            _server = new TestServer(hostBuilder);
            Client = _server.CreateClient();
            Client.BaseAddress = new Uri("http://localhost:6789");
            return Client;
        }
    }
}
