﻿using AssettoServer.Server;
using AssettoServer.Shared.Services;
using AssettoServer.Server.Configuration;
using AssettoServer.Server.Plugin;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Reflection;

namespace SpeedLimitPlugin;

public class SpeedLimitPlugin : CriticalBackgroundService, IAssettoServerAutostart
{
    private readonly EntryCarManager _entryCarManager;
    private readonly Func<EntryCar, EntryCarSpeedManager> _entryCarRaceFactory;
    private readonly Dictionary<int, EntryCarSpeedManager> _instances = new();
    private readonly ACServerConfiguration _serverConfiguration;

    public SpeedLimitPlugin(SpeedLimitConfiguration speedLimitConfiguration,
        EntryCarManager entryCarManager,
        ACServerConfiguration serverConfiguration,
        CSPServerScriptProvider scriptProvider,
        Func<EntryCar, EntryCarSpeedManager> entryCarRaceFactory, 
        IHostApplicationLifetime applicationLifetime) : base(applicationLifetime)
    {
        _entryCarManager = entryCarManager;
        _entryCarRaceFactory = entryCarRaceFactory;
        _serverConfiguration = serverConfiguration;

        if (_serverConfiguration.Extra.EnableClientMessages)
        {
            using var streamReader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("SpeedLimitPlugin.lua.speedLimit.lua")!);
            scriptProvider.AddScript(streamReader.ReadToEnd(), "speedLimit.lua");
            Log.Information("Server Speed Limit Set To: {SpeedLimit}Km/h", speedLimitConfiguration.SpeedLimit);
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        foreach (var entryCar in _entryCarManager.EntryCars)
        {
            _instances.Add(entryCar.SessionId, _entryCarRaceFactory(entryCar));
        }

        return Task.CompletedTask;
    }
    
    internal EntryCarSpeedManager GetRace(EntryCar entryCar) => _instances[entryCar.SessionId];
}
