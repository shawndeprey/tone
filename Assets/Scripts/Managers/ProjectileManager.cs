using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager Instance { get { return _instance; } }
    public List<GameObject> pools;

    private static ProjectileManager _instance;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
    }

    public ProjectilePool GetPool(int index)
    {
        return pools[index].GetComponent<ProjectilePool>();
    }

    public void Recycle(GameObject projectile)
    {
        Recycle(projectile, 0f);
    }

    public void Recycle(GameObject projectile, float seconds)
    {
        int index;

        switch (projectile.GetComponent<Projectile>().GetType().ToString())
        {
            case "BasicShot":
                index = 0;
                break;
            case "ChargeShot":
                index = 1;
                break;
            default:
                index = 0;
                break;
        }

        GetPool(index).Recycle(projectile, seconds);
    }
}