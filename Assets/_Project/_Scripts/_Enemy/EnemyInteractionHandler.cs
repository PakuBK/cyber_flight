using UnityEngine;
using CF.Player;
using CF.Controller;
using CF.Particles;

namespace CF.Enemy {
    public class EnemyInteractionHandler : MonoBehaviour
    {
        private EnemyStatusEffectHandler statusEffectHandler;
        private EnemyHealthController healthController;


        private void Awake()
        {
            statusEffectHandler = GetComponent<EnemyStatusEffectHandler>();
            healthController = GetComponent<EnemyHealthController>();
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            HandleCollision(collision);
        }

        /// <summary>
        /// Routes collision handling to appropriate methods based on the colliding object's tag.
        /// </summary>
        /// <param name="collision">The collider to process</param>
        /// <remarks>
        /// This method acts as a dispatcher, routing different collision types to their 
        /// respective handlers. Currently supports "Bullet" and "Special" tagged objects.
        /// </remarks>
        private void HandleCollision(Collider2D collision)
        {
            if (collision.CompareTag("Bullet"))
            {
                HandleBulletCollision(collision);
            }
            else if (collision.CompareTag("Special"))
            {
                HandleSpecialCollision(collision);
            }
        }

        /// <summary>
        /// Handles collision with player bullets, applying damage and visual effects.
        /// </summary>
        /// <param name="collision">The bullet collider</param>
        /// <remarks>
        /// Only processes bullets that are not from enemies (player bullets).
        /// Applies damage, spawns hit particles, and destroys the bullet.
        /// Enemy bullets are ignored to prevent friendly fire.
        /// </remarks>
        private void HandleBulletCollision(Collider2D collision)
        {
            BulletController controller = collision.GetComponent<BulletController>();
            if (!controller.fromEnemy)
            {
                healthController.TakeDamage(controller.Damage);
                ParticleManager.current.SpawnVFX(ParticleOrigin.EnemyHit, transform.position);
                controller.VanishBullet();
            }
        }

        /// <summary>
        /// Handles collision with special attacks, applying damage, status effects, and enhanced feedback.
        /// </summary>
        /// <param name="collision">The special attack collider</param>
        /// <remarks>
        /// Special attacks always deal damage and may apply additional status effects based on their type:
        /// - Stun: Temporarily disables enemy movement and actions
        /// - Slow: Reduces enemy movement speed
        /// Also triggers screen shake and enhanced particle effects for visual impact.
        /// </remarks>
        private void HandleSpecialCollision(Collider2D collision)
        {
            SpecialController controller = collision.GetComponent<SpecialController>();
            healthController.TakeDamage(controller.Damage);

            SpecialType specialType = controller.specialData.Type;

            switch (specialType)
            {
                case SpecialType.Stun:
                    statusEffectHandler.ApplyEffect(StatusEffect.Stunned, controller.specialData.SpecialDuration);
                    break;
                case SpecialType.Slow:
                    statusEffectHandler.ApplyEffect(StatusEffect.Slowed, controller.specialData.SpecialDuration);
                    break;
                case SpecialType.None:
                default:
                    // No additional effects for basic special attacks
                    break;
            }

            GameEvents.Current.ScreenShakeEnter(0.5f, 0.5f);
            ParticleManager.current.SpawnVFX(ParticleOrigin.EnemyHit, transform.position);
        }
    }
}
