using UnityEngine;

public class BasicShot : Projectile
{
    void Awake()
    {
        damage = 1;
        lifetime = 0.25f;
    }
}