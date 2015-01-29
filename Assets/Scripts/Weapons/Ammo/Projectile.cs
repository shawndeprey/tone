using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected int damage;
    protected float lifetime;

    void OnEnable()
    {
        if(lifetime > 0f)
        {
            ProjectilePool.Instance.Recycle(this.gameObject, lifetime);
        }
    }
}