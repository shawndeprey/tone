using UnityEngine;
using FSMHelper;

public class IdleState : FSMState
{
    public IdleState(GameObject npc)
    {
        stateID = StateID.Idle;
        SetEnemy(npc);
    }

    public override void DoBeforeLeaving() { }

    public override void DoBeforeEntering() { }

    public override void TransitionLogic(GameObject target, GameObject npc)
    {
        if (target != null && Vector3.Distance(target.transform.position, npc.transform.position) < 4)
        {
            enemy.SetTransition(Transition.PlayerVisible);
        }
    }

    public override void BehaviorLogicFixed(GameObject target)
    {
        enemy.rigidbody2D.velocity = new Vector2(0, enemy.rigidbody2D.velocity.y);
    }

    public override void BehaviorLogic(GameObject target)
    {
        
    }
}