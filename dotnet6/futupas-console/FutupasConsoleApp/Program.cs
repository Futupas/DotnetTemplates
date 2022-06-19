using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using FutupasConsoleApp;

// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");


Console.InputEncoding = System.Text.Encoding.Unicode;
Console.OutputEncoding = System.Text.Encoding.Unicode;
using (var host = CreateHostBuilder(args).Build())
{
    await host.StartAsync();
}
Environment.Exit(0);




IHostBuilder CreateHostBuilder(string[] args)
{
    var appsettingsPath = ResolveAppsettingsPath(args);
    return Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
            if (appsettingsPath == null)
            {
                Console.WriteLine($"Got default Appsettings file ({Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json")})");
                config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
            }
            else
            {
                Console.WriteLine($"Got command line Appsettings file ({appsettingsPath})");
                config.AddJsonFile(appsettingsPath, optional: false, reloadOnChange: false);
            }
        })
        .ConfigureServices((context, services) =>
        {
            services.AddLogging(builder =>
            {
                builder.AddConfiguration(context.Configuration.GetSection("Logging"));
                builder.AddFile(o => o.RootPath = AppContext.BaseDirectory);
            });
            services.AddHostedService<MainWorker>();
        });
}

/// <returns>Null if default path is not provided, or path if it is.</returns>
string ResolveAppsettingsPath(string[] args)
{
    if (args == null || args.Length < 2) return null;
    for (int i = 0; i < args.Length; i++)
    {
        var arg = args[i].ToLower();
        if (arg == "appsettings" || arg == "--appsettings" || arg == "conf" || arg == "--conf")
        {
            if (i < args.Length - 1)
            {
                var next = args[i + 1].Trim();
                return next.Length > 0 ? next : null;
            }
        }
    }
    return null;
}
