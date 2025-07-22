using CF.Data;
using UnityEngine;

namespace CF.Enemy
{
    /// <summary>
    /// The EnemyContext class is responsible for storing the current Context for the enemy state in the game.
    /// 
    /// </summary>
    public class EnemyContext : MonoBehaviour
    {
        public Transform transform { get; private set; }
        public SpriteRenderer spriteRenderer { get; private set; }

        private EnemyManager enemyManager;

        private EnemyStatusEffectHandler statusEffectHandler;

        public EnemyMovementController movementController { get; private set; }

        public Lane currentLane { get; private set; } = Lane.Middle;


        public EnemyData enemyData => enemyManager.GetCurrentEnemyData();


        private void Awake()
        {
            Initialize();
        }
        public void Initialize()
        {
            enemyManager = GetComponent<EnemyManager>();
            statusEffectHandler = GetComponent<EnemyStatusEffectHandler>();
            movementController = GetComponent<EnemyMovementController>();
            transform = GetComponent<Transform>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            currentLane = Lane.Middle;
        }

        public bool HasActiveEffect(StatusEffect effect)
        {
            return statusEffectHandler.HasActiveEffect(effect);
        }

        public void SetCurrentLane(Lane lane)
        {
            currentLane = lane;
        }

    }

    public enum Lane
    {
        Left,
        Middle,
        Right
    }

    
}
