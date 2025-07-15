using UnityEngine;
using CF.Data;
using CF.Particles;

namespace CF.Controller {
public class BulletController : MonoBehaviour
{
    private Vector2 movementVector;
    private float speed;
    private float size;
    private Sprite sprite;
    public float Damage { get; private set; }

    public bool fromEnemy;
    private Color glowColor;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private ParticleSystem currentTrail;

    public void UpdateSettings(BulletData data)
    {
        speed = data.Speed;
        size = data.Size;
        sprite = data.Sprite;
        movementVector = data.MovementVector;
        fromEnemy = data.FromEnemy;
        Damage = data.Damage;
        glowColor = data.GlowColor;

        GetComponent<Renderer>().material.SetColor("_Color", data.GlowColor);

        if (currentTrail != null) {
            currentTrail.textureSheetAnimation.SetSprite(0, sprite);
            currentTrail.gameObject.GetComponent<Renderer>().material.SetColor("_Color", glowColor);
            currentTrail.transform.localScale = Vector3.one * (size-0.25f);
            currentTrail.Play();
        }
        
            

        SetVelocity();
        ScaleObject();
        SetSprite();
    }



    #region Unity Functions 
    private void Awake()
    {
        InitializeBullet();
    }


    private void OnEnable()
    {
        SetVelocity();
        SetSprite();

        currentTrail.Play();

        if (rb.velocity.y > 0)
        {
            var speedMultiplier = currentTrail.main.startSpeedMultiplier;
            speedMultiplier = -1f;
        }
        else
        {
            var speedMultiplier = currentTrail.main.startSpeedMultiplier;
            speedMultiplier = 1f;
        }
    }

    private void OnDisable()
    {
        currentTrail.Stop();
    }

    private void Update()
    {
        CheckBullet();
    }

    #endregion

    public void VanishBullet()
    {
        GameEvents.Current.BulletOverEdgeEnter(gameObject);
    }

    #region Private Functions
    private void CheckBullet()
    {
        if (transform.position.y < -ScreenSize.GetScreenToWorldHeight)
        {
            VanishBullet();
        }
        else if (transform.position.y > ScreenSize.GetScreenToWorldHeight)
        {
            VanishBullet();
        }

        spriteRenderer.flipY = (rb.velocity.y > 0);

    }

    private void InitializeBullet()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        currentTrail = ParticleManager.current.SpawnBulletTrail(ParticleOrigin.BulletTrail, transform.position, transform);
        

        ScaleObject();
        SetSprite();

    }

    private void SetVelocity()
    {
        //rb.velocity = movementVector * speed * (ScreenSize.GetScreenToWorldHeight * 0.1f);
        rb.velocity = movementVector * speed;

        if (rb.velocity.y > 0)
        {
            var main = currentTrail.main;
            main.startSpeed = Mathf.Abs(main.startSpeed.constant) * -1f;
            main.startRotation = Mathf.Deg2Rad * 180f;
        }
        else
        {
            var main = currentTrail.main;
            main.startSpeed = Mathf.Abs(main.startSpeed.constant);
            main.startRotation = 0;
        }
    }

    private void SetSprite()
    {
        spriteRenderer.sprite = sprite;
        currentTrail.textureSheetAnimation.SetSprite(0, sprite);
    }

    private void ScaleObject()
    {
        transform.localScale = Vector3.one * size;
    }

    #endregion

}
}