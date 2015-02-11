using UnityEngine;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    public GameObject displayPanel;
    public GameObject itemSlot;
    public Sprite[] itemSprites;

    public void SetItem(int itemIndex)
    {
        itemSlot.GetComponent<Image>().sprite = itemSprites[itemIndex];
    }
}