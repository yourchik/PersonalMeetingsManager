using Microsoft.Extensions.Hosting;
using PersonalMeetingsManager;

var host = Startup.CreateHostBuilder(args).Build();
host.Run();

while (true)
{
    Console.WriteLine("Hello World!");
}