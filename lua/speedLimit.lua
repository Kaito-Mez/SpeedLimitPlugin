
local sim = ac.getSim()

local selectedCar = nil ---@type ac.StateCar

local speedLimit = nil ---@type int32

local setSpeedLimit = ac.OnlineEvent({
  key = ac.StructItem.key('setSpeedLimit'),
  targetSessionID = ac.StructItem.int32(),
  newLimit = ac.StructItem.int32()
}, function (sender, message)


  if message.targetSessionID == ac.getCar(0).sessionID then -- if sender is the server

    speedLimit = message.newLimit

  end
end)

local enforceSpeedLimit = ac.OnlineEvent({
  key = ac.StructItem.key('enforceSpeedLimit'),
  targetVelocity = ac.StructItem.vec3(),
  targetSessionID = ac.StructItem.int32()
}, function (sender, message)

  if message.targetSessionID == ac.getCar(0).sessionID then -- if sender is the server


    local targetVelocity = message.targetVelocity

    physics.setCarVelocity(0, targetVelocity)
  end

end)
