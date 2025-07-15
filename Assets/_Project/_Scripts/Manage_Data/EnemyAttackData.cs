using UnityEngine;
using CF.Audio;

namespace CF.Data {
[CreateAssetMenu(fileName = "Enemy Attack Data", menuName = "Data/Enemys/Attacks")]
public class EnemyAttackData : ScriptableObject
{
    [Header("Attack Settings")]
    public int AmountOfRainBullets = 1;
    public int[] AttackTargetOffset = {0};
    public float BulletRainTimer = 0.1f;
    [Header("Bullet Settings")]
    public bool OneColorForAll;
    [ColorUsageAttribute(true,true)]
    public Color AllBulletsColor;
    public BulletData[] Bullets;
    [Range(1,10)]
    public float Weight = 1;
    [Header("SFX")]
    public AudioOfType[] AttackSFX;
}
}

