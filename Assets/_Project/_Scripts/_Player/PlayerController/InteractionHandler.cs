using UnityEngine;
using CF.Environment;
using CF.Controller;
using CF.UI;
using CF.Data;
using CF.Particles;

namespace CF.Player {
public class InteractionHandler : MonoBehaviour
{
    [HideInInspector]
    public PlayerData data;

    [Header("DEBUGGING")][Tooltip("Load a ship from start [DEBUG]")]
    [SerializeField]
    private bool debugShip;
    [SerializeField]
    private PlayerData debugShipData;
    

    public int Health { get; private set; }

    private bool isAlive = true;

    private bool wasHit;

    private bool canBeHit = true;

    private float invincibilityTime = 0.2f;

    private float startHitTime;
        
    private AttackController attackController;
    private LaneController laneController;
    private ColorController colorController;

    private SpriteRenderer spriteRenderer;

    public bool ResetCurrency;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        data = DataController.LoadPlayerShip();

        if (debugShip) data = debugShipData;

        LoadData();
        attackController = GetComponent<AttackController>();
        laneController = GetComponent<LaneController>();
        colorController = GetComponent<ColorController>();

        laneController.LoadPlayerData(data);

        if (ResetCurrency)
        {
            DataController.SaveCurrency(0);
        }
    }

    private void Update()
    {
        if (wasHit)
        {
            if (Time.time > startHitTime + invincibilityTime)
            {
                wasHit = false;
                colorController.ResetColor();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") && !wasHit && canBeHit)
        {
            var controller = collision.GetComponent<BulletController>();
            if (controller.fromEnemy)
            {
                GameEvents.Current.PlayerHitEnter();
                GameEvents.Current.ScreenShakeEnter(0.5f, 0.5f);

                ParticleManager.current.SpawnVFX(ParticleOrigin.PlayerHit, transform.position);

                ProcessHit();
            }
        }
        else if (collision.CompareTag("Obstacle") && !wasHit && canBeHit)
        {
            var controller = collision.GetComponent<SpaceTrashController>();
            if (controller.fromEnemy)
            {
                GameEvents.Current.PlayerHitEnter();
                GameEvents.Current.ScreenShakeEnter(0.5f, 0.5f);

                ProcessHit();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
    }

    private void ProcessHit()
    {
        Health--;
        startHitTime = Time.time;
        wasHit = true;
        colorController.SetHitColor();

        UIController.current.UpdatePlayerHealth(Health, data.Health);

        if (Health <= 0 && isAlive)
        {
            PlayerDeath();
        }
    }

    private void PlayerDeath()
    {
        attackController.locked = true;
        laneController.locked = true;
        isAlive = false;

        GameEvents.Current.PlayerDeathEnter();
    }

    private void LoadData()
    {
        Health = data.Health;

        spriteRenderer.sprite = data.GameSprite;
    }

    public void ToggleCanBeHit(bool state)
    {
        canBeHit = state;
    }
    
}
}