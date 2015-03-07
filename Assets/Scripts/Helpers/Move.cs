using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed;
    public Vector2 movement;

    void Update()
    {
        if (!GameManager.Instance.isPaused)
        {
            movement *= speed;
            movement = Vector2.ClampMagnitude(movement, speed);

            GetComponent<Rigidbody2D>().velocity = movement;
        }
    }
}