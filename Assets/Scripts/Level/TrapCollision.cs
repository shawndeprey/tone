using UnityEngine;

public class TrapCollision : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger && other.tag == "Player")
        {
            other.GetComponent<Player>().Damage(0);
        }
    }
}