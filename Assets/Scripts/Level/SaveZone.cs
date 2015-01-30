using UnityEngine;

public class SaveZone : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger && other.tag == "Player")
        {
            GameManager.Instance.SaveGame(1);
        }
    }
}