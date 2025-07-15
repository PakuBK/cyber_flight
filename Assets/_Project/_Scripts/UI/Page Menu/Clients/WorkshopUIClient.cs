using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using CF.Data;
using CF.Player;

namespace CF.UI {
public class WorkshopUIClient : MonoBehaviour
{
    #region Singelton
    public static WorkshopUIClient Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Page Variables
    [Serializable]
    private enum SubPage {
        Weapons, Shields, Specials
    }

    private SubPage page = SubPage.Weapons; 
    private SubPage[] pageOrder = {SubPage.Weapons, SubPage.Shields, SubPage.Specials};
    private int curWeaponIdx, curShieldIdx, curSpecialIdx;
    #endregion

    #region UI References

    [SerializeField]
    private TextMeshProUGUI sectionTitle;
    [SerializeField]
    private Image viewBox;
    [SerializeField]
    private TextMeshProUGUI displayName;
    [SerializeField]
    private Button useButton;

    [Header("Attack Data")]
    [SerializeField]
    private RectTransform[] AttackFields;

    [Header("Shield Data")]
    [SerializeField]
    private RectTransform[] ShieldFields;

    [Header("Special Data")]
    [SerializeField]
    private RectTransform[] SpecialFields;

    // Attack UI Fields
    [SerializeField]
    private RectTransform bulletSpeedDisplay;
    [SerializeField]
    private RectTransform bulletsPerAttackDisplay;
    [SerializeField]
    private RectTransform attackSpeedDisplay;
    [SerializeField]
    private RectTransform attackDamageDisplay;

    // Shield UI Fields
    [SerializeField]
    private RectTransform shieldHealthDisplay;
    [SerializeField]
    private RectTransform shieldDurationDisplay;
    [SerializeField]
    private RectTransform shieldCooldownDisplay;

    // Special UI Fields
    [SerializeField]
    private RectTransform specialHomingIcon;
    [SerializeField]
    private RectTransform specialNotHomingIcon;
    [SerializeField]
    private RectTransform specialFreezeIcon;
    [SerializeField]
    private RectTransform specialStunIcon;
    [SerializeField]
    private RectTransform specialNormalIcon;
    [SerializeField]
    private RectTransform specialCooldownDisplay;
    #endregion
    
    private List<RectTransform> tempStatPanels;

    private void OnEnable()
    {
        tempStatPanels = new List<RectTransform>();
        page = SubPage.Weapons;
        LoadContent();
    }

    private void Update() {
        if (Application.platform == RuntimePlatform.Android && UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            PageController.instance.CloseFullPage();
        }
    }


    #region Navigate Functions

    public void NextSection() {
        int nextPageIdx = Array.FindIndex(pageOrder, x => x == page) + 1;
        nextPageIdx = nextPageIdx >= pageOrder.Length ? 0 : nextPageIdx;
        page = pageOrder[nextPageIdx];
        LoadContent();
    }

    public void PreviousSection() {
        int nextPageIdx = Array.FindIndex(pageOrder, x => x == page) - 1;
        nextPageIdx = nextPageIdx < 0 ? pageOrder.Length -1  : nextPageIdx;
        page = pageOrder[nextPageIdx];
        LoadContent();
    }

    
    public void NextItem()
    {
        switch(page)
        {
            case SubPage.Weapons:
            {
                curWeaponIdx++;
                LoadWeapon();
                break;
            }
            case SubPage.Shields:
            {
                curShieldIdx++;
                LoadShield();
                break;
            }
            case SubPage.Specials : {
                curSpecialIdx++;
                LoadSpecial();
                break;
            }
            default: break;
        }
    }

    public void PreviousItem()
    {
        switch(page)
        {
            case SubPage.Weapons:
            {
                curWeaponIdx--;
                LoadWeapon();
                break;
            }
            case SubPage.Shields:
            {
                curShieldIdx--;
                LoadShield();
                break;
            }
            case SubPage.Specials : {
                curSpecialIdx--;
                LoadSpecial();
                break;
            }
            default: break;
        }
    }

    #endregion

    #region Load Content 

    private void LoadContent()
    {
        UnloadStats();
        switch(page)
        {
            case SubPage.Weapons:
            {
                sectionTitle.text = "weapons";
                LoadWeapon();
                break;
            }
            case SubPage.Shields:
            {
                sectionTitle.text = "shields";
                LoadShield();
                break;
            }
            case SubPage.Specials:
            {
                sectionTitle.text = "specials";
                LoadSpecial();
                break;
            }
            default: break;
        }
    }

    private void LoadWeapon()
    {
        var amountOfWeapons = PlayerWeaponManager.Instance.PlayerWeapons.Count;
        if (curWeaponIdx >= amountOfWeapons || curWeaponIdx < 0)
        {
            curWeaponIdx = curWeaponIdx == amountOfWeapons ? 0 : amountOfWeapons - 1;
        }
        var weapon = PlayerWeaponManager.Instance.GetWeaponByIdx(curWeaponIdx);
        bool isOwned = PlayerWeaponManager.Instance.IsWeaponOwnedByIdx(curWeaponIdx);
        // displays
        viewBox.sprite = weapon.IconSprite;
        displayName.text = weapon.DisplayName;

        if (!isOwned)
        {
            viewBox.color = Color.black;
        }
        else 
        {
            viewBox.color = Color.white;
        }

        TurnButtonOff(PlayerWeaponManager.Instance.IsWeaponEquipedByIdx(curWeaponIdx) || !isOwned);
        
        LoadWeaponStats(weapon);
    }

    private void LoadShield()
    {
        var amountOfShields = PlayerShieldManager.Instance.PlayerShields.Count;
        if (curShieldIdx >= amountOfShields || curShieldIdx < 0)
        {
            curShieldIdx = curShieldIdx == amountOfShields ? 0 : amountOfShields - 1;
        }
        var currentShield = PlayerShieldManager.Instance.GetShieldByIdx(curShieldIdx);
        var isOwned = PlayerShieldManager.Instance.IsShieldOwnedByIdx(curShieldIdx);
        // displays
        viewBox.sprite = currentShield.IconSprite;
        displayName.text = currentShield.Name;

        if (!isOwned)
        {
            viewBox.color = Color.black;
        }
        else 
        {
            viewBox.color = Color.white;
        }

        TurnButtonOff(PlayerShieldManager.Instance.IsShieldEquipedByIdx(curShieldIdx) || !isOwned);

        LoadShieldStats(currentShield);
    }

    private void LoadSpecial()
    {
        var amountOfSpecials = PlayerSpecialManager.Instance.PlayerSpecials.Count;
        if (curSpecialIdx >= amountOfSpecials || curSpecialIdx < 0)
        {
            curSpecialIdx = curSpecialIdx == amountOfSpecials ? 0 : amountOfSpecials - 1;
        }
        var currentSpecial = PlayerSpecialManager.Instance.GetSpecialForIdx(curSpecialIdx);
        var isOwned = PlayerSpecialManager.Instance.IsSpecialOwnedByIdx(curSpecialIdx);
        // displays
        viewBox.sprite = currentSpecial.Icon;
        displayName.text = currentSpecial.DisplayName;

        if (!isOwned)
        {
            viewBox.color = Color.black;
        }
        else 
        {
            viewBox.color = Color.white;
        }

        TurnButtonOff(PlayerSpecialManager.Instance.IsSpecialEquipedByIdx(curSpecialIdx) || !isOwned);

        LoadSpecialStats(currentSpecial);
    }
    #endregion

    #region Load Stats 

    private void LoadWeaponStats(AttackData weapon) {
        foreach (var item in AttackFields)
        {
            item.gameObject.SetActive(true);
            tempStatPanels.Add(item);
        }
        attackDamageDisplay.sizeDelta = new Vector2(100 * Mathf.Clamp((float)weapon.Damage, 0f, 4f), 100);
        attackSpeedDisplay.sizeDelta = new Vector2(100 * (5-(Mathf.Clamp(weapon.BulletTimer * 10, 0f, 4f))), 100);
        bulletSpeedDisplay.sizeDelta = new Vector2(100 * (weapon.BulletSpeed / 15), 100);
        bulletsPerAttackDisplay.sizeDelta = new Vector2(100 * weapon.BulletsPerAttack, 100);
    }

    private void LoadShieldStats(ShieldData shield) {
        foreach (var item in ShieldFields)
        {
            item.gameObject.SetActive(true);
            tempStatPanels.Add(item);
        }
        shieldCooldownDisplay.sizeDelta = new Vector2(100 * Mathf.Clamp((float)shield.Cooldown, 0f, 4f), 100);
        shieldHealthDisplay.sizeDelta = new Vector2(100 * Mathf.Clamp((float)shield.Hitpoints/10f, 0f, 4f), 100);
        shieldDurationDisplay.sizeDelta = new Vector2(100 * Mathf.Clamp((float)shield.Duration, 0f, 4f), 100);
    }

    private void LoadSpecialStats(SpecialData special) {
        foreach (var item in SpecialFields)
        {
            item.gameObject.SetActive(true);
            tempStatPanels.Add(item);
        }
        specialCooldownDisplay.sizeDelta = new Vector2(100 * Mathf.Clamp((float)special.Cooldown, 0f, 4f), 100);
        specialFreezeIcon.gameObject.SetActive(false);
        specialStunIcon.gameObject.SetActive(false);
        specialNormalIcon.gameObject.SetActive(false);
        specialHomingIcon.gameObject.SetActive(false);
        specialNotHomingIcon.gameObject.SetActive(false);
        switch (special.Type)
        {
            case SpecialType.None:
                specialNormalIcon.gameObject.SetActive(true);
                break;
            case SpecialType.Slow:
                specialFreezeIcon.gameObject.SetActive(true);
                break;
            case SpecialType.Stun:
                specialStunIcon.gameObject.SetActive(true);
                break;
            default:
                break;
        }

        if (special.Homing)
        {
            specialHomingIcon.gameObject.SetActive(true);
        }
        else 
        {
            specialNotHomingIcon.gameObject.SetActive(true);
        }
    }

    private void UnloadStats() {
        foreach (var item in tempStatPanels)
        {
            item.gameObject.SetActive(false);
        }
        tempStatPanels.Clear();
    }

    #endregion

    #region Use and Unuse Functionality

    public void UseItem() {
        switch (page)
        {
            case SubPage.Weapons:
                EquipWeapon();
                break;
            case SubPage.Shields:
                EquipShield();
                break;
            case SubPage.Specials:
                EquipSpecial();
                break;
            default:
                break;
        }
    }

    private void EquipWeapon() {
        var currentWeapon = PlayerWeaponManager.Instance.GetWeaponByIdx(curWeaponIdx);
        DataController.SavePlayerAttack(currentWeapon);
        TurnButtonOff(true);
    }

    private void EquipShield() {
        var currentShield = PlayerShieldManager.Instance.GetShieldByIdx(curShieldIdx);
        DataController.SavePlayerShield(currentShield);
        TurnButtonOff(true);
    }

    private void EquipSpecial() {
        var currentSpecial = PlayerSpecialManager.Instance.GetSpecialForIdx(curSpecialIdx);
        DataController.SavePlayerSpecial(currentSpecial);
        TurnButtonOff(true);
    }
    #endregion

    private void TurnButtonOff(bool state) {
        useButton.enabled = !state;
        if (state)
        {
            useButton.targetGraphic.color = Color.grey;
        }
        else {
            useButton.targetGraphic.color = Color.white;
        }
    } 
    
}
}