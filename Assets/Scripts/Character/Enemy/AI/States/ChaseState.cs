using UnityEngine;
using FSMHelper;

public class ChaseState : FSMState
{
    private Vector2 movement = Vector2.zero;

    public ChaseState(GameObject npc)
    {
        stateID = StateID.Chase;
        SetEnemy(npc);
    }

    
    public override void DoBeforeLeaving() { }

    public override void DoBeforeEntering() { }

    public override void TransitionLogic(GameObject target, GameObject npc)
    {
        if (target == null || Vector3.Distance(target.transform.position, npc.transform.position) > 5)
        {
            enemy.SetTransition(Transition.PlayerNotVisible);
        }
    }

    public override void BehaviorLogicFixed(GameObject target)
    {
        if (target.transform.position.x < enemy.transform.position.x)
        {
            movement = new Vector2(-1 * enemy.speed, enemy.GetComponent<Rigidbody2D>().velocity.y);
        }
        else if (target.transform.position.x > enemy.transform.position.x)
        {
            movement = new Vector2(1 * enemy.speed, enemy.GetComponent<Rigidbody2D>().velocity.y);
        }
        else
        {
            movement = Vector2.zero;
        }

        if (!GameManager.Instance.isPaused)
        {
            enemy.GetComponent<Rigidbody2D>().velocity = movement;
        }
        else
        {
            enemy.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public override void BehaviorLogic(GameObject target)
    {
        
    }
}