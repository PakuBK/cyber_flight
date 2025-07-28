using CF.Player;
using CF.UI;
using UnityEngine;
using System;

namespace CF.Enemy
{
    public class EnemyEntranceState : EnemyState
    {
        public EnemyEntranceState(EnemyStateMachine stateMachine) : base(stateMachine) { }
        public override void Enter()
        {
            base.Enter();
            // UIController.current.ShowVersusUI( context.enemyData.Icon,context.enemyData.Icon,"test",OnUIAnimationComplete);
            // DEBUG
            Exit();
        }

        private void OnUIAnimationComplete()
        {
            Exit();
        }

        public override void Exit()
        {
            base.Exit();
            stateMachine.EnterState(EnemyStateType.Idle);
        }
    }
}