using UnityEngine;
using FSMHelper;

public class WanderingSpirit : Enemy
{
    public int damageAmount;
    public int healthAmount;
    public int maxHealthAmount;

    protected override void Awake()
    {
        base.Awake();
        _damage = damageAmount;
        _health = healthAmount;
        _maxHealth = maxHealthAmount;
    }

    protected override void MakeFSM()
    {
        fsm = new FSMSystem();

        IdleState idle = new IdleState(gameObject);
        idle.AddTransition(Transition.PlayerVisible, StateID.Chase);
        fsm.AddState(idle);

        ChaseState chase = new ChaseState(gameObject);
        chase.AddTransition(Transition.PlayerNotVisible, StateID.Idle);
        fsm.AddState(chase);
    }

    protected override void Damage(int amount)
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
    }

    protected override void HitSequence()
    {
        Vector2 force = new Vector2(0, 1);
        gameObject.rigidbody2D.velocity = force * 2.5f;
    }

    protected override void Death()
    {
        Destroy(gameObject);
    }
}