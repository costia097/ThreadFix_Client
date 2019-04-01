using System.Collections.Generic;
using core.message;
using Newtonsoft.Json;
using UnityEngine;

namespace core.processor
{
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
                case MessageType.PlayerState:
                    var playerMoveMessage = JsonConvert.DeserializeObject<PlayerStateMessage>(messageWrapper.Payload);
                    ProcessPlayerStateMessage(playerMoveMessage);
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
            var prefab = Resources.Load<GameObject>("Prefabs/MainHero");
            var instantiate = Object.Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            instantiate.name = playerJoinMessage.PlayerId;
            var serverPlayer = new ServerPlayer {X = 0, Y = 0, Z = 0, Name = playerJoinMessage.PlayerId};
        
            _players.Add(serverPlayer);
        }

        private void ProcessPlayerStateMessage(PlayerStateMessage playerStateMessage)
        {
            foreach (var serverPlayer in _players)
            {
                if (!serverPlayer.Name.Equals(playerStateMessage.PlayerId)) continue;
            
                var targetPlayer = GameObject.Find(playerStateMessage.PlayerId);
            
                if (targetPlayer == null) continue;
            
                var targetTransformPlayer = targetPlayer.GetComponent<Transform>();

                var targetPosition = new Vector2(playerStateMessage.X, playerStateMessage.Y);

                TryToDoSmoothy(targetTransformPlayer, targetPosition);
            
                targetTransformPlayer.rotation = Quaternion.Euler(new Vector3(playerStateMessage.RotationX,
                    playerStateMessage.RotationY, playerStateMessage.RotationZ));
            }
        }

        //TODO in progress
        
        private static void TryToDoSmoothy(Transform targetPlayerTransform, Vector2 targetPosition)
        {

            var currentPosition = targetPlayerTransform.position;
            
            for (var i = 0; i < 10; i++)
            {
                var localPosition = Vector2.Lerp(currentPosition, targetPosition, i / 10f);
                targetPlayerTransform.position = localPosition;
            }
        }

        private void ProcessFirstSyncMessage(FirstSyncMessage firstSyncMessage)
        {
            var serverPlayers = firstSyncMessage.Players;
		
            foreach (var serverPlayer in serverPlayers)
            {
                var prefab = Resources.Load<GameObject>("Prefabs/MainHero");
                var instantiate = Object.Instantiate(prefab, new Vector3(serverPlayer.X, serverPlayer.Y, serverPlayer.Z), Quaternion.identity);
                instantiate.name = serverPlayer.Name;
            }

            _players = serverPlayers;
        }
        
    }
}