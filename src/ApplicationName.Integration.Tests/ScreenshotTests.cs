using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace Bookland.Integration.Tests
{
    [UsesVerify]
    public class ScreenshotTests : IClassFixture<WebServerFixture>
    {
        private readonly WebServerFixture _fixture;

        public ScreenshotTests(WebServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [MemberData(nameof(GetData))]
        public async Task ScreenshotPage(string path)
        {
            // Navigate to the home page
            await _fixture.Page.GoToAsync($"{_fixture.BaseUrl}{path}");

            path = path.Replace("/", "-");

            await _fixture.Page.ScreenshotAsync(
                Path.Combine(TestHelpers.GetProjectDirectory() ?? string.Empty, "Screenshots", $"{nameof(ScreenshotPage)}{path}.png"),
                true);
        }

        public static IEnumerable<object[]> GetData()
        {
            var outputDirectory = TestHelpers.GetOutputDirectory();
            var patterns = new[] { "*.html" };

            var filePaths = Directory.GetFiles(outputDirectory, "*.html", SearchOption.AllDirectories)
                .Select(x => x.Substring(outputDirectory.Length).Replace("\\", "/"))
                .ToList();

            return filePaths.Select(x => new object[] { x });
        }
    }
}
