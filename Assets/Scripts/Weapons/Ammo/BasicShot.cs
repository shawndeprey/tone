using UnityEngine;

public class BasicShot : Projectile
{
    void Awake()
    {
        _damage = 1;
        _lifetime = 0.25f;
    }
}