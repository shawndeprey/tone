using UnityEngine;
using System.Collections.Generic;

public class PlayerInput : MonoBehaviour
{
    public Vector2 lastDirection { get { return _lastDirection; } set { _lastDirection = value; } }

    private Vector2 _lastDirection = new Vector2(1, 0);
    private Weapon primaryWeapon;
    private int currentWeaponIndex;
    private List<Weapon> weapons;

    void Start()
    {
        weapons = new List<Weapon>();
        Weapon[] tempWeapons = GetComponents<Weapon>();

        weapons.Add(null);
        for (int i = 0; i < tempWeapons.Length; i++)
        {
            weapons.Add(tempWeapons[i]);
        }

        currentWeaponIndex = 0;
        EquipWeapon(currentWeaponIndex);
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
        }
    }

    public void EquipNextWeapon()
    {
        currentWeaponIndex = currentWeaponIndex >= weapons.Count - 1 ? 0 : currentWeaponIndex + 1;
        EquipWeapon(currentWeaponIndex);
    }

    private void EquipWeapon(int weaponIndex)
    {
        primaryWeapon = weapons[weaponIndex];
        MenuManager.Instance.GetWeaponDisplay().SetItem(weaponIndex);
    }
}