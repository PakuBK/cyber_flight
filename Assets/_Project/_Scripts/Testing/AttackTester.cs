using CF.Data;
using UnityEngine;

namespace CF.Testing {
public class AttackTester : MonoBehaviour
{
    [SerializeField]
    private EnemyAttackData enemyAttackData;

    private int bulletsShot = 0;
    private float startTime;
    private bool attackFinished;
    private bool attackAlways;

    private void Update(){
        if (!attackFinished || attackAlways) Attack();
    }

    private void Attack(){
        if (bulletsShot >= enemyAttackData.AmountOfBullets) // Exit Attack
        {
            attackFinished = true;
        }

        if (Time.time >= startTime + enemyAttackData.TimeBetweenBullets) // Attack
        {
            startTime = Time.time;
            bulletsShot++;
            //ObjectPooler.Current.CreateEnemyBullet(enemyAttackData, transform.position);
            foreach (var bullet in enemyAttackData.Bullets)
            {
                bullet.FromEnemy = true;
                ObjectPooler.Current.InstantiateBullet(bullet, transform.position);
            }
        }
    }

    public void EnterAttack(){
        attackFinished = false;
        bulletsShot = 0;
    }

    public void ToggleAttack(){
        attackAlways = !attackAlways;
    }
}
}