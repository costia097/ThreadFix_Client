using UnityEngine;

public class MainHeroController : MonoBehaviour
{
    public Collider2D groundBoxCollider2D;

    public float speed;

    private void Start()
    {
        HeroRigidbody2D = GetComponent<Rigidbody2D>();
        HeroName = new Random().ToString();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            HeroRigidbody2D.transform.Translate(Vector2.left * Time.deltaTime * speed);
        }
        
        if (Input.GetKey(KeyCode.RightArrow))
        {
            HeroRigidbody2D.transform.Translate(Vector2.right * Time.deltaTime * speed);
        }
        
        if (Input.GetKey(KeyCode.Q))
        {
            var currentPosition = HeroRigidbody2D.transform.position;
            HeroRigidbody2D.transform.position = Vector2.Lerp(currentPosition,
                new Vector2(currentPosition.x - 1, currentPosition.y), Time.deltaTime * speed);
        }
        
        if (Input.GetKey(KeyCode.E))
        {
            var currentPosition = HeroRigidbody2D.transform.position;
            HeroRigidbody2D.transform.position = Vector2.Lerp(currentPosition,
                new Vector2(currentPosition.x + 1, currentPosition.y), Time.deltaTime * speed);
        }
        
        if (Input.GetKeyUp(KeyCode.Space) && HeroRigidbody2D.IsTouching(groundBoxCollider2D))
        {
            HeroRigidbody2D.AddForce(new Vector2(0, 30), ForceMode2D.Impulse);
        }
        
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public Rigidbody2D HeroRigidbody2D { get; private set; }

    public string HeroName { get; private set; }
}
