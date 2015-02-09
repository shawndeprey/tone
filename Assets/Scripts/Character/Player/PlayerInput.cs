using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 lastDirection { get { return _lastDirection; } }

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
                    Vector2 direction = Vector2.zero;
                    direction.y = Input.GetAxis("Vertical");
                    direction.y = direction.y < 0f ? -1f : direction.y > 0f ? 1f : 0f;

                    if (direction.y == 0.0f)
                    {
                        direction.x = Input.GetAxis("Horizontal");
                        direction.x = direction.x < 0f ? -1f : direction.x > 0f ? 1f : 0f;
                        if (direction.x == 0.0f)
                        {
                            direction = _lastDirection;
                        }
                        else
                        {
                            _lastDirection.x = direction.x;
                        }
                    }

                    primaryWeapon.Attack(direction);
                }
            }
            else if (Input.GetButtonUp("Primary Shoot"))
            {
                primaryWeapon.shootCooldown = 0f;
            }
        }
    }
}