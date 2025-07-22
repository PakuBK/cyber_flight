using UnityEngine;

namespace CF.Enemy {
    public class EnemyState
    {
        protected EnemyContext context => stateMachine.context;
        protected EnemyStateMachine stateMachine;

        public EnemyState(EnemyStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public virtual void Enter()
        {
        
        }

        public virtual void Exit() 
        {
        
        }

        public virtual void LogicUpdate() 
        {
        
        }

        public virtual void PhysicsUpdate() 
        {
        
        }

        protected virtual void PlayAnim(string _name)
        {
        
        }

  
}
}
