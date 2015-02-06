using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth { get { return _maxHealth; } }
    public int health { get { return _health; } set { _health = value > 0 ? value <= maxHealth ? value : maxHealth : 0; } }

    private int _health = 10;
    private int _maxHealth = 10;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Damage(int amount)
    {
        // Use 0 for insta-death
        if(amount == 0)
        {
            health = 0;
        }
        else
        {
            health -= amount;
        }

        if (health <= 0)
        {
            health = 0;
            Death();
        }
    }

    public void Heal(int amount)
    {
        health += amount;
    }

    public void SetMaxHealth(int amount)
    {
        _maxHealth = amount > 10 ? amount : 10;
    }

    private void Death()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}