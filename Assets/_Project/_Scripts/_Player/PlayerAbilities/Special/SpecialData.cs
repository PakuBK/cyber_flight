using UnityEngine;

namespace CF.Player {
[CreateAssetMenu(fileName = "Special Ability", menuName = "Data/Player Data/Special Data")]
public class SpecialData : ScriptableObject
{
    public string DisplayName;
    public bool Homing;

    public SpecialType Type;
    public float SpecialDuration;
    [Tooltip("The time it takes to shoot again after a special")]
    public float ShootAfterSpecialCooldown;
    public float Speed;
    public float Damage;
    public float Cooldown = 1f;
    public float Size;

    public Vector2 Direction;

    public Sprite Apperance;
    public Sprite Icon;
    public Sprite ParticleSprite;

    public Color CustomColor = Color.white;
}
}