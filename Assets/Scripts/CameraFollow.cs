using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float smoothRate = 0.5f;    // How much dampening to apply to camera movement
    private float panDistance = 3f;     // How much the player can pan the camera
    private float horizontal, vertical; // Holds player input values
    private Transform player;           // Reference to the player's transform
    private Transform thisTransform;    // Cached version of transform
    private Vector2 velocity;           // Velocity of camera follow

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        thisTransform = transform;
        velocity = new Vector2(0.5f, 0.5f);
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal") * panDistance;
        vertical = Input.GetAxis("Vertical") * panDistance;
    }

    void FixedUpdate()
    {
        Vector2 newPos2D = Vector2.zero;

        newPos2D.x = Mathf.SmoothDamp(thisTransform.position.x, player.position.x + horizontal, ref velocity.x, smoothRate);
        newPos2D.y = Mathf.SmoothDamp(thisTransform.position.y, player.position.y + vertical, ref velocity.y, smoothRate);

        Vector3 newPos = new Vector3(newPos2D.x, newPos2D.y, transform.position.z);
        transform.position = Vector3.Slerp(transform.position, newPos, Time.time);
    }
}