using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour
{
    public bool isActive;
    public float speed;
    public float distance;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool direction = false;

    void Start()
    {
        startPosition = new Vector3(transform.position.x, transform.position.y, 0);
        endPosition = new Vector3(transform.position.x, transform.position.y + distance, 0);
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.isPaused)
        {
            if (!isActive)
            {
                return;
            }

            if (direction)
            {
                if (transform.position == startPosition)
                {
                    direction = false;
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, startPosition, speed);
                }
            }
            else
            {
                if (transform.position == endPosition)
                {
                    direction = true;
                }
                else
                {
                    transform.position = Vector2.MoveTowards(transform.position, endPosition, speed);
                }
            }
        }
    }
}