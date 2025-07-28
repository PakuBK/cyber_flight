using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Enemy
{
    class EnemyDeathState : EnemyState
    {
        public EnemyDeathState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            // Play death animation
            // Play death sound effect
            // Optionally, trigger any death effects or cleanup
        }
    }
}
