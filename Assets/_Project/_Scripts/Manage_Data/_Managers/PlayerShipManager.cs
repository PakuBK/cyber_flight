using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CF.Data {

using System;
[ExecuteInEditMode]
public class PlayerShipManager : MonoBehaviour
{
    public TextAsset PlayerShipCSV;

    public List<PlayerData> PlayerShips;

    #region Singelton
    public static PlayerShipManager Instance = null;

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

        PlayerShips = LoadPlayerShips();
    }

    #endregion


    #region Load Data


    public List<PlayerData> LoadPlayerShips()
    {
        List<PlayerData> ships = new List<PlayerData>();

        var rows = SplitCSV();

        foreach (var row in rows)
        {
            var data = LoadShipData(row);
            ships.Add(data);
        }

        return ships;
    }

    private PlayerData LoadShipData(string row)
    {
        var columns = row.Split(',');

        string asset_name = columns[0] + "_ship";

        var data = Resources.Load<PlayerData>($"PlayerShips/{asset_name}");

        if (data is null) Debug.LogError($"[{asset_name}] was not found.");

        return data;
    }

    #endregion

    #region Ship Object Creation

    string[] SplitCSV() // thanks infallible code
    {
        return PlayerShipCSV.text.Split(
            new[] { Environment.NewLine },
            StringSplitOptions.None).
            Skip(1).ToArray();
    }

    #if UNITY_EDITOR
        private void CreateShip(string row)
    {
        var columns = row.Split(',');

        string asset_name = columns[0] + "_ship";

        if (Resources.Load<PlayerData>($"PlayerShips/{asset_name}") != null)
        {
            Debug.Log($"Scriptable Object [{asset_name}] already exist");
            return;
        }

        PlayerData asset = ScriptableObject.CreateInstance<PlayerData>();

        asset.name = asset_name;
        asset.DisplayName = columns[1];
        asset.MoveSpeed = float.Parse(columns[2]);
        asset.Health = int.Parse(columns[3]);
        asset.Rarity = int.Parse(columns[4]);

        var gameSprite = Resources.Load<Sprite>("Textures/PlayerShips/" + asset_name);

        asset.Icon = gameSprite;
        asset.GameSprite = gameSprite;

        AssetDatabase.CreateAsset(asset, $"Assets/Resources/PlayerShips/{asset_name}.asset");
        AssetDatabase.SaveAssets();

    }
    

    public void CreateAllShips()
    {
        var rows = SplitCSV();

        foreach (var row in rows)
        {
            CreateShip(row);
        }
    }
    #endif

    #endregion

    public float GetMaxSpeed()
    {
        float maxSpeed = 0;
        foreach (var item in PlayerShips)
        {
            if (item.MoveSpeed > maxSpeed)
            {
                maxSpeed = item.MoveSpeed;
            }
        }
        return maxSpeed;
    }

    public float GetMaxHealth()
    {
        float maxHealth = 0;
        foreach (var item in PlayerShips)
        {
            if (item.Health > maxHealth)
            {
                maxHealth = item.Health;
            }
        }
        return maxHealth;
    }

}
}