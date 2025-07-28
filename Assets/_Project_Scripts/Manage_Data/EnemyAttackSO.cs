using UnityEngine;
using CF.Audio;
using System;

namespace CF.Data {
    [CreateAssetMenu(fileName = "Enemy Attack SO", menuName = "Data/Enemys/AttackSO")]
    public class EnemyAttackSO : ScriptableObject
    {
        [Header("Telegraph Settings")]
        public float TelegraphDuration = 0.5f;
        public AnimationClip TelegraphAnim;
        public AudioOfType TelegraphSFX;
        public Color TelegraphColor = Color.yellow;

        [Header("Attack Settings")]
        public float AttackDelay = 0.1f;
        public float Cooldown = 0.5f;
        public int[] TargetLanes = { 0 };
        public BulletData[] Bullets;
        public AudioOfType AttackSFX;
    }
}
