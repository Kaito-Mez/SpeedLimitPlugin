using System.Numerics;
using AssettoServer.Network.Packets.Incoming;
using AssettoServer.Network.Packets.Outgoing;
using AssettoServer.Network.Packets.Shared;
using AssettoServer.Network.Tcp;
using AssettoServer.Server;

namespace SpeedLimitPlugin;

public class EntryCarSpeedManager
{
    private readonly EntryCar _entryCar;
    

    public EntryCarSpeedManager(EntryCar entryCar)
    {
        _entryCar = entryCar;
        _entryCar.PositionUpdateReceived += OnPositionUpdateReceived;
    }


    private void OnPositionUpdateReceived(EntryCar sender, in PositionUpdateIn positionUpdate)
    {
    }

}
