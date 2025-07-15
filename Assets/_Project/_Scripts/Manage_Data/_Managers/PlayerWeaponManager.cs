using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using CF.Player;

namespace CF.Data {
using System;
[ExecuteInEditMode]
public class PlayerWeaponManager : MonoBehaviour
{
    public TextAsset WeaponAsset;

    public List<AttackData> PlayerWeapons;

    [SerializeField]
    private bool OverrideAssetCreation;

    #region Singelton
    public static PlayerWeaponManager Instance = null;

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

        LoadPlayerWeapons();
    }

    #endregion

    #region Interface

    public AttackData GetWeaponByIdx(int idx)
    {
        return PlayerWeapons[idx];
    }

    public bool IsWeaponOwnedByIdx(int idx) {
        var attacks_owned = DataController.LoadPlayerAttacksOwned();
        return attacks_owned.Contains(PlayerWeapons[idx].name);
    }

    public bool IsWeaponEquipedByIdx(int idx) {
        var attack_equiped = DataController.LoadPlayerAttack();
        return PlayerWeapons[idx].name == attack_equiped.name;
    }

    #endregion

    #region Load Data

    public void LoadPlayerWeapons()
    {
        List<AttackData> weapons = new List<AttackData>();

        PlayerWeapons = Resources.LoadAll<AttackData>("PlayerAttacks").ToList();
    }

    private AttackData LoadWeaponData(string row)
    {
        var columns = row.Split(',');

        string asset_name = columns[0] + "_attack";

        var data = Resources.Load<AttackData>($"PlayerAttacks/{asset_name}");

        if (data is null) Debug.LogError($"[{asset_name}] was not found.");

        return data;
    }

    #endregion

    string[] SplitCSV() // thanks infallible code
    {
        return WeaponAsset.text.Split(
            new[] { Environment.NewLine },
            StringSplitOptions.None).
            Skip(1).ToArray();
    }

    #if UNITY_EDITOR
    #region Weapon Object Creation

    public void CreateAllWeapons()
    {
        var rows = SplitCSV();

        foreach (var row in rows)
        {
            CreateAttack(row);
        }
    }

    private void CreateAttack(string row)
    {
        var columns = row.Split(',');

        string asset_name = columns[0] + "_attack";

        if (!OverrideAssetCreation && Resources.Load<AttackData>($"PlayerAttacks/{asset_name}") != null)
        {
            Debug.Log($"Scriptable Object [{asset_name}] already exist");
            return;
        }

        AttackData asset = ScriptableObject.CreateInstance<AttackData>();

        asset.name = asset_name;
        asset.DisplayName = columns[1];
        asset.BulletsPerAttack = int.Parse(columns[2]);
        asset.BulletTimer = float.Parse(columns[3]);
        asset.Damage = float.Parse(columns[4]);
        asset.RandomSize = bool.Parse(columns[5]);
        asset.RandomOffset = bool.Parse(columns[6]);
        asset.RandomSpeed = bool.Parse(columns[7]);
        asset.RandomDirection = bool.Parse(columns[8]);

        asset.Intervall = bool.Parse(columns[9]);
        asset.IntervallThreshold = int.Parse(columns[10]);
        asset.IntervallTime = float.Parse(columns[11]);

        asset.BulletSpeed = float.Parse(columns[12]);
        asset.MinBulletSpeed = float.Parse(columns[13]);
        asset.MaxBulletSpeed = float.Parse(columns[14]);
        asset.BulletSize = float.Parse(columns[15]);
        asset.MinBulletSize = float.Parse(columns[16]);
        asset.MaxBulletSize = float.Parse(columns[17]);

        asset.FixedOffset = bool.Parse(columns[18]);
        asset.OffsetMultiplier = float.Parse(columns[19]);
        asset.Direction = new Vector2(float.Parse(columns[20]), 0);
        asset.MinDirectionX = float.Parse(columns[21]);
        asset.MaxDirectionX = float.Parse(columns[22]);

        // This will break if you change the bullet rooster
        var bulletSprites = Resources.LoadAll<Sprite>("Textures/Bullets/bullet_rooster");

        foreach (var s in bulletSprites) {
            if (s.name == columns[23]){
                asset.BulletSprite = s;
                break;
            }
        }

        var iconSprite = Resources.Load<Sprite>($"Textures/PlayerAttacks/Icons/{asset_name}_icon");

        if (iconSprite == null) Debug.LogWarning($"Attack [{asset_name}] could not load Icon sprite.");

        asset.IconSprite = iconSprite;

        AssetDatabase.CreateAsset(asset, $"Assets/Resources/PlayerAttacks/{asset_name}.asset");
        AssetDatabase.SaveAssets();

    }

    #endregion
    #endif
}
}