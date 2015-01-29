using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Weapon primaryWeapon;
    private Vector2 lastDirection = new Vector2(1, 0);

    void Start()
    {
        primaryWeapon = GetComponent<Weapon>();
    }

	void Update()
    {
        float temp = Input.GetAxis("Horizontal");
        lastDirection.x = temp < 0f ? -1f : temp > 0f ? 1f : lastDirection.x;

        if (Input.GetButton("Primary Shoot"))
        {
            if(primaryWeapon != null && primaryWeapon.CanAttack)
            {
                Vector2 direction;
                direction.x = Input.GetAxis("Horizontal");
                direction.x = direction.x < 0f ? -1f : direction.x > 0f ? 1f : 0f;
                direction.y = Input.GetAxis("Vertical");
                direction.y = direction.y < 0f ? -1f : direction.y > 0f ? 1f : 0f;

                if (direction.y == 0.0f)
                {
                    if (direction.x == 0.0f)
                    {
                        direction = lastDirection;   
                    }
                    else
                    {
                        lastDirection.x = direction.x;
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