using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 RawMovementInput { get; private set; }

    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool GrabInput { get; private set; }

    [SerializeField]
    private float m_InputHoldTime = 0.2f;

    private float m_JumpInputStartTime;

    #region Public Functions

    public void OnMoveInput(InputAction.CallbackContext ctx) 
    {
        RawMovementInput = ctx.ReadValue<Vector2>();

        if (Mathf.Abs(RawMovementInput.x) > 0.5f)
        {
            NormInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
        }
        else
        {
            NormInputX = 0;
        }
        if (Mathf.Abs(RawMovementInput.y) > 0.5f)
        {
            NormInputY = (int)(RawMovementInput * Vector2.up).normalized.y;
        }
        else
        {
            NormInputY = 0;
        }


    }

    public void OnJumpInput(InputAction.CallbackContext ctx) 
    {
        if (ctx.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            m_JumpInputStartTime = Time.time;
        }

        if (ctx.canceled)
        {
            JumpInputStop = true;
        }
    }

    public void OnGrabInput(InputAction.CallbackContext ctx) 
    {
        if (ctx.performed)
        {
            GrabInput = true;
        }

        if (ctx.canceled)
        {
            GrabInput = false;
        }
    }


    public void UseJumpInput() => JumpInput = false;

    #endregion

    #region Unity Functions
    private void Update()
    {
        CheckJumpInputHoldTime();
    }
    #endregion

    #region Private Functions

    private void CheckJumpInputHoldTime() 
    {
        if (Time.time >= m_JumpInputStartTime + m_InputHoldTime)
        {
            JumpInput = false;
        }
    }

    #endregion
}