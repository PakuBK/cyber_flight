using UnityEngine;
using CF.Player;
using CF.UI;
using CF.Audio;
using CF.Data;

namespace CF.Enemy {

public class EnemyBrain : MonoBehaviour
{
    public EnemyData Data;

    private Rigidbody2D rb;
    public SpriteRenderer spriteRenderer { get; private set; }

    private EnemyStateMachine StateMachine;

    public EnemyMoveState MoveState;
    public EnemyIdleState IdleState;
    public EnemyAttackState AttackState;

    [HideInInspector]
    public float[] laneAnchorPoints;

    [HideInInspector]
    public int currentLane;

    [HideInInspector]
    public Animator Anim
    {
        get
        {
            if (_anim == null)
            {
                _anim = GetComponent<Animator>();
            }
            return _anim;
        } 
        private set
        {
            _anim = value;
        }
    }

    private Animator _anim;

    public LaneController PlayerController;

    private float health;
    private bool isAlive = true;

    #region Hit Variables
    private bool wasHit;
    private float hitStartTime;
    private float hitTime = 0.1f;
    #endregion

    #region Special Variables
    public bool isStunded { get; private set; }
    public bool IsSlowed { get; private set; }

    public float specialTime { get; private set; }
    public float specialStartTime { get; private set; }
    public float specialMultiplier { get; private set; }
    #endregion

    #region Blink Variables
    private float blinkingStartTime;
    private Color currentBlinkColor;

    public Color HitColor;
    public Color SlowColor;
    public Color StunColor;

    #endregion

    #region Animation Variables

    [HideInInspector]
    public Material enemyMaterial;

    #endregion

    #region Unity Functions

    private void Start()
    {
        laneAnchorPoints = SetupScene.Current.laneAnchorPoints;

        transform.position = new Vector3(laneAnchorPoints[1], transform.position.y, transform.position.z);

        SetupScene.Current.ScaleObject(transform);

        enemyMaterial = GetComponent<SpriteRenderer>().material;
    }

    private void Update()
    {
        StateMachine.currentState.LogicUpdate();

        if (wasHit)
        {
            if (Time.time > hitStartTime + hitTime)
            {
                HitOver();
            }
        }
    }

    private void FixedUpdate()
    {
        StateMachine.currentState.PhysicsUpdate();
    }

    #endregion

    #region Handle Enemy Data

    public void Initialize()
    {
        _anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        StateMachine = new EnemyStateMachine();

        MoveState = new EnemyMoveState(this, StateMachine);
        IdleState = new EnemyIdleState(this, StateMachine);
        AttackState = new EnemyAttackState(this, StateMachine);

        health = Data.Health;

        currentBlinkColor = Color.white;
    }

    public void LoadEnemy(EnemyData _data)
    {
        Data = _data;

        IdleState.LockState(false);
        MoveState.LockState(false);
        AttackState.LockState(false);

        spriteRenderer.color = Color.white;

        isAlive = true;
        health = Data.Health;

        StateMachine.Initialize(IdleState);

        CheckHealth();
    }

    #endregion

    #region Interactions

    public void SetXVelocity(float _velocity) 
    {
        rb.velocity = new Vector2(_velocity, rb.velocity.y);
    }

    public void ProcessHit(float _damage)
    {
        health -= _damage;

        currentBlinkColor = spriteRenderer.color;
        spriteRenderer.color = HitColor;

        if (!wasHit)
        {
            hitStartTime = Time.time;
        }

        wasHit = true;

        CheckHealth();
    }
    public void ProcessSpecial(SpecialType _type, float _damage, float _time)
    {
        float concreteDamage = _damage; 

        if (!isAlive) return;

        switch (_type)
        {
            case SpecialType.None:
                break;
            case SpecialType.Stun:
                specialStartTime = Time.time;
                blinkingStartTime = Time.time;
                specialTime = _time;
                StunEnemy(true);
                break;
            case SpecialType.Slow:
                specialStartTime = Time.time;
                specialTime = _time;
                specialMultiplier = 1 / _damage;
                SlowEnemy(true);

                spriteRenderer.color = SlowColor;
                break;
        }

        ProcessHit(concreteDamage);
    }

    public void StunEnemy(bool _state)
    {
        isStunded = _state;
    }

    public void SlowEnemy(bool _state)
    {
        IsSlowed = _state;
    }

    private void CheckHealth()
    {
        UIController.current.UpdateEnemyHealth(health, Data.Health);

        if (health <= 0 && isAlive)
        {
            IdleState.LockState(true);
            MoveState.LockState(true);
            AttackState.LockState(true);

            PlaySoundOnDeath();

            spriteRenderer.color = Color.white;
            isAlive = false;
            Anim.Play(Data.DeathAnim.name);
        }
    }
        

    #endregion

    #region Visual Interactions

    public void FlashColorEffect(Color _color, float _flashTime = 0.2f)
    {
        if (Time.time > blinkingStartTime + _flashTime)
        {
            if (currentBlinkColor == Color.white)
            {
                currentBlinkColor = _color;
            }
            else if (currentBlinkColor == _color)
            {
                currentBlinkColor = Color.white;
            }
            spriteRenderer.color = currentBlinkColor;
            blinkingStartTime = Time.time;
        }
    }


    public void EntryAnimAttackTrigger(){
        AttackState.EntryAnimTrigger();
    }

    public void ExitAnimAttackTrigger(){
        AttackState.ExitAnimTrigger();
    }

    public void DeathAnimOver()
    {
        GameEvents.Current.EnemyDeathEnter();
    }

    private void HitOver()
    {
        spriteRenderer.color = currentBlinkColor;
        wasHit = false;
    }

    public void ResetPos()
    {
        currentLane = 1;
        transform.position = new Vector3(laneAnchorPoints[currentLane], transform.position.y, transform.position.z);
    }

    #endregion

    #region Audio Interaction

    private void PlaySoundOnDeath()
    {
        // unique explosion sound for different enemy
        var standard = AudioOfType.SFX_Explosion_01;
        AudioController.Current.PlayAudio(standard, true);

    }

    #endregion
}
}