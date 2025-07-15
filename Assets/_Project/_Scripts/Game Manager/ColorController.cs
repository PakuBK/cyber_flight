using UnityEngine;

namespace CF.Controller {
public class ColorController : MonoBehaviour
{
    public Color HitColor;

    public Color normalColor;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetHitColor()
    {
        spriteRenderer.color = HitColor;
    }

    public void ResetColor()
    {
        spriteRenderer.color = normalColor;
    }
}
}