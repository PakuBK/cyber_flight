using UnityEngine;
using CF.Player;
using CF.Controller;
using CF.Particles;

namespace CF.Enemy {
public class EnemyInteractionHandler : MonoBehaviour
{
    private EnemyContext context;

    private void Awake()
    {
        context = GetComponent<EnemyContext>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            var controller = collision.GetComponent<BulletController>();
            if (!controller.fromEnemy)
            {
                // brain.ProcessHit(controller.Damage);
                ParticleManager.current.SpawnVFX(ParticleOrigin.EnemyHit, transform.position);

                collision.GetComponent<BulletController>().VanishBullet();
            }
        }
        else if (collision.CompareTag("Special"))
        {
            var controller = collision.GetComponent<SpecialController>();
            // brain.ProcessSpecial(controller.Type, controller.Damage, controller.SpecialDuration);

            GameEvents.Current.ScreenShakeEnter(0.5f, 0.5f);
            ParticleManager.current.SpawnVFX(ParticleOrigin.EnemyHit, transform.position);
        }
    }
}
}
