using UnityEngine;

namespace CF.Player {
public class SpecialAbility 
{
    private AttackController attackController;
    private Transform player;
    private GameObject specialPrefab;

    public SpecialData Data;
    public float startTime;

    public SpecialAbility(Transform _player, GameObject _specialPrefab, SpecialData _data)
    {
        player = _player;
        specialPrefab = _specialPrefab;
        Data = _data;

        specialPrefab.GetComponent<SpecialController>().specialData = _data;

        startTime = -Data.Cooldown;
    }

    public void Enter()
    {
        if (Time.time > startTime + Data.Cooldown)
        {
            SpawnSpecial(player.position);
            startTime = Time.time;
        }
        
    }

    private void SpawnSpecial(Vector3 _pos)
    {
        ObjectPooler.Current.InstantiateSpecificPrefab(specialPrefab, _pos);
    }

    public bool CanShoot() => Time.time > startTime + Data.ShootAfterSpecialCooldown;
}
}