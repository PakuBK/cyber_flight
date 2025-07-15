using UnityEngine;
using CF.UI;
using CF.Data;

namespace CF.Player {
public class ShieldController : MonoBehaviour
{
    [Header("Shield Data")]
    public ShieldData data;
    public GameObject shieldVisual;

    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private InteractionHandler interaction;

    private float hitPoints;

    private float invincibilityTime = 0.01f;

    [SerializeField]
    private float recoveryTime = 0.2f;

    private float startTime, endTime, hitTime;

    private bool shieldActive, onCooldown, wasHit, onRecovery, shieldBroke;

    private void Awake()
    {
        data = DataController.LoadPlayerShield();
        
        spriteRenderer = shieldVisual.GetComponent<SpriteRenderer>();
        anim = shieldVisual.GetComponent<Animator>();
        interaction = gameObject.GetComponentInParent<InteractionHandler>();
        invincibilityTime = data.InvincibilityTime;

    }

    private void Start()
    {
        GameEvents.Current.onShieldAbility += ShieldOn;
    }

    private void Update()
    {
        if (shieldActive)
        {
            if (Time.time > startTime + data.Duration)
            {
                ShieldOff();
            }

            if (wasHit)
            {
                if (Time.time > hitTime + invincibilityTime)
                {
                    wasHit = false;
                }
            }
            UIController.current.UpdatePlayerShield(Time.time - startTime, data.Duration, true);
        }
        else if (onCooldown)
        {
            if (Time.time > endTime + data.Cooldown)
            {
                onCooldown = false;
            }
            UIController.current.UpdatePlayerShield(Time.time - endTime, data.Cooldown, false);
        }

        if (onRecovery)
        {
            if (Time.time > endTime + recoveryTime)
            {
                onRecovery = false;
                interaction.ToggleCanBeHit(true);

            }
        }
        
    }

    

    private void ShieldOn()
    {
        if (shieldActive || onCooldown) return;

        interaction.ToggleCanBeHit(false);

        startTime = Time.time;

        hitPoints = data.Hitpoints;

        shieldVisual.SetActive(true);
        shieldActive = true;
        anim.Play(data.shieldEntry.name);
    }

    private void ShieldOff()
    {
        anim.Play(data.shieldExit.name);
    }

    public void ShieldHit()
    {
        if (wasHit || shieldBroke) return;

        wasHit = true;
        hitTime = Time.time;
        anim.Play(data.shieldHit.name);

        hitPoints--;

        if (hitPoints <= 0)
        {   
            shieldBroke = true;
            ShieldOff();
        }
        
    }

    public void HitAnimationOver()
    {
        anim.Play(data.shieldOn.name);
    }

    public void EntryAnimationOver()
    {
        anim.Play(data.shieldOn.name);
    }

    public void ExitAnimationOver()
    {
        shieldVisual.SetActive(false);
        shieldActive = false;
        onCooldown = true;
        onRecovery = true;
        shieldBroke = false;
        endTime = Time.time;
    }
    
}
}