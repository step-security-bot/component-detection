namespace Microsoft.ComponentDetection.Orchestrator.Commands;

using System.Collections.Generic;
using Microsoft.ComponentDetection.Contracts;
using Microsoft.ComponentDetection.Orchestrator.Services;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;

/// <summary>
/// Lists available detectors.
/// </summary>
public sealed class ListDetectorsCommand : Command
{
    private readonly IEnumerable<IComponentDetector> detectors;
    private readonly ILogger<DetectorListingCommandService> logger;

    public ListDetectorsCommand(
        IEnumerable<IComponentDetector> detectors,
        ILogger<DetectorListingCommandService> logger)
    {
        this.detectors = detectors;
        this.logger = logger;
    }

    /// <inheritdoc/>
    public override int Execute(CommandContext context)
    {
        this.logger.LogInformation("Detectors:");

        foreach (var detector in this.detectors)
        {
            this.logger.LogInformation("{DetectorId}", detector.Id);
        }

        return (int)ProcessingResultCode.Success;
    }
}
