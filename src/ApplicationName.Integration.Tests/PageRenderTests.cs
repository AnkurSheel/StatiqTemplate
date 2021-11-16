using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PlaywrightSharp;
using VerifyXunit;
using Xunit;

namespace Bookland.Integration.Tests
{
    [UsesVerify]
    public class PageRenderTests : IClassFixture<TestServerFixture>
    {
        private readonly HttpClient? _httpClient;

        public PageRenderTests(TestServerFixture fixture)
        {
            _httpClient = fixture.Client;
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public async Task Page_is_rendered(string path)
        {
            var settings = new VerifyTests.VerifySettings();
            settings.UseParameters(path);

            if (_httpClient == null)
            {
                Assert.False(true);
                return;
            }

            var response = await _httpClient.GetAsync(path);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            await Verifier.Verify(content, settings);
        }

        [Fact]
        public async Task File_are_outputted_correctly()
        {
            var outputDirectory = TestHelpers.GetOutputDirectory();
            var files = Directory.GetFiles(outputDirectory, "*", SearchOption.AllDirectories)
                .OrderBy(x => x)
                .Select(x => x.Substring(outputDirectory.Length).Replace("\\", "/").Replace("index.html", "").Replace(".html", "/"));
            await Verifier.Verify(files);
        }

        public static IEnumerable<object[]> GetData()
        {
            var outputDirectory = TestHelpers.GetOutputDirectory();
            var patterns = new[] { "*.html", "*.js", "*.xml" };

            List<string> filePaths = new List<string>();

            filePaths = patterns.Aggregate(filePaths, (current, pattern) => current.Concat(Directory.GetFiles(outputDirectory, pattern, SearchOption.AllDirectories)).ToList());

            filePaths = filePaths.Select(x => x.Substring(outputDirectory.Length).Replace("\\", "/")).ToList();

            return filePaths.Select(x => new object[] { x });
        }
    }
}
