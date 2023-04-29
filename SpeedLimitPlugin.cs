using AssettoServer.Server;
using AssettoServer.Server.Plugin;
using AssettoServer.Utils;
using Microsoft.Extensions.Hosting;

namespace SpeedLimitPlugin;

public class SpeedLimitPlugin : CriticalBackgroundService, IAssettoServerAutostart
{
    private readonly EntryCarManager _entryCarManager;
    private readonly Func<EntryCar, EntryCarSpeedManager> _entryCarRaceFactory;
    private readonly Dictionary<int, EntryCarSpeedManager> _instances = new();
    
    public SpeedLimitPlugin(EntryCarManager entryCarManager, Func<EntryCar, EntryCarSpeedManager> entryCarRaceFactory, IHostApplicationLifetime applicationLifetime) : base(applicationLifetime)
    {
        _entryCarManager = entryCarManager;
        _entryCarRaceFactory = entryCarRaceFactory;
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
