using UnityEngine;

namespace CF.Data {

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    public string DisplayName;
    public Sprite Icon;

    [Header("Move State")]
    public float MoveSpeed;

    [Header("Player Stats")]
    public int Health;

    [Header("Game Display Sprite")]
    public Sprite GameSprite;

    public int Rarity;
}
}