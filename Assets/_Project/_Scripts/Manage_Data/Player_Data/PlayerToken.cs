namespace CF.Data {
[System.Serializable]
public class PlayerToken
{
    public string ResourcesShipName;
    public int Shards;
    public int BossFragments;

    public PlayerToken(PlayerData _newData, int _shards, int _bossfragments)
    {
        ResourcesShipName = _newData.name;
        Shards = _shards;
        BossFragments = _bossfragments;
    }
}
}
