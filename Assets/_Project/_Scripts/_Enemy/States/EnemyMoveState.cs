using UnityEngine;

namespace CF.Enemy {
public class EnemyMoveState : EnemyState
{
    protected bool isMoving;
    protected Vector3 lanePos;

    private float currentMoveSpeed;

    private int playerWeight;
    private int currentLane;


    public EnemyMoveState(EnemyBrain _brain, EnemyStateMachine _stateMachine) : base(_brain, _stateMachine)
    {
        currentMoveSpeed = _brain.Data.MoveSpeed;
        currentLane = brain.currentLane;
    }

    public override void Enter()
    {
        base.Enter();

        PlayAnim(brain.Data.MoveAnim.name);
        lanePos = GetNextLanePosition();
        isMoving = true;
        playerWeight = brain.Data.PlayerFocusWeight;

        if (brain.IsSlowed)
        {
            currentMoveSpeed = brain.Data.MoveSpeed * brain.specialMultiplier;
            brain.spriteRenderer.color = brain.SlowColor;
        }
        else
        {
            currentMoveSpeed = brain.Data.MoveSpeed;
            brain.spriteRenderer.color = Color.white;
        }
    }
    public override void Exit()
    {
        base.Exit();

        isMoving = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (lockState) return;

        if (isMoving)
        {
            MoveEnemy(lanePos);
        }
    }

    private Vector3 GetNextLanePosition()
    {
        float[] laneAnchorPoints = brain.laneAnchorPoints;

        int newLane;

        int[] movePositions = { 0, 1, 2 };
        newLane = GetWeightedLane(movePositions);

        currentLane = newLane;
        brain.currentLane = newLane;

        Vector3 _pos = new Vector3(brain.laneAnchorPoints[currentLane], brain.transform.position.y, 0);

        return _pos;
    }

    private void MoveEnemy(Vector3 _pos)
    {
        float dist = Vector2.Distance(_pos, brain.transform.position);
        if (!(dist < 0.15f))
        {
            float concreteSpeed = (currentMoveSpeed * (ScreenSize.GetScreenToWorldWidth * 0.1f)) * Time.fixedDeltaTime;
            brain.transform.Translate(Vector3.Normalize(_pos - brain.transform.position) * concreteSpeed);
        }
        else
        {
            brain.transform.position = _pos;

            if (Random.Range(1, 101) >= brain.Data.AttackProbabilty)
            {
                stateMachine.ChangeState(brain.IdleState);
            }
            else
            {
                stateMachine.ChangeState(brain.AttackState);
            }
            
        }
        
    }

    private int GetWeightedLane(int[] possibleMoves)
    {
        int[] weightedMoves = new int[possibleMoves.Length];
        int sum_weight = 0;
        for (int i = 0; i < weightedMoves.Length; i++)
        {
            if (brain.PlayerController.CurrentLane == possibleMoves[i]) // if move is on player pos add player weight
            {
                sum_weight += playerWeight;
                weightedMoves[i] = sum_weight;
            }
            else
            {
                if (currentLane == possibleMoves[i]) // moves on the same lines are less likely
                {
                    sum_weight += 1;
                    weightedMoves[i] = sum_weight;
                }
                else
                {
                    sum_weight += 2;
                    weightedMoves[i] = sum_weight;
                }
                
            }
        }
        int rand = Random.Range(0, sum_weight+1); // maybe this is wrong
        for (int i = 0; i < weightedMoves.Length; i++)
        {
            if (weightedMoves[i] >= rand)
            {
                return possibleMoves[i];
            }
        }
        return 0;
    }
}
}