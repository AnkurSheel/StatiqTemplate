using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.FileProviders;

namespace Bookland.Integration.Tests
{
    public class TestServerFixture : IDisposable
    {
        public HttpClient? Client { get; private set; }

        private TestServer? _server;

        private readonly string _outputDirectory;

        public TestServerFixture()
        {
            _outputDirectory = TestHelpers.GetOutputDirectory();
            CreateClient();
        }

        public void Dispose()
        {
            Client?.Dispose();
            _server?.Dispose();
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
