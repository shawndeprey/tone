using UnityEngine;

public class BasicGun : Weapon
{
    void Awake()
    {
        fireRate = 0.5f;
        shootCooldown = 0f;
    }
}