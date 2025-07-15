using UnityEngine;

namespace CF.Enemy {
public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(EnemyBrain _brain, EnemyStateMachine _stateMachine) : base(_brain, _stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        PlayAnim(brain.Data.IdleAnim.name);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(lockState) return;

        if (Time.time >= startTime + brain.Data.IdleTime)
        {
            stateMachine.ChangeState(brain.MoveState);
        }

    }

    
}
}