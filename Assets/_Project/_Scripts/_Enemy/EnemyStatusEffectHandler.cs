using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public struct StatusEffectInstance
{
    public StatusEffect Type;
    public float Duration;
    public float TimeApplied;
    public bool IsActive => Time.time < TimeApplied + Duration;
}

public enum StatusEffect
{
    Stunned,
    Slowed,
    Poisoned,
    Frozen
}

public class EnemyStatusEffectHandler : MonoBehaviour
{
    private List<StatusEffectInstance> activeEffects = new List<StatusEffectInstance>();

    public void ApplyEffect(StatusEffect type, float duration)
    {
        activeEffects.RemoveAll(e => e.Type == type);
        activeEffects.Add(new StatusEffectInstance { Type = type, Duration = duration, TimeApplied = Time.time });
    }

    public bool HasActiveEffect(StatusEffect type)
    {
        return activeEffects.Any(e => e.Type == type && e.IsActive);
    }

    private void Update()
    {
        activeEffects.RemoveAll(e => !e.IsActive);
    }
}