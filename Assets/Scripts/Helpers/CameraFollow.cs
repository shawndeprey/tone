using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float smoothRate = 0.5f;    // How much dampening to apply to camera movement
    private float panDistance = 3f;     // How much the player can pan the camera
    private float horizontal, vertical; // Holds player input values
    private Transform player;           // Reference to the player's transform
    private Vector2 velocity;           // Velocity of camera follow

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
            GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
            camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, camera.transform.position.z);
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
            horizontal = Input.GetAxis("Horizontal") * panDistance;
            vertical = Input.GetAxis("Vertical") * panDistance;
        }
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.isPaused)
        {
            Vector2 newPos2D = Vector2.zero;

            newPos2D.x = Mathf.SmoothDamp(transform.position.x, player.position.x + horizontal, ref velocity.x, smoothRate);
            newPos2D.y = Mathf.SmoothDamp(transform.position.y, player.position.y + vertical, ref velocity.y, smoothRate);

            Vector3 newPos = new Vector3(newPos2D.x, newPos2D.y, transform.position.z);
            transform.position = Vector3.Slerp(transform.position, newPos, Time.time);
        }
    }
}