using UnityEngine;
using System.Collections.Generic;

namespace CF.Data {
    public enum AttackPatternType { Sequential, Random, Conditional }

    [CreateAssetMenu(fileName = "Enemy Attack Pattern SO", menuName = "Data/Enemys/AttackPatternSO")]
    public class EnemyAttackPatternSO : ScriptableObject
    {
        [Header("Pattern Settings")]
        public AttackPatternType PatternType = AttackPatternType.Sequential;
        public List<EnemyAttackSO> Attacks = new List<EnemyAttackSO>();

        [Header("Pattern Conditions (Optional)")]
        public bool UseHealthThreshold;
        public float HealthThreshold = 0.5f; // Example: below 50% health
        // Add more pattern logic fields as needed
    }
}
