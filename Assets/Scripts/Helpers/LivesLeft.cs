using UnityEngine;
using UnityEngine.UI;

public class LivesLeft : MonoBehaviour
{
    private int livesLeft;
    private Text livesText;

    void Start()
    {
        livesLeft = GameManager.Instance.extraLives;
        livesText = gameObject.GetComponent<Text>();
        livesText.text = livesLeft.ToString();
    }

    void Update()
    {
        if (livesLeft != GameManager.Instance.extraLives)
        {
            livesLeft = GameManager.Instance.extraLives;
            livesText.text = livesLeft.ToString();
        }
    }
}