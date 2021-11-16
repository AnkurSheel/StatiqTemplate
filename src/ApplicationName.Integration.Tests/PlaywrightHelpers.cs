using System;
using System.Threading.Tasks;
using PlaywrightSharp;

namespace Bookland.Integration.Tests
{
    public static class PlaywrightHelpers
    {
        private static readonly Lazy<Task> _install = new Lazy<Task>(() => Playwright.InstallAsync());
        public static Task InstallAsync() => _install.Value;
    }
}
