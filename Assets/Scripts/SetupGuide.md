# Multiplayer prototype setup

1. Add **NetworkManager** object to the scene.
2. Add **UnityTransport** component to NetworkManager and assign it in NetworkConfig.
3. Add a player prefab with components:
   - `NetworkObject`
   - `CharacterController`
   - `NetworkTransform`
   - `NetworkPlayerController`
4. Register this prefab into NetworkManager -> Network Prefabs.
5. Add an empty object `Bootstrap` and attach:
   - `SimpleWorldBootstrap`
   - `RoomConnectionUI`
6. Run two game instances:
   - Instance A: Start Host
   - Instance B: Join Room (127.0.0.1:7777)

Result: basic room-based online session with synced player movement and generated world.
