using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using UnityEngine;

[SuppressMessage("ReSharper", "InconsistentNaming")]
[SuppressMessage("ReSharper", "ArrangeTypeMemberModifiers")]
public class MainHeroScript : MonoBehaviour {
	
	private ClientFunctional clientFunctional;
	
	private Rigidbody2D heroRigidbody2d;
	
	public float speed = 4;

	public bool isOffline;

	public BoxCollider2D groundBoxCollider2d;
	
	private long playerId;
	
	
	void Start () {
		heroRigidbody2d = GetComponent<Rigidbody2D>();
		clientFunctional = new ClientFunctional();
		
		playerId = GetHashCode();

		if (!isOffline)
		{
			clientFunctional.StartClient();
		}
		
		Debug.Log(heroRigidbody2d);
	}
	
	
	void Update () {
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			var position = heroRigidbody2d.position;
			
			position = new Vector2(position.x - 1 * speed * Time.deltaTime, position.y);
			heroRigidbody2d.position = position;

			var movementDto = new MovementDto {playerId = playerId, x = position.x, y = position.y};

			var serializeObject = JsonConvert.SerializeObject(movementDto);

			Debug.Log(serializeObject);
		}

		if (Input.GetKey(KeyCode.RightArrow))
		{
			var position = heroRigidbody2d.position;
			heroRigidbody2d.position = new Vector2(position.x + 1 * speed * Time.deltaTime, position.y);
				
			var movementDto = new MovementDto {playerId = playerId, x = position.x, y = position.y};

			var serializeObject = JsonConvert.SerializeObject(movementDto);
			
			Debug.Log(serializeObject);
		}

		if (Input.GetKeyUp(KeyCode.Space) && heroRigidbody2d.IsTouching(groundBoxCollider2d))
		{
			heroRigidbody2d.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
			Debug.Log("JUMP");
		}
	
	}
}
