using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PersonalMeetingsManager;
using PersonalMeetingsManager.Services.Interface;
using System.Runtime.InteropServices;

var host = Startup.CreateHostBuilder(args).Build();

var menu = host.Services.GetService<IMenu>();

menu?.ShowMenu();

host.WaitForShutdown();

