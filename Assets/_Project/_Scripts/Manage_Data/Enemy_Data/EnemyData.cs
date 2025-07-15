using UnityEngine;

namespace CF.Data {

[CreateAssetMenu(fileName ="Enemy Data", menuName ="Data/Enemys/Base")]
public class EnemyData : ScriptableObject
{
    public string Name;
    public Sprite Icon;
    public float Health;
    public float MoveSpeed;
    public float IdleTime;

    [Header("Drop Rates")]
    public int ShardsMin = 1;
    public int ShardsMax = 1;
    public BossFragment FragmentTier;
    public int FragmentAmount;

    public int PlayerFocusWeight = 1;
    [Range(0,100)]
    public int AttackProbabilty = 1;

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