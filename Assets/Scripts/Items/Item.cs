using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public float useCooldown;

    protected float useRate;

    public bool CanUse { get { return useCooldown <= 0f; } }

    void Update()
    {
        if (!GameManager.Instance.isPaused)
        {
            if (useCooldown > 0f)
            {
                useCooldown -= Time.deltaTime;
            }
        }
    }

    public abstract void Use(Vector2 direction);
    public abstract void UsePressed();
    public abstract void UseReleased();
}