using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour
{
    public string sceneName;
    public string doorName;
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
                GameManager.Instance.SetDoor(doorName);
                Application.LoadLevel(sceneName);
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
