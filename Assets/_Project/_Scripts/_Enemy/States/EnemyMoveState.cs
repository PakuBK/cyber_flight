using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace CF.Enemy {
    public class EnemyMoveState : EnemyState
    {
        private int targetLaneIndex = 0;

        private EnemyMovementController movementController => context.movementController;

        public EnemyMoveState(EnemyStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            base.Enter();
            int currentLaneIndex = (int) context.currentLane;

            if (movementController.IsAtLane(currentLaneIndex))
            {
                do { targetLaneIndex = Random.Range(0, 4); } while (targetLaneIndex == currentLaneIndex);
            }
            else
            {
                targetLaneIndex = movementController.GetNearestLaneIndex();
            }
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

            // Move towards the target lane, if reached, exit the state
            if (movementController.MoveToLane(targetLaneIndex))
            {
                context.SetCurrentLane((Lane)targetLaneIndex);
                Exit();
            }
        }

        public override void Exit()
        {
            base.Exit();
            // Decide to attack or idle
            int attackProbability = context.enemyData.AttackProbability;
            if (Random.Range(0, 100) < attackProbability)
            {
                stateMachine.EnterState(EnemyStateType.Attack);
            }
            else
            {
                stateMachine.EnterState(EnemyStateType.Idle);
            }
        }

    
    }
}