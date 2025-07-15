using UnityEngine;
using CF.UI;
using CF.Data;

namespace CF.Player {
public class AttackController : MonoBehaviour
{
    public bool IsDebug;

    #region Passive Attack
    [Header("Passive Attack")][SerializeField]
    private AttackData attackData;
    private NormalAbility normal;
    #endregion

    #region Special Attack
    [Header("Special Attack")] [SerializeField]
    private SpecialData specialData;
    private SpecialAbility specialAbility;
    [SerializeField]
    private GameObject specialPrefab;
    #endregion

    [HideInInspector]
    public bool IsShooting;

    [HideInInspector]
    public bool locked;

    private void Awake()
    {
        if (!IsDebug)
        {
            attackData = DataController.LoadPlayerAttack();

            specialData = DataController.LoadPlayerSpecial();
        }

        normal = new NormalAbility(transform, attackData);
        specialAbility = new SpecialAbility(transform, specialPrefab, specialData);

    }

    private void Start()
    {
        GameEvents.Current.onSpecialAbility += specialAbility.Enter;
    }

    private void Update()
    {
        if (locked) return;

        // this can be optimized but it will make it uglier
        if (ShouldShoot())
        {
            normal.AbilityUpdate();
        }

        UIController.current.UpdatePlayerSpecial(Time.time - specialAbility.startTime, specialAbility.Data.Cooldown);
    }

    public void ToggleNormalAbility(bool state)
    {
        IsShooting = state;
    }

    private bool ShouldShoot() => IsShooting && specialAbility.CanShoot();

}
}