using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CF.UI {
public class UIGameClient : MonoBehaviour
{
    public PageController pageController;

    public Button entryButton;

    private bool m_MenuOpen;
    #region Public Functions

    public void ToggleMenuUIButton() 
    {
        ToggleMenu();
    }

    public void ToggleMenuButton(InputAction.CallbackContext ctx) 
    {
        if (!ctx.performed) { return; }
        ToggleMenu();
    }



    #endregion

    #region Private Functions
    private void ToggleMenu()
    {
        if (m_MenuOpen)
        {
            pageController.CloseAllPages();
            m_MenuOpen = false;
        }
        else
        {
            pageController.OpenFullPage(PageType.Menu);
            SelectButton(entryButton.gameObject);
            m_MenuOpen = true;
        }
    }

    private void SelectButton(GameObject _button) 
    {
        _button.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_button);
    }
    #endregion
}
}