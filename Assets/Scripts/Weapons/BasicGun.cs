using UnityEngine;

public class BasicGun : Weapon
{
    void Awake()
    {
        fireRate = 0.5f;
        shootCooldown = 0f;
    }

    public override void Attack(Vector2 direction)
    {
        if (CanAttack)
        {
            shootCooldown = fireRate;

            GameObject projectileObject = ProjectileManager.Instance.GetPool(0).Create(transform.position + new Vector3(direction.x / 2f, direction.y / 2f, 0f));

            if (projectileObject == null)
            {
                return;
            }

            Move move = projectileObject.GetComponent<Move>();
            move.movement = direction;

            projectileObject.SetActive(true);
        }
    }

    public override void ShootPressed()
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

        Attack(direction);
    }

    public override void ShootReleased()
    {
        shootCooldown = 0f;
    }
}