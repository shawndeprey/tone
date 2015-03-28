using UnityEngine;
using System.Collections.Generic;

public class PlayerInput : MonoBehaviour
{
    public Vector2 lastDirection { get { return _lastDirection; } set { _lastDirection = value; } }

    private Vector2 _lastDirection = new Vector2(1, 0);
    private Weapon primaryWeapon;
    private int currentWeaponIndex;
    private List<Weapon> weapons;
    private Item primaryItem;
    private int currentItemIndex;
    private List<Item> items;

    void Start()
    {
        weapons = new List<Weapon>();
        Weapon[] tempWeapons = GetComponents<Weapon>();

        weapons.Add(null);
        for (int i = 0; i < tempWeapons.Length; i++)
        {
            weapons.Add(tempWeapons[i]);
        }

        currentWeaponIndex = GameManager.Instance.equippedWeapon;
        EquipWeapon(currentWeaponIndex);



        items = new List<Item>();
        Item[] tempItems = GetComponents<Item>();

        items.Add(null);
        for (int i = 0; i < tempItems.Length; i++)
        {
            items.Add(tempItems[i]);
        }

        currentItemIndex = GameManager.Instance.equippedItem;
        EquipItem(currentItemIndex);
    }

	void Update()
    {
        if (!GameManager.Instance.isPaused)
        {
            float temp = Input.GetAxis("Horizontal");
            _lastDirection.x = temp < 0f ? -1f : temp > 0f ? 1f : _lastDirection.x;

            if (primaryWeapon != null)
            {
                if (Input.GetButton("Primary Shoot"))
                {
                    primaryWeapon.ShootPressed();
                }
                else if (Input.GetButtonUp("Primary Shoot"))
                {
                    primaryWeapon.ShootReleased();
                }
            }

            if (Input.GetButtonDown("Weapon Swap"))
            {
                EquipNextWeapon();
            }

            if (primaryItem != null)
            {
                if (Input.GetButton("Use Item"))
                {
                    primaryItem.UsePressed();
                }
                else if (Input.GetButtonUp("Use Item"))
                {
                    primaryItem.UseReleased();
                }
            }

            if (Input.GetButtonDown("Item Swap"))
            {
                EquipNextItem();
            }
        }
    }

    public void EquipNextWeapon()
    {
        currentWeaponIndex = currentWeaponIndex >= weapons.Count - 1 ? 0 : currentWeaponIndex + 1;
        GameManager.Instance.equippedWeapon = currentWeaponIndex;
        EquipWeapon(currentWeaponIndex);
    }

    public void EquipWeapon(int weaponIndex)
    {
        primaryWeapon = weapons[weaponIndex];

        int count = GameManager.Instance.GetCurrentAmmo(weaponIndex);
        int max = GameManager.Instance.GetMaxAmmo(weaponIndex);
        MenuManager.Instance.GetWeaponDisplay().SetItem(weaponIndex, count, max);
    }

    public void EquipNextItem()
    {
        currentItemIndex = currentItemIndex >= items.Count - 1 ? 0 : currentItemIndex + 1;
        GameManager.Instance.equippedItem = currentItemIndex;
        EquipItem(currentItemIndex);
    }

    public void EquipItem(int itemIndex)
    {
        primaryItem = items[itemIndex];

        int count = GameManager.Instance.GetCurrentCharges(itemIndex);
        int max = GameManager.Instance.GetMaxCharges(itemIndex);
        MenuManager.Instance.GetItemDisplay().SetItem(itemIndex, count, max);
        Debug.Log(itemIndex);
    }
}