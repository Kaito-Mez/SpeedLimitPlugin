using AssettoServer.Server.Plugin;
using Autofac;

namespace SpeedLimitPlugin;

public class SpeedLimitModule : AssettoServerModule<SpeedLimitConfiguration>
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<SpeedLimitPlugin>().AsSelf().As<IAssettoServerAutostart>().SingleInstance();
        builder.RegisterType<EntryCarSpeedManager>().AsSelf();
    }
}
