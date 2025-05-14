using Serilog;
using System.Reflection;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            host.WaitForShutdownAsync();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args).ConfigureAppConfiguration(del =>
                {
                    del.AddJsonFile($"{Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)}/appsettings.local.json", false, true);
                }).UseSerilog()
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    });
    }
}