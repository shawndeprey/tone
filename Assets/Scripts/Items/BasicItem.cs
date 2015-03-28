using UnityEngine;

public class BasicItem : Item
{
    void Awake()
    {
        useRate = 0.5f;
        useCooldown = 0f;
    }

    public override void Use(Vector2 direction)
    {
        if (CanUse)
        {
            useCooldown = useRate;
        }
    }

    public override void UsePressed()
    {
        Vector2 direction = Vector2.zero;
        direction.y = Input.GetAxis("Vertical");
        direction.y = direction.y < 0f ? -1f : direction.y > 0f ? 1f : 0f;

        if (direction.y == 0.0f)
        {
            direction.x = Input.GetAxis("Horizontal");
            direction.x = direction.x < 0f ? -1f : direction.x > 0f ? 1f : 0f;
            if (direction.x == 0.0f)
            {
                direction = gameObject.GetComponent<PlayerInput>().lastDirection;
            }
            else
            {
                gameObject.GetComponent<PlayerInput>().lastDirection = new Vector2(direction.x, gameObject.GetComponent<PlayerInput>().lastDirection.y);
            }
        }

        Use(direction);
    }

    public override void UseReleased()
    {
        useCooldown = 0f;
    }
}