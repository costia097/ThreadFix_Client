using UnityEngine;

public class MainHeroController : MonoBehaviour
{
    public Collider2D groundBoxCollider2D;

    public float speed;

    private void Start()
    {
        HeroRigidbody2D = GetComponent<Rigidbody2D>();
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
        
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public Rigidbody2D HeroRigidbody2D { get; private set; }
}
