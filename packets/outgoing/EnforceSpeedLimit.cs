using AssettoServer.Network.Packets.Outgoing;
using AssettoServer.Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace SpeedLimitPlugin.packets.outgoing
{
    public class EnforceSpeedLimit : IOutgoingNetworkPacket
    {
        public int Index { get; set; }
        public Vector3 TargetVelocity;
        public void ToWriter(ref PacketWriter writer)
        {
            writer.Write((byte)ACServerProtocol.Extended);
            writer.Write((byte)CSPMessageTypeTcp.ClientMessage);
            writer.Write<byte>(255);
            writer.Write<ushort>(60000);
            writer.Write(0x2F19032B);
            writer.Write(TargetVelocity);
            writer.Write(Index);
        }
    }
}
