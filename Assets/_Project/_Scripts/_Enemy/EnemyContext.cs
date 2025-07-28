using CF.Data;
using CF.Player;
using UnityEngine;

namespace CF.Enemy
{
    /// <summary>
    /// The EnemyContext class is responsible for storing the current Context for the enemy state in the game.
    /// 
    /// </summary>
    public class EnemyContext : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer { get; private set; }

        [SerializeField]
        private EnemyManager enemyManager;

        [SerializeField]
        private LaneController playerLaneController;

        private EnemyStatusEffectHandler statusEffectHandler;

        public EnemyMovementController movementController { get; private set; }

        public Lane currentLane { get; private set; } = Lane.Middle;


        public EnemyData enemyData => enemyManager.GetCurrentEnemyData();


        private void Awake()
        {
            Initialize();
        }
        private void Initialize()
        {
            statusEffectHandler = GetComponent<EnemyStatusEffectHandler>();
            movementController = GetComponent<EnemyMovementController>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            currentLane = Lane.Middle;
        }

        /// <summary>
        /// Checks if the enemy has an active status effect.
        /// </summary>
        /// <remarks>Checks status effect via the StatusEffectHandler Component.</remarks>
        /// <param name="effect">Type of the Status Effect to be applied.</param>
        /// <returns>True if the enemy has this effect.</returns>
        public bool HasActiveEffect(StatusEffect effect)
        {
            return statusEffectHandler.HasActiveEffect(effect);
        }

        public void SetCurrentLane(Lane lane)
        {
            currentLane = lane;
        }

        public int GetPlayerLaneIndex()
        {
            return playerLaneController.CurrentLane;
        }

    }

    public enum Lane
    {
        Left,
        Middle,
        Right
    }

    
}
