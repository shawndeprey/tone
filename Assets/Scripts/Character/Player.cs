using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 10;

    private int maxHealth = 10;

    public void Damage(int amount)
    {
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

    private void Death()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
}