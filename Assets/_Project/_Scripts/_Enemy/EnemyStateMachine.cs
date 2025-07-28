namespace CF.Enemy {

    using UnityEngine;
    public class EnemyStateMachine : MonoBehaviour 
    {

        private EnemyState currentState;

        public EnemyContext context { get; private set; }



        private EnemyEntranceState entranceState;
        private EnemyIdleState idleState;
        private EnemyAttackState attackState;
        private EnemyInterruptState interruptState;
        private EnemyMoveState moveState;

        private void Awake()
        {
            Initialize();
        }

        private void Start()
        {
            currentState.Enter();
        }

        private void Update()
        {
            if (currentState != null)
            {
                currentState.LogicUpdate();
            }
        }

        private void FixedUpdate()
        {
            if (currentState != null)
            {
                currentState.PhysicsUpdate();
            }
        }


        public void Initialize()
        {
            this.context = GetComponent<EnemyContext>();
            entranceState = new EnemyEntranceState(this);
            idleState = new EnemyIdleState(this);
            attackState = new EnemyAttackState(this);
            interruptState = new EnemyInterruptState(this);
            moveState = new EnemyMoveState(this);

            currentState = entranceState; 
        }

        public void EnterState(EnemyStateType state) 
        {
            switch (state)
            {
                case EnemyStateType.Entrance:
                    if (currentState is EnemyEntranceState) return; // Prevent re-entering the entrance state
                    currentState = entranceState;
                    break;
                case EnemyStateType.Idle:
                    if (currentState is EnemyIdleState) return; // Prevent re-entering the idle state
                    currentState = idleState;
                    break;
                case EnemyStateType.Attack:
                    if (currentState is EnemyAttackState) return; // Prevent re-entering the attack state
                    currentState = attackState;
                    break;
                case EnemyStateType.Interrupt:
                    if (currentState is EnemyInterruptState) return; // Prevent re-entering the interrupt state
                    currentState = interruptState;
                    break;
                case EnemyStateType.Move:
                    if (currentState is EnemyMoveState) return; // Prevent re-entering the move state
                    currentState = moveState;
                    break;
                default:
                    currentState = idleState; // Default to idle state if an unknown state is provided
                    break;
            }
            currentState.Enter();
        }
    }
    public enum EnemyStateType
    {
        Entrance,
        Idle,
        Attack,
        Interrupt,
        Move
    }
}