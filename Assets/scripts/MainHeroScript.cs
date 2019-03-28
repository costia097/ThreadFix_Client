using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Sockets;
using core;
using core.message;
using Newtonsoft.Json;
using UnityEngine;
using Random = System.Random;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "ArrangeTypeMemberModifiers")]
[SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Local")]
public class MainHeroScript : MonoBehaviour {
	
	private Rigidbody2D heroRigidbody2d;
	
	public float speed = 4;

	public bool isOffline;

	public bool isDebugMode;

	public BoxCollider2D groundBoxCollider2d;
	
	private NetworkStream networkStream;
	
	private StreamReader streamReader;
	
	private StreamWriter streamWriter;
	
	private bool isGameActive;

	private List<ServerPlayer> players;
	
	void Start ()
	{
		var random = new Random();
		heroRigidbody2d = GetComponent<Rigidbody2D>();

		heroRigidbody2d.name = random.Next().ToString();

		isGameActive = true;

		players = new List<ServerPlayer>();

		if (!isOffline)
		{
			startClient();
		}

		sendClientPlayerJoinMessage();
		
		//0.0151f

		InvokeRepeating("doMultiplayerMovementLogic", 0.0f, 0.0333f);
	}

	private void doMultiplayerMovementLogic()
	{
		sendMainHeroPositionToServer();
		
		while (networkStream.DataAvailable)
		{
			var messageString = streamReader.ReadLine();
			if (messageString != null && !messageString.Equals(""))
			{
				processMessage(messageString);
			}
		}
	}

	private void sendMainHeroPositionToServer()
	{
		var messageWrapper = new MessageWrapper();
		var playerMoveMessage = new PlayerMoveMessage();
		var heroPosition = heroRigidbody2d.position;

		playerMoveMessage.x = (float)Math.Round(heroPosition.x, 2);
		playerMoveMessage.y =(float)Math.Round(heroPosition.y, 2);
		playerMoveMessage.playerId = heroRigidbody2d.name;

		var playerMovePayload = JsonConvert.SerializeObject(playerMoveMessage);
		messageWrapper.payload = playerMovePayload;

		var messageWrapperString = JsonConvert.SerializeObject(messageWrapper);

		sendMessageToServer(messageWrapperString);
	}

	private void sendClientPlayerJoinMessage()
	{
		var messageWrapper = new MessageWrapper();
		var playerJoinMessage = new PlayerJoinMessage {playerId = heroRigidbody2d.name};
		var playerJoinMessagePayload = JsonConvert.SerializeObject(playerJoinMessage);
		
		messageWrapper.payload = playerJoinMessagePayload;
		messageWrapper.messageType = MessageType.PLAYER_JOIN;

		var messageWrapperString = JsonConvert.SerializeObject(messageWrapper);
		
		sendMessageToServer(messageWrapperString);
	}

	private void processMessage(string message)
	{
		var messageWrapper = JsonConvert.DeserializeObject<MessageWrapper>(message);

		switch (messageWrapper.messageType)
		{
			case MessageType.PLAYER_JOIN:
				var playerJoinMessage = JsonConvert.DeserializeObject<PlayerJoinMessage>(messageWrapper.payload);
				processPlayerJoinMessage(playerJoinMessage);
				break;
			case MessageType.PLAYER_MOVE:
				var playerMoveMessage = JsonConvert.DeserializeObject<PlayerMoveMessage>(messageWrapper.payload);
				processPlayerMoveMessage(playerMoveMessage);
				break;
			case MessageType.FIRST_SYNC:
				var serverPlayers = JsonConvert.DeserializeObject<List<ServerPlayer>>(messageWrapper.payload);
				var firstSyncMessage = new FirstSyncMessage();
				firstSyncMessage.players = serverPlayers;
				processFirstSyncMessage(firstSyncMessage);
				break;
			default:
				throw new UnityException();
		}
	}

	private void processPlayerJoinMessage(PlayerJoinMessage playerJoinMessage)
	{
		var prefab = Resources.Load<GameObject>("Prefabs/Hero4");
		var instantiate = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
		instantiate.name = playerJoinMessage.playerId;
		var serverPlayer = new ServerPlayer {x = 0, y = 0, name = playerJoinMessage.playerId};
		players.Add(serverPlayer);
	}

	private void processPlayerMoveMessage(PlayerMoveMessage playerMoveMessage)
	{
		foreach (var serverPlayer in players)
		{
			if (serverPlayer.name.Equals(playerMoveMessage.playerId))
			{
				var targetPlayer = GameObject.Find(playerMoveMessage.playerId);
				if (targetPlayer != null)
				{
					var targetTransformPlayer = targetPlayer.GetComponent<Transform>();
					targetTransformPlayer.position = new Vector3(playerMoveMessage.x, playerMoveMessage.y);
				}	
			}
		}
	}

	private void processFirstSyncMessage(FirstSyncMessage firstSyncMessage)
	{
		var serverPlayers = firstSyncMessage.players;
		
		foreach (var serverPlayer in serverPlayers)
		{
			var prefab = Resources.Load<GameObject>("Prefabs/Hero4");
			var instantiate = Instantiate(prefab, new Vector3(serverPlayer.x, serverPlayer.y, serverPlayer.z), Quaternion.identity);
			instantiate.name = serverPlayer.name;
		}

		players = serverPlayers;
	}

	private void startClient()
	{
		var tcpClient = new TcpClient();
		tcpClient.Connect("127.0.0.1", 27015);
		
		networkStream = tcpClient.GetStream();
            
		streamReader = new StreamReader(networkStream);
		streamWriter = new StreamWriter(networkStream) {AutoFlush = true};
	}

	private void sendMessageToServer(string message)
	{
		streamWriter.WriteLine(message);
	}
	
	void Update () {
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			var position = heroRigidbody2d.position;
			
			position = new Vector2(position.x - 1 * speed * Time.deltaTime, position.y);
			heroRigidbody2d.position = position;
		}

		if (Input.GetKey(KeyCode.RightArrow))
		{
			var position = heroRigidbody2d.position;
			heroRigidbody2d.position = new Vector2(position.x + 1 * speed * Time.deltaTime, position.y);
		}

		if (Input.GetKeyUp(KeyCode.Space) && heroRigidbody2d.IsTouching(groundBoxCollider2d))
		{
			heroRigidbody2d.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
		}

		if (Input.GetKeyUp(KeyCode.Escape))
		{
			isGameActive = false;
			Application.Quit();
		}
	}
	void FixedUpdate()
	{
	}
}
