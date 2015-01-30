using UnityEngine;

public class BoundsZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Projectile projectile = other.gameObject.GetComponent<Projectile>();

        if(projectile != null)
        {
            ProjectilePool.Instance.Recycle(projectile.gameObject);
        }
    }
}