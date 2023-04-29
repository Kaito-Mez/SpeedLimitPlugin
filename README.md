# SpeedLimitPlugin
A plugin for https://github.com/compujuckel/AssettoServer that allows for a server wide top speed restriction.

## Configuration
Enable the plugin in `extra_cfg.yml`
```yaml
EnablePlugins:
- SpeedLimitPlugin
```

Example configuration (add to bottom of `extra_cfg.yml`)  
```yaml
---
!SpeedLimitConfiguration
SpeedLimit: 260
```
Note: SpeedLimit is in km/h, default SpeedLimit is 260
