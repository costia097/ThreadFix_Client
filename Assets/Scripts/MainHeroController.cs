using System.Globalization;
using UnityEngine;

public class MainHeroController : MonoBehaviour
{
    public Collider2D groundBoxCollider2D;

    public float speed;

    private NetworkManager _networkManager;

    private bool _isWatchToRightDirection = true;

    private SpriteRenderer _spriteRenderer;
    private static readonly int IsDestruct = Animator.StringToHash("isDestruct");

    private void Start()
    {
        HeroRigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _networkManager = GetComponent<NetworkManager>();
        
        name = GetHashCode().ToString();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            HeroRigidbody2D.transform.Translate(Vector2.left * Time.deltaTime * speed);
            _spriteRenderer.flipX = true;
            _isWatchToRightDirection = false;
        }
        
        if (Input.GetKey(KeyCode.RightArrow))
        {
            HeroRigidbody2D.transform.Translate(Vector2.right * Time.deltaTime * speed);
            _spriteRenderer.flipX = false;
            _isWatchToRightDirection = true;
        }
        
        if (Input.GetKeyUp(KeyCode.Space) && HeroRigidbody2D.IsTouching(groundBoxCollider2D))
        {
            HeroRigidbody2D.AddForce(new Vector2(0, 30), ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            var target = GameObject.Find("groundGrassElement");
            var targetAnimator = target.GetComponent<Animator>();
            targetAnimator.SetBool(IsDestruct, true);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            var bombName = PlayerThrowBomb();
            _networkManager.SendPlayerThrowBomb(bombName);
        }
        
        //TODO
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public Rigidbody2D HeroRigidbody2D { get; private set; }

    public bool IsWatchToRightDirection
    {
        get { return _isWatchToRightDirection; }
    }

    private string PlayerThrowBomb()
    {
        var bombGameObjectPrefab = Resources.Load<GameObject>("Prefabs/Bomb");

        var position = transform.position;

        var instantiatedBomb = 
            Instantiate(bombGameObjectPrefab, _isWatchToRightDirection ? new Vector2(position.x + 1, position.y) : new Vector2(position.x - 1, position.y), Quaternion.identity);

        instantiatedBomb.name = Random.value.ToString(CultureInfo.InvariantCulture);
        
        var bombRigidbody2D = instantiatedBomb.GetComponent<Rigidbody2D>();
        
        bombRigidbody2D.AddForce(_isWatchToRightDirection ? new Vector2(100, 300) : new Vector2(-100, 300));

        return instantiatedBomb.name;
    }
}
