using UnityEngine;

public class MainHeroController : MonoBehaviour
{
    public Collider2D groundBoxCollider2D;

    public float speed;

    private NetworkManager _networkManager;

    private void Start()
    {
        HeroRigidbody2D = GetComponent<Rigidbody2D>();
        _networkManager = GetComponent<NetworkManager>();
        
        name = GetHashCode().ToString();
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
        
        if (Input.GetKeyUp(KeyCode.Space) && HeroRigidbody2D.IsTouching(groundBoxCollider2D))
        {
            HeroRigidbody2D.AddForce(new Vector2(0, 30), ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            _networkManager.SendPlayerWaveSword();
        }
        
        //TODO
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public Rigidbody2D HeroRigidbody2D { get; private set; }
}
