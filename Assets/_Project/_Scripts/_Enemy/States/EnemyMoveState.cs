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
            Debug.Log("EnemyMoveState: Entering Move State");
            base.Enter();
            int currentLaneIndex = (int)context.currentLane;

            // Ensure Enemy is at a lane before moving
            if (movementController.IsAtLane(currentLaneIndex))
            {
                if (Random.Range(0, 100) < context.enemyData.PlayerFocusWeight) {
                    targetLaneIndex = context.GetPlayerLaneIndex();
                }
                else
                {
                    do { targetLaneIndex = Random.Range(0, 3); } while (targetLaneIndex == currentLaneIndex);
                }  
            }
            // If the enemy is not at a lane, find the nearest lane index
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