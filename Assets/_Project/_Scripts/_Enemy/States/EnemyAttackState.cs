using UnityEngine;
using CF.Data;
using CF.Audio;

namespace CF.Enemy {
public class EnemyAttackState : EnemyState
{
    private EnemyData enemyData;

    private EnemyAttackData currentPreset;

    private int currentAmountOfRainBullets;
    private bool canAttack;

    

    public EnemyAttackState(EnemyBrain _brain, EnemyStateMachine _stateMachine) : base(_brain, _stateMachine)
    {
        enemyData = brain.Data;
    }

    #region State Logic

    public override void Enter()
    {
        base.Enter();
        currentPreset = SelectRandomPreset();

        if (currentPreset.OneColorForAll) {
            foreach (var bullet in currentPreset.Bullets)
            {
                bullet.GlowColor = currentPreset.AllBulletsColor;
            }
        }

        startTime = Time.time;
        currentAmountOfRainBullets = 0;
        enemyData = brain.Data;

        PlayAnim(brain.Data.EntryClip.name);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (lockState) return;

        if (canAttack)
        {
            Attack();
        }
    }

    public override void Exit() 
    {
        base.Exit();
        canAttack = false;
    }

    private void Attack()
    {
        brain.enemyMaterial.SetFloat("_GlowScale", 1f);
        if (currentAmountOfRainBullets >= currentPreset.AmountOfRainBullets) // Exit Attack
        {
            canAttack = false;
            brain.enemyMaterial.SetFloat("_GlowScale", 0f);
            PlayAnim(brain.Data.ExitClip.name);
            return;
        }

        if (Time.time >= startTime + currentPreset.BulletRainTimer) // Attack
        {
            startTime = Time.time;
            currentAmountOfRainBullets++;
            //ObjectPooler.Current.CreateEnemyBullet(currentPreset, brain.transform.position);
            /*
            foreach (BulletData bullet in currentPreset.Bullets)
            {
                bullet.FromEnemy = true;
                ObjectPooler.Current.InstantiateBullet(bullet, brain.transform.position);
            }
            */
            
            ObjectPooler.Current.InstantiateBulletsDelayed(currentPreset.Bullets, brain.transform, true);
            PlayAttackSFX();
        }
    }
    #endregion

    private EnemyAttackData SelectRandomPreset()
    {
        var attacks = brain.Data.Attacks;

        float sum_weight = 0;

        var weight_attacks = new float[attacks.Length];

        for (int i = 0; i < attacks.Length; i++)
        {
            var adjusted_weight = attacks[i].Weight;
            foreach(var offset in attacks[i].AttackTargetOffset){
                if (brain.PlayerController.CurrentLane == brain.currentLane + offset) {
                    adjusted_weight *= 3;
                    break;
                }
            }
            
            sum_weight += adjusted_weight;
            

            weight_attacks[i] = sum_weight;
        }

        float randomFloat = Random.Range(0f, sum_weight);

        for (int i = 0; i < attacks.Length; i++)
        {
            if (weight_attacks[i] >= randomFloat)
            {
                return attacks[i];
            }
        }

        Debug.LogError("Error in Random Bullet Selection");

        return null;

    }



    #region Animation
    public void EntryAnimTrigger()
    {
        canAttack = true;
        PlayAnim(brain.Data.AttackClip.name);
    }

    public void ExitAnimTrigger() => TransitionToIdle();

    public void TransitionToIdle(){
        Debug.Log("trans to idle");
        stateMachine.ChangeState(brain.IdleState);
    }

    #endregion

    #region Sound
    private void PlayAttackSFX()
    {
        if (currentPreset.AttackSFX.Length == 0)
        {
            AudioController.Current.PlayAudioOnTop(AudioOfType.SFX_Enemy_Weapon_01);
        }
        else {
            var clip = currentPreset.AttackSFX[Random.Range(0, currentPreset.AttackSFX.Length)];
            AudioController.Current.PlayAudioOnTop(clip);
        }
        
    }

    #endregion
}
}