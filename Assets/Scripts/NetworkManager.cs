using System.IO;
using System.Net.Sockets;
using core.message;
using core.processor;
using Newtonsoft.Json;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
	private static readonly int IsRunning = Animator.StringToHash("isRunning");
	
	private static readonly int IsSlashing = Animator.StringToHash("isSlashing");
	
	public bool isOffline;

	private NetworkStream _networkStream;

	private StreamReader _streamReader;

	private StreamWriter _streamWriter;

	private TcpClient _tcpClient;

	private Rigidbody2D _mainHeroRigidbody2D;

	private Animator _animator;

	private MainHeroController _mainHeroController;

	private void Start()
    {
	    _mainHeroController = GetComponent<MainHeroController>();
	    _mainHeroRigidbody2D =_mainHeroController.HeroRigidbody2D;
	    _animator = GetComponent<Animator>();
	    
	    var heroName = _mainHeroController.name;
	    
	    if (isOffline) return;
	    
	    StartClient();
		    
	    SendClientPlayerJoinMessage(heroName);
	    
	    // 0.03333333333f
	    // 10.0f

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
		SendMainHeroStateToServer();
		
		while (_networkStream.DataAvailable)
		{
			var messageString = _streamReader.ReadLine();
			if (messageString != null && !messageString.Equals(""))
			{
				MessageProcessor.ProcessMessage(messageString);
			}
		}
	}

	private void SendMainHeroStateToServer()
	{
		var messageWrapper = new MessageWrapper();
		var playerStateMessage = new PlayerStateMessage();
		var heroPosition = _mainHeroRigidbody2D.position;

		playerStateMessage.X = heroPosition.x;
		playerStateMessage.Y = heroPosition.y;

		playerStateMessage.IsWatchToRightDirection = _mainHeroController.IsWatchToRightDirection;

		playerStateMessage.IsRunning = _animator.GetBool(IsRunning);
		playerStateMessage.IsSlashing = _animator.GetBool(IsSlashing);

		playerStateMessage.RotationZ = _mainHeroRigidbody2D.transform.rotation.eulerAngles.z;
		
		playerStateMessage.PlayerId = _mainHeroRigidbody2D.name;

		var playerMovePayload = JsonConvert.SerializeObject(playerStateMessage);
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

	/*
	 * triggers on animation event
	 */
	private void SendPlayerWaveSword()
	{		
		var messageWrapper = new MessageWrapper();

		var isWatchToRightDirection = _mainHeroController.IsWatchToRightDirection;

		var directionMode = isWatchToRightDirection ? Direction.Right : Direction.Left;

		var playerWaveMessage = new PlayerWaveMessage {PlayerName = name, Direction = directionMode};

		var playerWaveMessagePayload = JsonConvert.SerializeObject(playerWaveMessage);

		messageWrapper.Payload = playerWaveMessagePayload;
		messageWrapper.MessageType = MessageType.PlayerWave;

		var messageWrapperString = JsonConvert.SerializeObject(messageWrapper);

		Debug.Log("SEND playerWaveMessage " + playerWaveMessage);

		SendMessageToServer(messageWrapperString);
	}
	
	private void SendMessageToServer(string message)
	{
		_streamWriter.WriteLine(message);
	}
}
