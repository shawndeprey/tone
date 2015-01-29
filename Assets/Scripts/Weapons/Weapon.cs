using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float shootCooldown;

    protected float fireRate;

    public bool CanAttack { get { return shootCooldown <= 0f; } }

    void Update()
    {
        if(shootCooldown > 0f)
        {
            shootCooldown -= Time.deltaTime;
        }
    }

    public void Attack(Vector2 direction)
    {
        if(CanAttack)
        {
            shootCooldown = fireRate;

            GameObject projectileObject = ProjectilePool.Instance.Create(transform.position + new Vector3(direction.x / 2f, direction.y / 2f, 0f));

            if (projectileObject == null)
            {
                return;
            }

            Move move = projectileObject.GetComponent<Move>();
            move.movement = direction;

            projectileObject.SetActive(true);
        }
    }
}