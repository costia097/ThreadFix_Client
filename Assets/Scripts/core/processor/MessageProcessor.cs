using System.Collections.Generic;
using core.entities;
using core.message;
using Newtonsoft.Json;
using UnityEngine;

namespace core.processor
{
    public static class MessageProcessor
    {
    
        public static void ProcessMessage(string message)
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
                case MessageType.FirstPlayersSync:
                    var serverPlayers = JsonConvert.DeserializeObject<List<ServerPlayer>>(messageWrapper.Payload);
                    var firstPlayersSyncMessage = new FirstPlayersSyncMessage();
                    firstPlayersSyncMessage.Players = serverPlayers;
                    ProcessFirstPlayersSyncMessage(firstPlayersSyncMessage);
                    break;
                case MessageType.FirstEnemiesSync:
                    var enemies = JsonConvert.DeserializeObject<List<Enemy>>(messageWrapper.Payload);
                    var firstEnemiesSyncMessage = new FirstEnemiesSyncMessage();
                    firstEnemiesSyncMessage.Enemies = enemies;
                    ProcessFirstEnemiesSyncMessage(firstEnemiesSyncMessage);
                    break;
                case MessageType.EnemyState:
                    var enemyStateMessage = JsonConvert.DeserializeObject<EnemyStateMessage>(messageWrapper.Payload);
                    ProcessEnemyStateMessage(enemyStateMessage);
                    break;
                case MessageType.PlayerWave:
                    var playerWaveMessage = JsonConvert.DeserializeObject<PlayerWaveMessage>(messageWrapper.Payload);
                    ProcessPlayerWaveMessage(playerWaveMessage);
                    break;
                default:
                    throw new UnityException();
            }
        }
    
        private static void ProcessPlayerJoinMessage(PlayerJoinMessage playerJoinMessage)
        {
            var prefab = Resources.Load<GameObject>("Prefabs/MainHero");
            var instantiate = Object.Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            instantiate.name = playerJoinMessage.PlayerId;
        }

        private static void ProcessPlayerStateMessage(PlayerStateMessage playerStateMessage)
        {
            var targetPlayer = GameObject.Find(playerStateMessage.PlayerId);
            
            if (targetPlayer == null) return;
            
            var targetTransformPlayer = targetPlayer.GetComponent<Transform>();

            var targetPosition = new Vector2(playerStateMessage.X, playerStateMessage.Y);

            TryToDoSmoothy(targetTransformPlayer, targetPosition);
            
            targetTransformPlayer.rotation = Quaternion.Euler(new Vector3(playerStateMessage.RotationX,
                playerStateMessage.RotationY, playerStateMessage.RotationZ));
        }

        private static void ProcessEnemyStateMessage(EnemyStateMessage enemyStateMessage)
        {
            var targetEnemy = GameObject.Find(enemyStateMessage.Name);
            if (targetEnemy == null) return;

            var targetEnemyTransform = targetEnemy.GetComponent<Transform>();

            targetEnemyTransform.position = new Vector2(enemyStateMessage.X, enemyStateMessage.Y);
        }

        private static void ProcessPlayerWaveMessage(PlayerWaveMessage playerWaveMessage)
        {
            var targetPlayer = GameObject.Find(playerWaveMessage.PlayerName);
            if (targetPlayer == null) return;

            var targetPlayerAnimator = targetPlayer.GetComponent<Animator>();

            targetPlayerAnimator.Play("Slashing");
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

        private static void ProcessFirstPlayersSyncMessage(FirstPlayersSyncMessage firstPlayersSyncMessage)
        {
            var serverPlayers = firstPlayersSyncMessage.Players;
		
            foreach (var serverPlayer in serverPlayers)
            {
                var prefab = Resources.Load<GameObject>("Prefabs/MainHero");
                var instantiate = Object.Instantiate(prefab, new Vector3(serverPlayer.X, serverPlayer.Y, serverPlayer.Z), Quaternion.identity);
                instantiate.name = serverPlayer.Name;
            }
        }

        private static void ProcessFirstEnemiesSyncMessage(FirstEnemiesSyncMessage enemiesSyncMessage)
        {
            var serverEnemies = enemiesSyncMessage.Enemies;
            
            serverEnemies.ForEach(enemy =>
            {
                var enemyPrefab = Resources.Load<GameObject>("Prefabs/Hero4");
                var enemyInstantiate =
                    Object.Instantiate(enemyPrefab, new Vector3(enemy.X, enemy.Y, 0), Quaternion.identity);
                enemyInstantiate.name = enemy.Name;
            });
        }
        
        
    }
}