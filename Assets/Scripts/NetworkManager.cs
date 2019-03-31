using System.IO;
using System.Net.Sockets;
using core.message;
using Newtonsoft.Json;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
	public bool isOffline;

	private NetworkStream _networkStream;

	private StreamReader _streamReader;

	private StreamWriter _streamWriter;

	private TcpClient _tcpClient;

	private Rigidbody2D _mainHeroRigidbody2D;

	private MessageProcessor _messageProcessor;

	private void Start()
    {
	    _mainHeroRigidbody2D = GetComponent<MainHeroController>().HeroRigidbody2D;
	    _messageProcessor = new MessageProcessor();
	    var heroName = GetComponent<MainHeroController>().HeroName;
	    
	    if (isOffline) return;
	    
	    StartClient();
		    
	    SendClientPlayerJoinMessage(heroName);
		
	    InvokeRepeating("DoMultiplayerMovementLogic", 0.0f, 0.03333333333f);
    }

	private void StartClient()
	{
		var tcpClient = new TcpClient();

		tcpClient.Connect("127.0.0.1", 27015);
		
		_networkStream = tcpClient.GetStream();
            
		_streamReader = new StreamReader(_networkStream);
		_streamWriter = new StreamWriter(_networkStream) {AutoFlush = true};
	}

	private void DoMultiplayerMovementLogic()
	{
		SendMainHeroPositionToServer();
		
		while (_networkStream.DataAvailable)
		{
			var messageString = _streamReader.ReadLine();
			if (messageString != null && !messageString.Equals(""))
			{
				_messageProcessor.ProcessMessage(messageString);
			}
		}
	}

	private void SendMainHeroPositionToServer()
	{
		var messageWrapper = new MessageWrapper();
		var playerMoveMessage = new PlayerMoveMessage();
		var heroPosition = _mainHeroRigidbody2D.position;

		playerMoveMessage.X = heroPosition.x;
		playerMoveMessage.Y = heroPosition.y;

		playerMoveMessage.RotationZ = _mainHeroRigidbody2D.transform.rotation.eulerAngles.z;
		
		playerMoveMessage.PlayerId = _mainHeroRigidbody2D.name;

		var playerMovePayload = JsonConvert.SerializeObject(playerMoveMessage);
		messageWrapper.Payload = playerMovePayload;

		var messageWrapperString = JsonConvert.SerializeObject(messageWrapper);

		SendMessageToServer(messageWrapperString);
	}

	private void SendClientPlayerJoinMessage(string heroName)
	{
		var messageWrapper = new MessageWrapper();
		var playerJoinMessage = new PlayerJoinMessage {PlayerId = heroName};
		
		var playerJoinMessagePayload = JsonConvert.SerializeObject(playerJoinMessage);
		
		messageWrapper.Payload = playerJoinMessagePayload;
		messageWrapper.MessageType = MessageType.PlayerJoin;

		var messageWrapperString = JsonConvert.SerializeObject(messageWrapper);
		
		SendMessageToServer(messageWrapperString);
	}
	
	private void SendMessageToServer(string message)
	{
		_streamWriter.WriteLine(message);
	}
}
