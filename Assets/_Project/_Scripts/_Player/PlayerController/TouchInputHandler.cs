using UnityEngine;
using UnityEngine.InputSystem;

namespace CF.Player {

public class TouchInputHandler : MonoBehaviour
{
    public delegate void InputEventHandler(Vector2 _input);
    public event InputEventHandler OnEndTouch;

    private TouchControlls touchControlls;

    private AttackController attackController;

    private Vector2 startPos;
    private Vector2 endPos;

    public float SwipeInputTime = 0.2f;
    private float startInputTime;
    private float minDistance = 100f;

    private bool touchStarted;

    private void Awake()
    {
        touchControlls = new TouchControlls();
        touchControlls.Enable();
        attackController = GetComponent<AttackController>();
    }

    private void OnEnable()
    {
        touchControlls.Enable();
    }

    private void OnDisable()
    {
        touchControlls.Disable();
    }

    private void Start()
    {
        touchControlls.Touch.TouchPress.started += ctx => StartTouch(ctx);
        touchControlls.Touch.TouchPress.canceled += ctx => EndTouch(ctx);
    }

    private void Update()
    {
        
        if (touchStarted && Vector2.Distance(startPos, touchControlls.Touch.TouchPos.ReadValue<Vector2>()) > minDistance)
        {
            if (Time.time >= startInputTime + SwipeInputTime )
            {
                EndTouch();
            }
        }
        
    }

    private void StartTouch(InputAction.CallbackContext ctx)
    {
        startPos = touchControlls.Touch.TouchPos.ReadValue<Vector2>();
        startInputTime = Time.time;
        touchStarted = true;
        attackController.ToggleNormalAbility(true);
    }

    private void EndTouch()
    {
        endPos = touchControlls.Touch.TouchPos.ReadValue<Vector2>();
        Vector2 directionVector = (endPos - startPos).normalized;
        Vector2 normalizedVector = NormVector(directionVector);

        if (normalizedVector.y > 0)
        {
            GameEvents.Current.SpecialAbilityEnter();
        }
        else if (normalizedVector.y < 0)
        {
            GameEvents.Current.ShieldAbilityEnter();
        }
        else
        {
            OnEndTouch?.Invoke(normalizedVector);
        }

        touchStarted = false;
    }

    private void EndTouch(InputAction.CallbackContext ctx)
    {
        attackController.ToggleNormalAbility(false);
    }

    private Vector2 NormVector(Vector2 _rawInput) 
    {
        float _normInputX;
        float _normInputY;

        if (Mathf.Abs(_rawInput.x) > 0.5f)
        {
            _normInputX = (int)(_rawInput * Vector2.right).normalized.x;
        }
        else
        {
            _normInputX = 0;
        }
        if (Mathf.Abs(_rawInput.y) > 0.5f)
        {
            _normInputY = (int)(_rawInput * Vector2.up).normalized.y;
        }
        else
        {
            _normInputY = 0;
        }

        return new Vector2(_normInputX, _normInputY);
    }
}
}
