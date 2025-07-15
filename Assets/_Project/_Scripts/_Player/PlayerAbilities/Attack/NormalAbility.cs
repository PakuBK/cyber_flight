using UnityEngine;
using CF.Audio;
using CF.Data;

namespace CF.Player {
public class NormalAbility
{
    private Transform player;
    private AttackData data;

    private float startTime;

    private int bulletsShot;
    private bool isInIntervall;
    private float intervallStartTime;

    private int bulletsCreated;

    
    public NormalAbility(Transform _player, AttackData _passiveAttackData)
    {
        player = _player;
        data = _passiveAttackData;
        startTime = Time.time;
    }
    public void AbilityUpdate()
    {
        if (data.Intervall)
        {
            if (data.IntervallThreshold <= bulletsShot)
            {
                isInIntervall = true;
                intervallStartTime = Time.time;
            }
            if (isInIntervall && Time.time >= intervallStartTime + data.IntervallTime)
            {
                isInIntervall = false;
                bulletsShot = 0;
            }
        }

        if (isInIntervall) return;

        if (Time.time > startTime + data.BulletTimer)
        {
            bulletsCreated = -data.BulletsPerAttack/2;
            for (int i = 0; i < data.BulletsPerAttack; i++)
            {
                CreateBullet();
                PlayAttackSound();
                bulletsCreated++;
            }
            startTime = Time.time;
        }
    }

    private void CreateBullet()
    {
        float speed = data.BulletSpeed;
        float size = data.BulletSize;
        float damage = data.Damage;
        Vector3 _pos = player.position;
        Vector2 movementVector = data.Direction;
        Sprite sprite = data.BulletSprite;
        Color glowColor = data.GlowColor;

        if (data.RandomSize && !data.RandomOffset && !data.RandomSpeed && !data.RandomDirection)
        {
            var _bulletData = new BulletData(speed, size, damage, sprite, movementVector, false, glowColor);
            SpawnBullet(_bulletData, _pos);
            return;
        }

        if (data.RandomOffset)
        {
            float _laneWidth = Mathf.Abs(SetupScene.Current.laneAnchorPoints[1] - SetupScene.Current.laneAnchorPoints[0]);
            float _offset = Random.Range(-_laneWidth * data.OffsetMultiplier, _laneWidth * data.OffsetMultiplier);

            _pos += new Vector3(_offset, 0, 0);
        }
        else if (data.FixedOffset)
        {
            float _laneWidth = Mathf.Abs(SetupScene.Current.laneAnchorPoints[1] - SetupScene.Current.laneAnchorPoints[0]);
            float _offset = _laneWidth * data.OffsetMultiplier * bulletsCreated;
            _pos += new Vector3(_offset, 0, 0);
        }

        if (data.RandomSpeed)
        {
            speed = Random.Range(data.MinBulletSpeed, data.MaxBulletSpeed);
        }

        if (data.RandomSize)
        {
            size = Random.Range(data.MinBulletSize, data.MaxBulletSpeed);
        }

        if (data.RandomDirection)
        {
            movementVector = new Vector2(Random.Range(data.MinDirectionX, data.MaxDirectionX), data.Direction.y);
        }


        var bulletData = new BulletData(speed, size, damage,sprite, movementVector, false, glowColor);
        SpawnBullet(bulletData, _pos);
    }

    private void SpawnBullet(BulletData _data, Vector3 _pos)
    {
        ObjectPooler.Current.InstantiateBullet(_data, _pos);
    }

    private void PlayAttackSound()
    {
        var clip = data.Attack_SFX[Random.Range(0, data.Attack_SFX.Length)];
        AudioController.Current.PlayAudio(clip);
    }

}
}