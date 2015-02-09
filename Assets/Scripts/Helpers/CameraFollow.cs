using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float smoothRate = 1.5f;        // How much dampening to apply to camera movement
    private float panDistance = 3f;         // How much the player can pan the camera
    private float vertical;                 // The players vertical input value
    private float horizontalDistance = 5f;  // The distance the camera pans horizontally
    private bool facingLeft;                // The direction the player is facing horizontally
    private Transform player;               // Reference to the player's transform
    private Vector2 velocity;               // Velocity of camera follow

    void OnLevelWasLoaded(int level)
    {
        if(level == 0)
        {
            GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
            camera.transform.position = new Vector3(0, 0, camera.transform.position.z);
        }
        else
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        velocity = new Vector2(0.5f, 0.5f);
    }

    void Update()
    {
        if (!GameManager.Instance.isPaused)
        {
            facingLeft = player.GetComponent<PlayerInput>().lastDirection.x < 0;
            vertical = Input.GetAxis("Vertical") * panDistance;
        }
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.isPaused)
        {
            Vector2 newPos2D = Vector2.zero;

            if(facingLeft)
            {
                newPos2D.x = Mathf.SmoothDamp(transform.position.x, player.position.x + -horizontalDistance, ref velocity.x, smoothRate);
            }
            else
            {
                newPos2D.x = Mathf.SmoothDamp(transform.position.x, player.position.x + horizontalDistance, ref velocity.x, smoothRate);
            }
            newPos2D.y = Mathf.SmoothDamp(transform.position.y, player.position.y + vertical, ref velocity.y, smoothRate);

            Vector3 newPos = new Vector3(newPos2D.x, newPos2D.y, transform.position.z);
            transform.position = Vector3.Slerp(transform.position, newPos, Time.time);
        }
    }

    public void Initialize()
    {
        facingLeft = player.GetComponent<PlayerInput>().lastDirection.x < 0;
        if (facingLeft)
        {
            camera.transform.position = new Vector3(player.position.x + -horizontalDistance, player.position.y, camera.transform.position.z);
        }
        else
        {
            camera.transform.position = new Vector3(player.position.x + horizontalDistance, player.position.y, camera.transform.position.z);
        }
    }

    public void SetPosition(Vector2 position)
    {
        camera.transform.position = new Vector3(position.x, position.y, camera.transform.position.z);
    }
}