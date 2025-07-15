using UnityEngine;

namespace CF.Environment {
public class SpaceTrashController : MonoBehaviour
{
    [SerializeField]
    private Vector2 movementVector;

    public bool fromEnemy = true;

    [HideInInspector]
    public float speed;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    #region Unity Functions 
    private void Awake()
    {
        InitializeBullet();
    }

    private void OnEnable()
    {
        SetVelocity();
    }

    private void Update()
    {
        CheckBullet();
        RotateSelf();
    }

    #endregion

    public void VanishBullet()
    {
        GameEvents.Current.SpaceTrashOverEdgeEnter(gameObject);
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
    }

    private void SetVelocity()
    {
        rb.velocity = movementVector * speed;
    }


    public void UpdateVelocity()
    {
        SetVelocity();
    }

    private void RotateSelf()
    {
        transform.Rotate(Vector3.forward, Random.Range(10f, 45f) * Time.fixedDeltaTime);
    }

    #endregion
}
}