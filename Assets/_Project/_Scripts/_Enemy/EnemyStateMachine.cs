namespace CF.Enemy {
public class EnemyStateMachine
{
    public EnemyState currentState;

    public bool deactivate_this;

    public void ChangeState(EnemyState state) 
    {
        if (deactivate_this)
        {
            return;
        }

        currentState.Exit();
        currentState = state;
        currentState.Enter();

    }
    
    public void Initialize(EnemyState startState) 
    {
        currentState = startState;
        currentState.Enter();
    }
}
}