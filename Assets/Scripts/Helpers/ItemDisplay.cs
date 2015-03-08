using UnityEngine;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    public GameObject displayPanel;
    public GameObject itemSlot;
    public Text itemCount;
    public Sprite[] itemSprites;

    public void SetItem(int itemIndex, int count, int max)
    {
        itemSlot.GetComponent<Image>().sprite = itemSprites[itemIndex];
        SetItemCount(count, max);
    }

    public void SetItemCount(int count, int max)
    {
        if (max == 0)
        {
            itemCount.text = "~~~";
        }
        else
        {
            itemCount.text = count + "\n" + "/" + max;
        }
    }
}