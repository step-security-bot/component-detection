using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ComponentDetection.Orchestrator.Commands;
using Microsoft.ComponentDetection.Orchestrator.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Extensions.DependencyInjection;

if (args.Contains("--debug", StringComparer.OrdinalIgnoreCase))
{
    Console.WriteLine($"Waiting for debugger attach. PID: {Process.GetCurrentProcess().Id}");
    while (!Debugger.IsAttached)
    {
        await Task.Delay(1000);
    }
}

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate: "[BOOTSTRAP] [{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateBootstrapLogger();

// var serviceProvider = new ServiceCollection()
//     .AddComponentDetection()
//     .ConfigureLoggingProviders()
//     .BuildServiceProvider();
// var orchestrator = serviceProvider.GetRequiredService<Orchestrator>();
// var result = await orchestrator.LoadAsync(args);
var serviceCollection = new ServiceCollection()
    .AddComponentDetection()
    .ConfigureLoggingProviders();
using var registrar = new DependencyInjectionRegistrar(serviceCollection);
var app = new CommandApp<ListDetectorsCommand>(registrar);
app.Configure(
    config =>
    {
        config.Settings.ApplicationName = "component-detection";
        config.CaseSensitivity(CaseSensitivity.None);

        config.AddCommand<ListDetectorsCommand>("list-detectors")
            .WithDescription("Lists available detectors");
    });
return await app.RunAsync(args).ConfigureAwait(false);

// var exitCode = (int)result.ResultCode;
// if (result.ResultCode is ProcessingResultCode.Error or ProcessingResultCode.InputError)
// {
//     exitCode = -1;
// }
//
// Console.WriteLine($"Execution finished, status: {exitCode}.");
//
// // Manually dispose to flush logs as we force exit
// await serviceProvider.DisposeAsync();
//
// await Log.CloseAndFlushAsync();
//
// // force an exit, not letting any lingering threads not responding.
// Environment.Exit(exitCode);
