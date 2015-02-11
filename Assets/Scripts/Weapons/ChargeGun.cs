using UnityEngine;
using System.Collections;

public class ChargeGun : Weapon
{
    private float chargeTime = 1.5f;
    private bool fireCharge = false;
    private bool charging = false;
    private bool fireHeld = false;

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
            if (!fireHeld)
            {
                fireHeld = true;
                NormalShot(direction);
            }

            if (!charging)
            {
                charging = true;
                fireCharge = false;
                StartCoroutine(ChargeTimer(chargeTime));
            }

            if (fireCharge)
            {
                ChargeShot(direction);
                charging = false;
            }
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
        StopCoroutine("ChargeTimer");
        fireCharge = false;
        fireHeld = false;
        shootCooldown = 0f;
    }

    private void NormalShot(Vector2 direction)
    {
        GameObject projectileObject = ProjectileManager.Instance.GetPool(0).Create(transform.position + new Vector3(direction.x / 2f, direction.y / 2f, 0f));
        if (projectileObject == null)
        {
            return;
        }

        Move move = projectileObject.GetComponent<Move>();
        move.movement = direction;

        projectileObject.SetActive(true);
    }

    private void ChargeShot(Vector2 direction)
    {
        GameObject projectileObject = ProjectileManager.Instance.GetPool(1).Create(transform.position + new Vector3(direction.x / 2f, direction.y / 2f, 0f));
        if (projectileObject == null)
        {
            return;
        }

        Move move = projectileObject.GetComponent<Move>();
        move.movement = direction;

        projectileObject.SetActive(true);
    }

    private IEnumerator ChargeTimer(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        fireCharge = true;
    }
}