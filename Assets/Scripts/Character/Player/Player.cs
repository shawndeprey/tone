using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public int maxHealth { get { return _maxHealth; } }
    public int health { get { return _health; } set { _health = value > 0 ? value <= maxHealth ? value : maxHealth : 0; } }
    public bool invulnerable { get { return _invulnerable; } }

    private int _health = 10;
    private int _maxHealth = 10;
    private bool _invulnerable = false;
    private Vector2 enemyDirection;
    private Color spriteColor;
    private SpriteRenderer sprite;

    void Awake()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        spriteColor = sprite.color;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        MenuManager.Instance.GetComponent<HealthDisplay>().SetHealth(_health, _maxHealth);
    }

    public void Damage(int amount)
    {
        HitSequence();

        // Use 0 for insta-death
        if (amount == 0)
        {
            health = 0;
        }
        else
        {
            health -= amount;
        }

        if (health <= 0)
        {
            health = 0;
            Death();
        }

        MenuManager.Instance.GetComponent<HealthDisplay>().SetHealth(_health, _maxHealth);
    }

    public void Heal(int amount)
    {
        health += amount;
        MenuManager.Instance.GetComponent<HealthDisplay>().SetHealth(_health, _maxHealth);
    }

    public void SetMaxHealth(int amount)
    {
        _maxHealth = amount > 10 ? amount : 10;
    }

    private void Death()
    {
        MenuManager.Instance.SwitchMenu("Game Over Panel");
        gameObject.GetComponent<Disabler>().Disable();
    }

    private void HitSequence()
    {
        StartCoroutine(FlashPlayer(5, 0.1f));
        Vector2 force = new Vector2(enemyDirection.x, 1);
        gameObject.rigidbody2D.velocity = force * 15;
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (!_invulnerable && other.gameObject.tag == "Enemy")
        {
            enemyDirection = other.gameObject.rigidbody2D.velocity;
            int damageAmount = other.gameObject.GetComponent<Enemy>().damage;
            Damage(damageAmount);
        }
    }

    IEnumerator FlashPlayer(int numTimes, float delay)
    {
        _invulnerable = true;
        for (int loop = 0; loop < numTimes; loop++)
        {
            sprite.color = new Color(0.75f, spriteColor.g * 0.25f, spriteColor.b * 0.25f, 0.5f);

            yield return new WaitForSeconds(delay);

            sprite.color = new Color(0.5f, spriteColor.g * 0.25f, spriteColor.b * 0.25f, 0.75f);

            yield return new WaitForSeconds(delay);

            sprite.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, 1);
        }
        _invulnerable = false;
    }
}