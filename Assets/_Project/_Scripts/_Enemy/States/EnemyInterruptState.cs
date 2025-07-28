using System.Diagnostics;

namespace CF.Enemy
{
    public class EnemyInterruptState : EnemyState
    {
        public EnemyInterruptState(EnemyStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            UnityEngine.Debug.Log("EnemyInterruptState: Entering state");
            base.Enter();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            // Check if the enemy is no longer stunned
            if (!context.HasActiveEffect(StatusEffect.Stunned))
            {
                // Check if enemy is on a valid lane position
                if (!context.movementController.IsAtLane((int)context.currentLane))
                {
                    // If not, move to the current lane before idling
                    base.Exit();
                    stateMachine.EnterState(EnemyStateType.Move);
                    return;
                }
                // Exit the interrupt state and transition to idle state
                Exit();
            }
        }

        public override void Exit()
        {
            base.Exit();
            stateMachine.EnterState(EnemyStateType.Idle);
        }

    }
}