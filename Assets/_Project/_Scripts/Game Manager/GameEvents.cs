using System;
using UnityEngine;

namespace CF {

public class GameEvents : MonoBehaviour
{
    public event Action onEnemyAttackOver;
    public event Action<GameObject> onBulletOverEdge;
    public event Action<GameObject> onSpecialOverEdge;
    public event Action<GameObject> onSpaceTrashOverEdge;

    public event Action onSpecialAbility;
    public event Action onShieldAbility;

    public event Action onPlayerDeath;

    public event Action onEnemyDeath;

    public event Action onPlayerHit;

    public event Action<float, float> onScreenShake;

    public event Action<int> onEnvEvent;

    public static GameEvents Current { get { return _current; } }

    private static GameEvents _current;

    private void Awake()
    {
        if (_current != null && _current != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _current = this;
        }
    }

    public void BulletOverEdgeEnter(GameObject _obj)
    {
        onBulletOverEdge?.Invoke(_obj);
    }

    public void SpecialOverEdgeEnter(GameObject _obj)
    {
        onSpecialOverEdge?.Invoke(_obj);
    }

    public void SpaceTrashOverEdgeEnter(GameObject _obj)
    {
        onSpaceTrashOverEdge?.Invoke(_obj);
    }


    #region Player
    public void SpecialAbilityEnter()
    {
        onSpecialAbility?.Invoke();
    }

    public void ShieldAbilityEnter()
    {
        onShieldAbility?.Invoke();
    }

    public void PlayerDeathEnter()
    {
        onPlayerDeath?.Invoke();
    }


    public void PlayerHitEnter()
    {
        onPlayerHit?.Invoke();
    }

    #endregion

    #region Enemy

    public void EnemyDeathEnter()
    {
        onEnemyDeath?.Invoke();
    }

    public void EnemyAttackOverEnter()
    {
        onEnemyAttackOver?.Invoke();
    }

    #endregion

    public void ScreenShakeEnter(float shakeTime, float shakePower)
    {
        onScreenShake?.Invoke(shakeTime, shakePower);
    }

    public void EnvironmentEventEnter(int lane)
    {
        onEnvEvent?.Invoke(lane);
    }

}
}
