using UnityEngine;

namespace CF.Player {
[CreateAssetMenu(fileName = "Defensive Ability", menuName = "Data/Player Data/Defensive Data")]
public class ShieldData : ScriptableObject
{
    public string Name = "name";

    [Header("Sprite")]
    public Sprite IconSprite;

    public float Duration;
    public float Hitpoints;
    public float Cooldown;
    public float InvincibilityTime = 0.05f;

    [Header("Animation Clips")]
    public AnimationClip shieldEntry;
    public AnimationClip shieldOn;
    public AnimationClip shieldHit;
    public AnimationClip shieldExit;

}
}