using UnityEngine;

public class SpecialZone : MonoBehaviour
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
                DoZoneAction();
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

    protected virtual void DoZoneAction() { }
}