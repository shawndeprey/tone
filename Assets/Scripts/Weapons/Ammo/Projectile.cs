using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public int damage { get { return _damage; } }

    protected int _damage;
    protected float _lifetime;

    void OnEnable()
    {
        if (_lifetime > 0f)
        {
            if (ProjectileManager.Instance != null)
            {
                ProjectileManager.Instance.Recycle(gameObject, _lifetime);
            }
        }
    }
}