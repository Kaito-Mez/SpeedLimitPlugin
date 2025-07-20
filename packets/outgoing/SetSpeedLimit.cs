using AssettoServer.Shared.Network.Packets.Outgoing;
using AssettoServer.Shared.Network.Packets;

namespace SpeedLimitPlugin.packets.outgoing
{
    public class SetSpeedLimit : IOutgoingNetworkPacket
    {
        public int Index { get; set; }
        public int SpeedLimit;
        public void ToWriter(ref PacketWriter writer)
        {
            writer.Write((byte)ACServerProtocol.Extended);
            writer.Write((byte)CSPMessageTypeTcp.ClientMessage);
            writer.Write<byte>(255);
            writer.Write<ushort>(60000);
            writer.Write(0xDD8FBFDC);
            writer.Write(SpeedLimit);
            writer.Write(Index);
        }
    }
}

