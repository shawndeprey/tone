using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float shootCooldown;

    protected float fireRate;

    public bool CanAttack { get { return shootCooldown <= 0f; } }

    void Update()
    {
        if (!GameManager.Instance.isPaused)
        {
            if (shootCooldown > 0f)
            {
                shootCooldown -= Time.deltaTime;
            }
        }
    }

    public abstract void Attack(Vector2 direction);
    public abstract void ShootPressed();
    public abstract void ShootReleased();
}