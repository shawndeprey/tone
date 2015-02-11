using UnityEngine;

public class ChargeShot : Projectile
{
    void Awake()
    {
        _damage = 5;
        _lifetime = 0.5f;
    }
}