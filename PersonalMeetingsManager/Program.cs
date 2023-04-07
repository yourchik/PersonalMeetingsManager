using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PersonalMeetingsManager;
using PersonalMeetingsManager.Services.Interface;

var host = Startup.CreateHostBuilder(args).Build();


Console.WriteLine("Hello World!");

var menu = host.Services.GetService<IMenu>();

menu?.ShowMenu();

host.WaitForShutdown();