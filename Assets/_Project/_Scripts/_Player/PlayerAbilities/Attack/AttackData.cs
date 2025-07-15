using UnityEngine;
using CF.Audio;

namespace CF.Player {

[CreateAssetMenu(fileName = "Attack Ability", menuName = "Data/Player Data/Attack Data")]
public class AttackData : ScriptableObject
{
    public string DisplayName;
    [Header("Attack Settings")]
    public int BulletsPerAttack = 1;
    public float BulletTimer = 0.1f;
    public float Damage = 1f;
    [Space]
    public bool RandomSize;
    public bool RandomOffset;
    public bool RandomSpeed;
    public bool RandomDirection;
    [Space]
    [Header("Intervall Setting")]
    public bool Intervall;
    public int IntervallThreshold;
    public float IntervallTime;
    [Header("Speed Settings")]
    public float BulletSpeed = 10f;
    public float MinBulletSpeed;
    public float MaxBulletSpeed;
    [Header("Size Settings")]
    [Range(0.1f, 2f)]
    public float BulletSize = 0.5f;
    public float MinBulletSize;
    public float MaxBulletSize;
    [Header("Offset Settings")]
    [Tooltip("Use this for seperation of multiple bullets")]
    public bool FixedOffset;
    [Range(0, 1f)]
    public float OffsetMultiplier;
    [Header("Direction")]
    public Vector2 Direction;
    [Range(-1, 1)]
    public float MinDirectionX;
    [Range(-1, 1)]
    public float MaxDirectionX;
    [Header("Sprite")]
    public Sprite BulletSprite;
    public Sprite IconSprite;
    [ColorUsageAttribute(true,true)]
    public Color GlowColor;
    [Header("Audio")]
    public AudioOfType[] Attack_SFX;
}
}