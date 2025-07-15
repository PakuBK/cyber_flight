using UnityEngine;

namespace CF.Controller {
public class CameraController : MonoBehaviour
{
    public Transform Target;

    private float startY;

    private float shakeTimeRemaining, shakePower, shakeFadeTime, shakeRotation;

    public float rotationMultiplier;

    private void Awake()
    {
        startY = transform.position.y;
    }

    private void Start()
    {
        GameEvents.Current.onScreenShake += EnterScreenShake;
    }

    public void EnterScreenShake(float length, float power)
    {
        shakeTimeRemaining = length;
        shakePower = power;

        shakeFadeTime = power / length;

        shakeRotation = power * rotationMultiplier;
    }

    private void LateUpdate()
    {
        if (shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;

            float xAmount = Random.Range(-1f, 1f) * shakePower;
            float yAmount = Random.Range(-1f, 1f) * shakePower;

            transform.position += new Vector3(xAmount, yAmount, 0);

            shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);

            shakeRotation = Mathf.MoveTowards(shakeRotation, 0f, shakeFadeTime * rotationMultiplier * Time.deltaTime);
        }

        transform.rotation = Quaternion.Euler(0, 0, shakeRotation * Random.Range(-1,1f));
    }

    private void Update()
    {
        if (Target != null)
        {
            transform.position = new Vector3(0, 0, -10);
        }
    }
}
}