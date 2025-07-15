using System.Collections;
using UnityEngine;

namespace CF.UI {
public class Page : MonoBehaviour
{
    public static readonly string FlagOn = "On";
    public static readonly string FlagOff = "Off";
    public static readonly string FlagNone = "None";


    public bool debug;
    public PageType type;
    public bool useAnimation;

    public GameObject entryButton;

    public string targetState { get; private set; }

    private Animator m_Animator;

    #region Public Functions
    public void Animate(bool _on)
    {
        if (useAnimation)
        {
            if (!m_Animator) 
            {
                LogWarning("You're trying to use the animator before it's registered");
                return;
            } 
            m_Animator.SetBool("on", _on);

            StopCoroutine("AwaitAnimation");
            StartCoroutine(AwaitAnimation(_on));
        }
        else
        {
            if (!_on)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }
    }
    #endregion

    #region Unity Functions

    private void OnEnable()
    {
        CheckAnimatorIntergrity();
    }

    #endregion

    #region Private Functions

    private IEnumerator AwaitAnimation(bool _on)
    {
        targetState = _on ? FlagOn : FlagOff; // if _on == True than return FlagOn otherwise return FlagOff

        // wait for the animator to reach the target state
        while (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName(targetState))
        {
            yield return null;
        }

        // wait for the animator to finish animating
        while (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) // while the current animator is not finished yield
        {
            yield return null;
        }

        targetState = FlagNone;

        Log("Page [" + type + "] finshed transitioning to " + (_on ? "on" : "off"));

        if (!_on)
        {
            gameObject.SetActive(false);
        }
    }

    private void CheckAnimatorIntergrity()
    {
        if (useAnimation)
        {
            m_Animator = GetComponent<Animator>();
            if (!m_Animator)
            {
                LogWarning("You're trying to animate a page [" + type + "] without an animator.");
            }
        }
    }

    private void Log(string _msg)
    {
        if (!debug) return;
        Debug.Log("[Page]: " + _msg);
    }
    private void LogWarning(string _msg)
    {
        if (!debug) return;
        Debug.LogWarning("[Page]: " + _msg);
    }

    #endregion
}
}

