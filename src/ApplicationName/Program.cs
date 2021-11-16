using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Statiq.App;
using Statiq.Common;
using Statiq.Web;
using StatiqHelpers.Extensions;
using StatiqHelpers.ImageHelpers;
using StatiqHelpers.Pipelines;

namespace ankursheel_website
{
    internal class Program
    {
        public static async Task<int> Main(string[] args)
            => await Bootstrapper.Factory.CreateWeb(args)
                .RemovePipelines()
                .AddSetting(WebKeys.OutputPath, "../../output")
                .AddSetting(WebKeys.CachePath, "../../cache")
                .AddCommand<ResizeImage>()
                .AddPipelines()
                .AddServices()
                .ConfigureServices(
                    services =>
                    {
                        services.AddSingleton(new PostListOptions(document => document.GetPublishedDate(), Descending: true));
                    })
                .RunAsync();
    }
}
