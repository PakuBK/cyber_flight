using UnityEngine;
using CF.Audio;
using System.ComponentModel;

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

    public enum AttackTargetType
    {
        SingleLane,
        TwoLanes,
        RandomLane
    }

    [CreateAssetMenu(fileName = "Enemy Attack Data", menuName = "Data/Enemys/Attacks")]
    public class EnemyAttackData : ScriptableObject
    {
        [Header("General")]
        public EnemyAttackType AttackType = EnemyAttackType.Bullet;

        [Header("Target Settings")]
        [Range(-1, 1)]
        [Tooltip("The lanes that this attack will hit relativ to the player. -1 = left, 0 = middle, 1 = right")]
        public int[] TargetOffset = {0};
        [Tooltip("How many Lanes will this attack effect")]
        public AttackTargetType TargetType = AttackTargetType.SingleLane;
        [Range(1, 10)]
        public int Weight = 1;


        [Header("Telegraph Settings")]
        public float TelegraphDuration = 0.5f;

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

        

        [Header("SFX")]
        public AudioOfType[] AttackSFX;
    }
}

