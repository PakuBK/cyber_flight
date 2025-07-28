using UnityEngine;

namespace CF.Data {
    public enum AttackSelectionMode
    {
        WeighedRandom, // Weighed random selection based on Attack Weight
        RandomAmongValid, // Random selection among valid attacks, ignoring weights
        WeightedRandomAmongValid, // Weighted random selection among valid attacks
        FirstValidByWeight // Selects the first valid attack based on weight
    }

    [CreateAssetMenu(fileName ="Enemy Data", menuName ="Data/Enemys/Base")]
    public class EnemyData : ScriptableObject
    {
        [Header("Enemy Settings")]
        public string Name;
        public Sprite Icon;

        [Header("Enemy Stats")]
        public float Health;
        public float MoveSpeed;
        [Range(0, 100)]
        public int PlayerFocusWeight = 1;
        [Range(0, 100)]
        public int AttackProbability = 1;
        public AttackSelectionMode AttackSelectionMode = AttackSelectionMode.WeightedRandomAmongValid;
        public float IdleWaitMin = 0.5f;
        public float IdleWaitMax = 1.5f;

        [Header("Drop Rates")]
        public int ShardsMin = 1;
        public int ShardsMax = 1;
        public BossFragment FragmentTier;
        public int FragmentAmount;
       

        [Header("Animation Clips")]

        public AnimationClip IdleAnim;
        public AnimationClip MoveAnim;
        public AnimationClip DeathAnim;

        public AnimationClip EntryClip;
        public AnimationClip AttackClip;
        public AnimationClip ExitClip;

        [Header("Hit Settings")]
        public Color HitColor = Color.white;

        [Header("Attack Moves")]
        public EnemyAttackData[] Attacks;

    }
}