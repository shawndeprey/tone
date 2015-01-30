using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed;
    public Vector2 movement;

    void Update()
    {
        movement *= speed;
        movement = Vector2.ClampMagnitude(movement, speed);

        rigidbody2D.velocity = movement;
    }
}