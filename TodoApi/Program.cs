using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Formatting.Json;
using Datadog.Trace;
using Datadog.Trace.Configuration;


namespace TodoApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Tracer.Configure(TracerSettings.FromDefaultSources());
            
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(new JsonFormatter(renderMessage: true))
                .CreateLogger();

            var builder = CreateHostBuilder(args);
            var app = builder.Build();
            app.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls("http://*:5000");
                webBuilder.UseStartup<Startup>();
            });
    }
}
