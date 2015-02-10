using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public GameObject healthPanel;
    public Sprite[] healthBarSprites;
    public GameObject[] healthBars;

    private int healthPerBar = 2;

    public void SetHealth(int healthAmount, int maxHealth)
    {
        int numBars = (maxHealth + healthPerBar - 1) / healthPerBar;
        int numFullBars = (healthAmount + healthPerBar - 1) / healthPerBar;

        int length = healthBars.Length;
        for (int i = 0; i < length; i++)
        {
            if (i < numFullBars)
            {
                healthBars[i].GetComponent<Image>().sprite = healthBarSprites[2];
            }
            else if (i < numBars)
            {
                healthBars[i].GetComponent<Image>().sprite = healthBarSprites[0];
            }
            else
            {
                healthBars[i].SetActive(false);
            }
        }

        if (healthAmount % 2 != 0)
        {
            healthBars[numFullBars - 1].GetComponent<Image>().sprite = healthBarSprites[1];
        }
    }
}