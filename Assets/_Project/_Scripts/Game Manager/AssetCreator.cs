using UnityEngine;
using UnityEditor;
using CF.Player;

[ExecuteInEditMode]
[CreateAssetMenu(fileName = "JSON Util", menuName = "Util/JSON")]
public class AssetCreator : ScriptableObject
{
    public bool OverrideAssetCreation;

    #region Shield Creation
    public TextAsset shieldJSON;

    [System.Serializable]
    public class ShieldJSONFormat {
        public string Name = "name";
        public string ShieldSprite;
        public float Duration;
        public float Hitpoints;
        public float Cooldown;
        public float InvincibilityTime;
    }
    [System.Serializable]
    public class ShieldContainer {
         public ShieldJSONFormat[] shields;
    }

    public ShieldContainer GetShieldData() {
        ShieldContainer shieldContainer = new ShieldContainer();
        shieldContainer = JsonUtility.FromJson<ShieldContainer>(shieldJSON.text);
        return shieldContainer;
    }

    #if UNITY_EDITOR

    public void CreateShieldAssets() {
        ShieldContainer shieldContainer = GetShieldData();

        foreach (var shield in shieldContainer.shields)
        {
            ShieldData asset = ScriptableObject.CreateInstance<ShieldData>();
            var asset_name = shield.Name.ToLower() + "_shield";

            if (!OverrideAssetCreation && Resources.Load<ShieldData>($"PlayerShields/{asset_name}") != null)
            {
                Debug.Log($"Scriptable Object [{asset_name}] already exist");
                return;
            }
            
            asset.name = asset_name;
            asset.Name = shield.Name;
            asset.Duration = shield.Duration;
            asset.Hitpoints = shield.Hitpoints;
            asset.Cooldown = shield.Cooldown;
            asset.InvincibilityTime = shield.InvincibilityTime;

            Sprite iconSprite = Resources.Load<Sprite>($"Textures/PlayerShields/Icons/{shield.Name}_shield_icon");

            if (iconSprite == null) Debug.LogWarning($"Shield [{shield.Name}] could not load Icon sprite.");

            asset.IconSprite = iconSprite;

            asset.shieldEntry = Resources.Load<AnimationClip>("Animations/Player/Shields/Default/shieldEntry");
            asset.shieldExit = Resources.Load<AnimationClip>("Animations/Player/Shields/Default/shieldExit");
            asset.shieldHit = Resources.Load<AnimationClip>("Animations/Player/Shields/Default/shieldHit");
            asset.shieldOn = Resources.Load<AnimationClip>("Animations/Player/Shields/Default/shieldOn");
            
            AssetDatabase.CreateAsset(asset, $"Assets/Resources/PlayerShields/{asset.name}.asset");
            AssetDatabase.SaveAssets();
        }

        Debug.Log("Remember that default animations are used");
    }

    #endif
    #endregion

    #region Specials Creation

    public TextAsset specialJSON;

    [System.Serializable]
    public class SpecialJSONFormat {
        public string Name;
        public bool Homing;
        public int Type;
        public float SpecialDuration;
        public float ShootAfterSpecialCooldown;
        public float Speed;
        public float Damage;
        public float Cooldown;
        public float Size;
        public int Direction;
        public string SpritePath;

    }
    [System.Serializable]
    public class SpecialContainer {
         public SpecialJSONFormat[] specials;
    }

    public SpecialContainer GetSpecialData() {
        SpecialContainer specialContainer = new SpecialContainer();
        specialContainer = JsonUtility.FromJson<SpecialContainer>(specialJSON.text);
        return specialContainer;
    }

    #if UNITY_EDITOR

    public void CreateSpecialAssets() {
        SpecialContainer specialContainer = GetSpecialData();
        foreach (var s in specialContainer.specials)
        {
            SpecialData asset = ScriptableObject.CreateInstance<SpecialData>();
            var asset_name = s.Name + "_special";

            if (!OverrideAssetCreation && Resources.Load<SpecialData>($"PlayerSpecials/{asset_name}") != null)
            {
                Debug.Log($"Scriptable Object [{asset_name}] already exist");
                return;
            }

            asset.name = asset_name;
            asset.DisplayName = s.Name;
            asset.Homing = s.Homing;
            asset.Type = (SpecialType) s.Type;
            asset.SpecialDuration = s.SpecialDuration;
            asset.ShootAfterSpecialCooldown = s.ShootAfterSpecialCooldown;
            asset.Speed = s.Speed;
            asset.Damage = s.Damage;
            asset.Cooldown = s.Cooldown;
            asset.Size = s.Size;
            asset.Direction = new Vector2(s.Direction, 1);

            var iconSprite = Resources.Load<Sprite>($"Textures/PlayerSpecials/Icons/{asset_name.ToLower()}_icon");

            if (iconSprite == null) Debug.LogWarning($"Special [{s.Name}] could not load Icon sprite.");

            asset.Icon = iconSprite;

            var ApperanceSprite = Resources.Load<Sprite>(s.SpritePath);

            if (ApperanceSprite == null) Debug.LogWarning($"Special [{s.Name}] could not load automatically Appearance sprite.");

            asset.Apperance = ApperanceSprite;

            var ParticleSprite = Resources.Load<Sprite>($"Textures/PlayerSpecials/Particles/{asset_name}_sprite");

            if (ParticleSprite == null) Debug.LogWarning($"Special [{s.Name}] could not automatically load Particle sprite.");

            asset.ParticleSprite = ParticleSprite;

            AssetDatabase.CreateAsset(asset, $"Assets/Resources/PlayerSpecials/{asset_name.ToLower()}.asset");
            AssetDatabase.SaveAssets();
        }
    }

    #endif

    #endregion
}
