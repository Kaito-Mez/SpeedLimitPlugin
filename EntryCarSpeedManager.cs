using System.Numerics;
using AssettoServer.Shared.Network.Packets.Incoming;
using AssettoServer.Network.Tcp;
using AssettoServer.Server;
using SpeedLimitPlugin.packets.outgoing;

namespace SpeedLimitPlugin;

public class EntryCarSpeedManager
{
    private readonly EntryCar _entryCar;
    private readonly EntryCarManager _entryCarManager;
    private readonly int _speedLimitKmh;
    private int c = 0;


    public EntryCarSpeedManager(SpeedLimitConfiguration speedLimitConfiguration, EntryCar entryCar, EntryCarManager entryCarManager)
    {
        _entryCar = entryCar;
        _entryCarManager = entryCarManager;
        _speedLimitKmh = speedLimitConfiguration.SpeedLimit;
        _entryCarManager.ClientConnected += OnClientConnected;
        _entryCar.PositionUpdateReceived += OnPositionUpdateReceived;
    }

    private void OnClientConnected(ACTcpClient client, EventArgs args)
    {
        if (client.EntryCar.Equals(_entryCar))
        {
            client.FirstUpdateSent += InitializeSpeedLimit;
            client.LapCompleted += InitializeSpeedLimit;
        }
    }

    private void InitializeSpeedLimit(ACTcpClient client, EventArgs args)
    {
        client.SendPacket(new SetSpeedLimit
        {
            Index = client.SessionId,
            SpeedLimit = _speedLimitKmh
        });
    }

    private void OnPositionUpdateReceived(EntryCar sender, in PositionUpdateIn positionUpdate)
    {
        if (sender == _entryCar)
        {
            c++;

            // Check speeds every 3 ticks
            if (c == 3)
            {

                float absSpeed = sender.Status.Velocity.Length(); 

                if (ConvertACToKmh(absSpeed) > _speedLimitKmh)
                {
                    Vector3 scaledVelocity = Vector3.Normalize(_entryCar.Status.Velocity) * (ConvertKmhToAC(_speedLimitKmh));
                    _entryCar.Client?.SendPacket(new EnforceSpeedLimit
                    {
                        Index = sender.SessionId,
                        TargetVelocity = scaledVelocity
                    });
                }

                c = 0;
            }
        }

    }


    public int ConvertACToKmh(float acSpeed)
    {
        return (int)(acSpeed * 3.6);
    }

    public float ConvertKmhToAC(int kmH)
    {
        return (float)(kmH / 3.6);
    }
}
