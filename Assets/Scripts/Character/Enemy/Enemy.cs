using UnityEngine;
using FSMHelper;

public abstract class Enemy : MonoBehaviour
{
    public int maxHealth { get { return _maxHealth; } }
    public int health { get { return _health; } set { _health = value > 0 ? value <= maxHealth ? value : maxHealth : 0; } }
    public GameObject target { get; set; }
    public int damage { get { return _damage; } }
    public float speed;

    protected int _damage;
    protected int _health;
    protected int _maxHealth;
    protected GameObject _target;
    protected FSMSystem fsm;

    protected virtual void Awake()
    {
        MakeFSM();
    }

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.isPaused)
        {
            fsm.CurrentState.BehaviorLogicFixed(target);
        }
    }

    void Update()
    {
        if (!GameManager.Instance.isPaused)
        {
            fsm.CurrentState.TransitionLogic(target, gameObject);
            fsm.CurrentState.BehaviorLogic(target);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Projectile projectile = other.gameObject.GetComponent<Projectile>();

        if (projectile != null)
        {
            ProjectilePool.Instance.Recycle(projectile.gameObject);
            Damage(projectile.damage);
        }
    }

    public void SetTransition(Transition t)
    {
        fsm.PerformTransition(t);
    }

    protected abstract void MakeFSM();

    protected abstract void Damage(int amount);
    protected abstract void HitSequence();
    protected abstract void Death();
}