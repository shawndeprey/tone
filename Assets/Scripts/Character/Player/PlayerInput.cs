using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 lastDirection { get { return _lastDirection; } set { _lastDirection = value; } }

    private Weapon primaryWeapon;
    private Vector2 _lastDirection = new Vector2(1, 0);

    void Start()
    {
        primaryWeapon = GetComponent<Weapon>();
    }

	void Update()
    {
        if (!GameManager.Instance.isPaused)
        {
            float temp = Input.GetAxis("Horizontal");
            _lastDirection.x = temp < 0f ? -1f : temp > 0f ? 1f : _lastDirection.x;

            if (Input.GetButton("Primary Shoot"))
            {
                if (primaryWeapon != null && primaryWeapon.CanAttack)
                {
                    primaryWeapon.ShootPressed();
                }
            }
            else if (Input.GetButtonUp("Primary Shoot"))
            {
                primaryWeapon.ShootReleased();
            }
        }
    }
}