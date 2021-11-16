using System.IO;
using System.Linq;

namespace Bookland.Integration.Tests
{
    public class TestHelpers
    {
        public static string GetOutputDirectory()
        {
            var directory = GetSolutionDirectory();

            return directory != null
                ? Path.Combine(directory, "output")
                : "";
        }

        public static string? GetProjectDirectory()
            => GetDirectory("*.csproj");

        public static string? GetSolutionDirectory()
            => GetDirectory("*.sln");

        private static string? GetDirectory(string extension)
        {
            var directory = Directory.GetCurrentDirectory();

            while (directory != null && !Directory.GetFiles(directory, extension).Any())
            {
                directory = Directory.GetParent(directory)?.FullName;
            }

            return directory;
        }
    }
}
