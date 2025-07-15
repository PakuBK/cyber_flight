using UnityEngine;

namespace CF.Enemy {
public class EnemyState
{
    protected EnemyBrain brain;
    protected EnemyStateMachine stateMachine;
    protected float startTime;
    protected string animName;

    protected bool lockState;

    public EnemyState(EnemyBrain _brain, EnemyStateMachine _stateMachine)
    {
        brain = _brain;
        stateMachine = _stateMachine;

    }

    public virtual void Enter()
    {
        startTime = Time.time;

        if (!brain.isStunded)
        {
            LockState(false);
        }
    }

    public virtual void Exit() 
    {
        
    }

    public virtual void LogicUpdate() 
    {
        if (brain.isStunded)
        {
            brain.FlashColorEffect(Color.magenta);

            LockState(true);

            if (Time.time > brain.specialStartTime + brain.specialTime)
            {
                brain.StunEnemy(false);
                LockState(false);
                brain.spriteRenderer.color = Color.white;
            }
        }

        if (brain.IsSlowed)
        {
            if (Time.time > brain.specialStartTime + brain.specialTime)
            {
                brain.SlowEnemy(false);
            }
        }

    }

    public virtual void PhysicsUpdate() 
    {
        
    }

    protected virtual void PlayAnim(string _name)
    {
        brain.Anim.Play(_name);
    }

    public void LockState(bool _state)
    {
        lockState = _state;
    }
}
}
