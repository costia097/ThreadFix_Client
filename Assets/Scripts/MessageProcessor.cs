using System.Collections.Generic;
using core;
using core.message;
using Newtonsoft.Json;
using UnityEngine;

public class MessageProcessor
{
    
    private List<ServerPlayer> _players = new List<ServerPlayer>();
    
    public void ProcessMessage(string message)
    {
        var messageWrapper = JsonConvert.DeserializeObject<MessageWrapper>(message);

        switch (messageWrapper.MessageType)
        {
            case MessageType.PlayerJoin:
                var playerJoinMessage = JsonConvert.DeserializeObject<PlayerJoinMessage>(messageWrapper.Payload);
                ProcessPlayerJoinMessage(playerJoinMessage);
                break;
            case MessageType.PlayerMove:
                var playerMoveMessage = JsonConvert.DeserializeObject<PlayerMoveMessage>(messageWrapper.Payload);
                ProcessPlayerMoveMessage(playerMoveMessage);
                break;
            case MessageType.FirstSync:
                var serverPlayers = JsonConvert.DeserializeObject<List<ServerPlayer>>(messageWrapper.Payload);
                var firstSyncMessage = new FirstSyncMessage();
                firstSyncMessage.Players = serverPlayers;
                ProcessFirstSyncMessage(firstSyncMessage);
                break;
            default:
                throw new UnityException();
        }
    }
    
    private void ProcessPlayerJoinMessage(PlayerJoinMessage playerJoinMessage)
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Hero4-Authres");
        var instantiate = Object.Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        instantiate.name = playerJoinMessage.PlayerId;
        var serverPlayer = new ServerPlayer {X = 0, Y = 0, Z = 0, Name = playerJoinMessage.PlayerId};
        
        _players.Add(serverPlayer);
    }

    private void ProcessPlayerMoveMessage(PlayerMoveMessage playerMoveMessage)
    {
        foreach (var serverPlayer in _players)
        {
            if (!serverPlayer.Name.Equals(playerMoveMessage.PlayerId)) continue;
            
            var targetPlayer = GameObject.Find(playerMoveMessage.PlayerId);
            
            if (targetPlayer == null) continue;
            
            var targetTransformPlayer = targetPlayer.GetComponent<Transform>();
            
            targetTransformPlayer.position = new Vector2(playerMoveMessage.X, playerMoveMessage.Y);
            
            targetTransformPlayer.rotation = Quaternion.Euler(new Vector3(playerMoveMessage.RotationX,
                playerMoveMessage.RotationY, playerMoveMessage.RotationZ));
        }
    }

    private void ProcessFirstSyncMessage(FirstSyncMessage firstSyncMessage)
    {
        var serverPlayers = firstSyncMessage.Players;
		
        foreach (var serverPlayer in serverPlayers)
        {
            var prefab = Resources.Load<GameObject>("Prefabs/Hero4-Authres");
            var instantiate = Object.Instantiate(prefab, new Vector3(serverPlayer.X, serverPlayer.Y, serverPlayer.Z), Quaternion.identity);
            instantiate.name = serverPlayer.Name;
        }

        _players = serverPlayers;
    }
        
}