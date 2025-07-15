using UnityEngine;
using CF.Particles;

namespace CF.Player {
public class SpecialController : MonoBehaviour
{
    public SpecialData currentData;

    private Rigidbody2D rb;
    private Transform enemy;
    private SpriteRenderer spriteRenderer;

    private ParticleSystem system;

    public float Damage;
    public float Cooldown;
    public SpecialType Type;
    public float SpecialDuration;

    private bool Homing;
    private Vector2 Direction;
    private float Speed;
    private float Size;
    private Sprite Apperance;
    private Sprite ParticleSprite;
    private Color customColor;

    public void LoadData(SpecialData _data)
    {
        Damage = _data.Damage;
        Cooldown = _data.Cooldown;
        Type = _data.Type;
        SpecialDuration = _data.SpecialDuration;

        Homing = _data.Homing;
        Direction = _data.Direction;
        Speed = _data.Speed;
        Size = _data.Size;
        Apperance = _data.Apperance;
        ParticleSprite = _data.ParticleSprite;
        customColor = _data.CustomColor;
    }

    public void InitializeSpecial(SpecialData _data)
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (system == null)
        {
            system = GetComponent<ParticleSystem>();
        }

        LoadData(_data);

        ScaleObject();
        SetupParticle();
        spriteRenderer.color = customColor;
    }

    private void OnEnable()
    {
        InitializeSpecial(currentData);

        spriteRenderer.sprite = Apperance;

        if (!Homing)
        {
            rb.velocity = Direction * Speed;
        }
        else
        {
            enemy = SetupScene.Current.GetCurrentEnemy();
        }
    }

    private void OnDisable()
    {
        system.Stop();
    }

    private void Update()
    {
        CheckSpecial();
    }

    private void FixedUpdate()
    {
        if (Homing)
        {
            FollowEnemy();
            RotateObject();
        }
    }

    private void FollowEnemy()
    {
        if (Vector3.Distance(transform.position, enemy.position) > 0.01f)
        {
            float step = Speed * Time.fixedDeltaTime;
            transform.position = Vector3.MoveTowards(transform.position, enemy.position, step);
        }
    }

    private void ScaleObject()
    {
        transform.localScale = Vector3.one * Size;
    }

    private void SetupParticle()
    {
        if (system == null)
        {
            system = ParticleManager.current.SpawnBulletTrail(ParticleOrigin.SpecialTrail, transform.position, transform);
        }
        
        system.textureSheetAnimation.SetSprite(0, ParticleSprite);
        var main = system.main;
        main.startColor = customColor;

        system.Play();
    }

    private void RotateObject() 
    {
        Vector2 direction = (Vector2)enemy.position - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle - 90));
    }

    private void CheckSpecial()
    {
        if (transform.position.y < -ScreenSize.GetScreenToWorldHeight)
        {
            GameEvents.Current.SpecialOverEdgeEnter(gameObject);
        }
        else if (transform.position.y > ScreenSize.GetScreenToWorldHeight)
        {
            GameEvents.Current.SpecialOverEdgeEnter(gameObject);
        }

        spriteRenderer.flipY = rb.velocity.y < 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            GameEvents.Current.SpecialOverEdgeEnter(gameObject);
        }
    }  
}
}