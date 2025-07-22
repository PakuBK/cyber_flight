using UnityEngine;
using CF.Audio;

namespace CF.Data {
    public enum EnemyAttackType
    {
        Bullet,
        Laser
        // Add more types as needed
    }

    public enum BulletCycleType
    {
        Singel,
        Cycle,
        Random
    }

    [CreateAssetMenu(fileName = "Enemy Attack Data", menuName = "Data/Enemys/Attacks")]
    public class EnemyAttackData : ScriptableObject
    {
        [Header("General")]
        public EnemyAttackType AttackType = EnemyAttackType.Bullet;

        [Header("Attack Settings")]
        public int[] AttackTargetOffset = {0};


        [Header("Telegraph Settings")]
        public float TelegraphDuration = 1f;

        [Header("Bullet Settings")]
        public BulletCycleType BulletIteration = BulletCycleType.Singel;
        public int AmountOfBullets = 1;
        public float TimeBetweenBullets = 0.1f;
        public bool OneColorForAll;
        
        [ColorUsageAttribute(true,true)]
        public Color AllBulletsColor;
        public BulletData[] Bullets;

        [Header("Laser Settings")]
        public GameObject LaserPrefab;
        public float LaserDuration = 2f;
        public float LaserDamagePerSecond = 10f;
        public int LaserTargetLane = 1;

        [Range(1,10)]
        public float Weight = 1;

        [Header("SFX")]
        public AudioOfType[] AttackSFX;
    }
}

