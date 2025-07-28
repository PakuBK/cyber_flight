using UnityEngine;

namespace CF.Enemy {
    public class EnemyIdleState : EnemyState
    {
        private float waitTime;
        private float startTime;

        public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine) {}

        public override void Enter()
        {
            base.Enter();
            Debug.Log("EnemyIdleState: Entering Idle State");
            waitTime = Random.Range(context.enemyData.IdleWaitMin, context.enemyData.IdleWaitMax);
            startTime = Time.time;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // Check for stun, transition to interrupt state if stunned
            if (context.HasActiveEffect(StatusEffect.Stunned))
            {
                base.Exit();
                stateMachine.EnterState(EnemyStateType.Interrupt);
                return;
            }

            if (Time.time >= startTime + waitTime)
            {
                int attackProbability = context.enemyData.AttackProbability;
                if (Random.Range(0, 100) < attackProbability)
                {
                    Exit();
                    stateMachine.EnterState(EnemyStateType.Attack);
                }
                else
                {
                    Exit();
                    stateMachine.EnterState(EnemyStateType.Move);
                }
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}