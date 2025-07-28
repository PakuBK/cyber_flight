using UnityEngine;
using CF.Data;
using CF.Audio;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
            Debug.Log("EnemyAttackState: Entering Attack State");
            base.Enter();

            // Select attack based on the enemy's attack selection mode 
            if (context.enemyData.AttackSelectionMode == AttackSelectionMode.WeighedRandom)
            {
                attackData = SelectWeightedRandomAttack();
            }
            else if (context.enemyData.AttackSelectionMode == AttackSelectionMode.RandomAmongValid)
            {
                attackData = SelectRandomAmongValidAttack();
            }
            else if (context.enemyData.AttackSelectionMode == AttackSelectionMode.WeightedRandomAmongValid)
            {
                attackData = SelectWeightedRandomAmongValidAttack();
            }
            else if (context.enemyData.AttackSelectionMode == AttackSelectionMode.FirstValidByWeight)
            {
                attackData = SelectFirstValidAttackByWeight();
            }
            else
            {
                Debug.LogError("Invalid attack selection mode!");
                stateMachine.EnterState(EnemyStateType.Idle);
                return;
            }

            if (attackData == null)
            {
                Debug.LogError("No valid attack data found for enemy!");
                stateMachine.EnterState(EnemyStateType.Idle);
                return;
            }

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

            // Check for stun, transition to interrupt state if stunned
            if (context.HasActiveEffect(StatusEffect.Stunned))
            {
                base.Exit();
                stateMachine.EnterState(EnemyStateType.Interrupt);
                return;
            }

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

        /// <summary>
        /// If the attack selection mode is WeightedRandomAmongValid, this method will select a random attack that can hit the player based on weights.
        /// </summary>
        /// <returns>Randomly selected attack based weights from valid attacks</returns>
        private EnemyAttackData SelectWeightedRandomAmongValidAttack()
        {
            List<EnemyAttackData> validAttacks = context.enemyData.Attacks
                .Where(a => WillAttackHit(a))
                .ToList();
            if (validAttacks.Count == 0)
            {
                return HelperSelectRandom(context.enemyData.Attacks); // Fallback to a random attack if no valid attacks found
            }
            // Select a random attack from the valid attacks
            return HelperSelectRandomByWeight(validAttacks.ToArray());
        }

        /// <summary>
        /// If the attack selection mode is RandomAmongValid, this method will select a random attack that can hit the player.
        /// </summary>
        /// <returns>Randomly selected attack that can hit the player</returns>
        private EnemyAttackData SelectRandomAmongValidAttack()
        {
            List<EnemyAttackData> validAttacks = context.enemyData.Attacks
                .Where(a => WillAttackHit(a))
                .ToList();
            if (validAttacks.Count == 0)
            {
                return HelperSelectRandom(context.enemyData.Attacks); // Fallback to a random attack if no valid attacks found
            }
            return validAttacks[Random.Range(0, validAttacks.Count)];
        }


        /// <summary>
        /// If the attack selection mode is WeighedRandom, this method will select a random attack that can hit the player.
        /// </summary>
        /// <returns>Randomly selected attack based on weights</returns>
        private EnemyAttackData SelectWeightedRandomAttack()
        {
            EnemyAttackData[] attacks = context.enemyData.Attacks;
            return HelperSelectRandomByWeight(attacks);
        }

        /// <summary>
        /// If the attack selection mode is FirstValidByWeight, this method will select the first valid attack that can hit the player ordered by weight.
        /// </summary>
        /// <returns>First valid attack sorted by weight</returns>
        private EnemyAttackData SelectFirstValidAttackByWeight()
        {
            List<EnemyAttackData> attacks = context.enemyData.Attacks.ToList();
            attacks.OrderByDescending(a => a.Weight);
            foreach (EnemyAttackData attack in attacks)
            {
                if (attack.TargetType == AttackTargetType.RandomLane) return attack; // Random lane attacks can always be selected
                if (WillAttackHit(attack))
                {
                    return attack; // Return the first attack that can hit the player
                }
            }
            // fallback to first attack if none match
            return attacks.FirstOrDefault();
        }

        /// <summary>
        /// Helper method to select a random attack based on weights.
        /// </summary>
        /// <param name="attacks">Enemy Attacks passed by caller</param>
        /// <returns>Randomly selected attack by weight</returns>
        private EnemyAttackData HelperSelectRandomByWeight(EnemyAttackData[] attacks) {
            if (attacks == null || attacks.Length == 0)
            {
                Debug.LogError("No attacks available for selection");
                return null;
            }
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

        private EnemyAttackData HelperSelectRandom(EnemyAttackData[] attacks)
        {
            if (attacks == null || attacks.Length == 0)
            {
                Debug.LogError("No attacks available for selection");
                return null;
            }
            int randomIndex = Random.Range(0, attacks.Length);
            return attacks[randomIndex];
        }

        private bool WillAttackHit(EnemyAttackData data)
        {
            int[] attackOffsets = data.TargetOffset;
            int current_lane = ((int)context.currentLane);
            int player_lane = context.GetPlayerLaneIndex();

            int[] validLanes = new int[attackOffsets.Length];
            for (int i = 0; i < validLanes.Length; i++) { validLanes[i] = -1; }
            for (int i = 0; i < attackOffsets.Length; i++)
            {
                validLanes[i] = current_lane + attackOffsets[i];
            }
            // Check if the player's lane is in the valid lanes
            foreach (int lane in validLanes)
            {
                if (lane == player_lane)
                {
                    return true; // Attack will hit the player
                }
            }
            return false; // Attack will not hit the player
        }

    }
}