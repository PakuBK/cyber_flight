using UnityEngine;
using CF.Environment;
using CF.Controller;
using CF.Particles;

namespace CF.Player {
public class ShieldCollider : MonoBehaviour
{
    private ShieldController shield;

    private void Awake()
    {
        shield = GetComponentInParent<ShieldController>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // TODO could implement a reflect attack here
        if (collision.CompareTag("Bullet"))
        {
            var controller = collision.gameObject.GetComponent<BulletController>();
            if (controller.fromEnemy)
            {
                ParticleManager.current.SpawnVFX(ParticleOrigin.ShieldHit, transform.position + Vector3.up);
                shield.ShieldHit();
                controller.VanishBullet();
            }
        }
        else if (collision.CompareTag("Obstacle"))
        {
            var controller = collision.gameObject.GetComponent<SpaceTrashController>();
            if (controller.fromEnemy)
            {
                ParticleManager.current.SpawnVFX(ParticleOrigin.ShieldHit, transform.position + Vector3.up);
                shield.ShieldHit();
                controller.VanishBullet();
            }
        }
    }

    public void ShieldHitAnimationOver()
    {
        shield.HitAnimationOver();
    }

    public void ShieldEntryAnimatioOver()
    {
        shield.EntryAnimationOver();
    }

    public void ShieldExitAnimationOver()
    {
        shield.ExitAnimationOver();
    }
}
}