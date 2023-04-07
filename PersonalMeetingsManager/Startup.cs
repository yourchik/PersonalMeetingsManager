using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PersonalMeetingsManager.Models;
using PersonalMeetingsManager.Services.Implementation;
using PersonalMeetingsManager.Services.Interface;

namespace PersonalMeetingsManager;

public static class Startup
{
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                ConfigureServices(services);
            });

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IMenu, Menu>();
        services.AddSingleton<IMeetingManager, MeetingManager>();
        services.AddSingleton<IExelExport, ExelExport>();
    }
}