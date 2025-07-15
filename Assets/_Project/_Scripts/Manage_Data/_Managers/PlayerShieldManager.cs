using CF.Player;
using UnityEngine;
using System.Collections.Generic;

namespace CF.Data {
public class PlayerShieldManager : MonoBehaviour
{
    #region Singelton
    public static PlayerShieldManager Instance;

    private void Awake() {
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

    #region Interface

    private List<ShieldData> playerShields = null;

    public List<ShieldData> PlayerShields {
        get => playerShields == null ? LoadPlayerShields() : playerShields;
        private set {
            playerShields = value;
        }
    }

    public ShieldData GetShieldByIdx(int idx) {
        return PlayerShields[idx];
    }

    public bool IsShieldOwnedByIdx(int idx) {
        var shields_owned = DataController.LoadPlayerShieldsOwned();
        return shields_owned.Contains(PlayerShields[idx].name);
    }

    public bool IsShieldEquipedByIdx(int idx) {
        var shield_equiped = DataController.LoadPlayerShield();
        return PlayerShields[idx].name == shield_equiped.name;
    }

    #endregion

    #region Load Data

    private List<ShieldData> LoadPlayerShields()
    {
        List<ShieldData> shields = new List<ShieldData>();
        
        var shieldAssets = Resources.LoadAll("PlayerShields", typeof(ShieldData));

        foreach (var asset in shieldAssets)
        {
            var shield = (ShieldData) asset;
            shields.Add(shield);
        }

        playerShields = shields;
        return shields;
    }

    #endregion
}
}
