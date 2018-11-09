using Newtonsoft.Json;
using UnityEngine;

public class MainHeroScript : MonoBehaviour {
	private ClientFunctional ClientFunctional;
	private Rigidbody2D Rigidbody2D;
	public float speed = 4;
	private long PlayerId;
	
	void Start () {
		Rigidbody2D = GetComponent<Rigidbody2D>();
		ClientFunctional = new ClientFunctional();
		PlayerId = GetHashCode();
		ClientFunctional.StartClient();
		Debug.Log(Rigidbody2D);
	}
	
	
	void Update () {
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			Rigidbody2D.position =
				new Vector2(Rigidbody2D.position.x - 1 * speed * Time.deltaTime, Rigidbody2D.position.y);

			MovementDto movementDto = new MovementDto();
			
			movementDto.PlayerId = PlayerId;
			movementDto.X = Rigidbody2D.position.x;
			movementDto.Y = Rigidbody2D.position.y;

			var serializeObject = JsonConvert.SerializeObject(movementDto);


			ClientFunctional.SendMessage(EventType.Movement, "test");
		}

		if (Input.GetKey(KeyCode.RightArrow))
		{
			Rigidbody2D.position =
				new Vector2(Rigidbody2D.position.x + 1 * speed * Time.deltaTime, Rigidbody2D.position.y);

			ClientFunctional.SendMessage(EventType.Movement, "test");
		}

		if (Input.GetKeyUp(KeyCode.Space))
		{
			Debug.Log("JUMP");
		}
	}
}
