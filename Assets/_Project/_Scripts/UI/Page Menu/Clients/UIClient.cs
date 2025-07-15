using UnityEngine;

namespace CF.UI {
public class UIClient : MonoBehaviour
{

    public PageController pageController;

    private void Start()
    {
        SceneLoader.LoadUIBackground();
    }

    #region Public Functions

    public void StartGame()
    {
        SceneLoader.LoadMainGame();
    }

    #region Open Page Functions
    public void OpenMenuPage()
    {
        pageController.OpenFullPage(PageType.Menu, true);
    }

    public void OpenDockPage()
    {
        pageController.OpenFullPage(PageType.Dock, true);
    }

    public void OpenWorkshop()
    {
        pageController.OpenFullPage(PageType.Workshop, true);
    }
    public void OpenLoadingPage()
    {
        pageController.OpenFullPage(PageType.Loading, true);
    }

    public void OpenSettingsPage()
    {
        pageController.OpenFullPage(PageType.Options, true);
    }
    #endregion
    public void GoBackAPage() 
    {
        pageController.CloseFullPage();
    }

    #endregion

    #region Private Functions

    private void ToggleAdditivePage(PageType _type) 
    {
        if (pageController.additivePages.Count != 0)
        {
            if (pageController.additivePages.Peek() == _type)
            {
                pageController.CloseAdditivePage();
            }
        }
        else
        {
            pageController.OpenAdditivePage(_type);
        }
    }

    private PageType GetPage(int _value) 
    {
        return (PageType)_value;
    }

    #endregion
}
}