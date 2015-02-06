using UnityEngine;

public class SaveZone : MonoBehaviour
{
    public GameObject canvas;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger && other.tag == "Player")
        {
            canvas.SetActive(true);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.isTrigger && other.tag == "Player")
        {
            if (Input.GetButtonDown("Action"))
            {
                MenuManager.Instance.SaveIndicator();
                GameManager.Instance.SaveGame(1);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.isTrigger && other.tag == "Player")
        {
            canvas.SetActive(false);
        }
    }
}