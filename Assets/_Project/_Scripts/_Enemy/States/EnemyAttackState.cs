using UnityEngine;
using CF.Data;
using CF.Audio;
using System.Collections;

namespace CF.Enemy {
    public class EnemyAttackState : EnemyState
    {
        private EnemyAttackData attackData;
        private float telegraphStartTime;
        private bool telegraphDone;
        private bool attackDone;
        private bool attackStarted;

        public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine) {}

        public override void Enter()
        {
            base.Enter();
            attackData = selectWeightedAttack(); ; // Get from enemy's attack set
            telegraphStartTime = Time.time;
            telegraphDone = false;
            attackDone = false;
            attackStarted = false;


            // Play telegraph animation/SFX
            // If using Animancer, register OnTelegraphFinished callback
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // Telegraph phase -> later with Animacer or animation events
            if (!telegraphDone)
            {
                if (Time.time >= telegraphStartTime + attackData.TelegraphDuration) // Or use animation event
                {
                    telegraphDone = true;
                }
                return; // Wait for telegraph to finish
            }

            // Attack phase, only starts once telegraph is done
            if (!attackDone && !attackStarted)
            {
                attackStarted = true;
                if (attackData.AttackType == EnemyAttackType.Laser)
                {
                    Debug.Log("Placeholder: Laser Attack Started");
                    context.StartCoroutine(LaserAttackCoroutine(attackData.LaserDuration));
                }
                else if (attackData.AttackType == EnemyAttackType.Bullet)
                {
                    ObjectPooler.Current.InstantiateBulletsDelayed(attackData.Bullets,
                        context.transform,
                        () => { attackDone = true; },
                        true
                    );
                }
            }

            // Exit when attack is done
            if (attackDone)
            {
                Exit();
                stateMachine.EnterState(EnemyStateType.Idle);
            }
        }

        public override void Exit() 
        {
            base.Exit();
        }

        private IEnumerator LaserAttackCoroutine(float duration)
        {
            yield return new WaitForSeconds(duration);
            attackDone = true;
            Debug.Log("Placeholder: Laser Attack Finished");
        }

        private EnemyAttackData selectWeightedAttack()
        {
            EnemyAttackData[] attacks = context.enemyData.Attacks;
            float totalWeight = 0f;

            foreach (var attack in attacks)
            {
                totalWeight += attack.Weight;
            }

            float randomValue = Random.Range(0f, totalWeight);

            foreach (var attack in attacks)
            {
                if (randomValue < attack.Weight)
                {
                    return attack;
                }
                randomValue -= attack.Weight;
            }

            Debug.LogError("Error in weighted attack selection");
            return null;
        }

    }
}