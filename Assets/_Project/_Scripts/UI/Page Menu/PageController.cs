using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CF.UI {
public class PageController : MonoBehaviour
{
    public static PageController instance;

    public bool debug;
    public PageType entryPage;
    public Page[] pages;

    private Hashtable m_Pages;
    public PageType currentPage { get; private set; } = PageType.None;

    public Stack<PageType> additivePages = new Stack<PageType>();

    public Stack<PageType> pageHistory = new Stack<PageType>();

    #region Public Functions
    

    public void OpenFullPage(PageType _type, bool transition = false) 
    {
        pageHistory.Push(_type);
        Log("Pushed + [" + _type + "]");
        if (transition)
        {
            TransitionToPage(_type);
        }
        else
        {
            TurnPageOn(_type);
        }
        
    }

    public void CloseFullPage() 
    {
        RevertToLastPage();
    }

    public void OpenAdditivePage(PageType _type) 
    {
        additivePages.Push(_type);
        TurnPageOn(_type);
    }

    public void CloseAdditivePage() 
    {
        TurnPageOff(additivePages.Pop());
    }


    public void CloseAllPages() 
    {
        for (int i = 0; i < additivePages.Count; i++)
        {
            CloseAdditivePage();
        }

        TurnPageOff(currentPage);

        pageHistory.Clear();
    }

    #endregion

    #region Unity Function

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            m_Pages = new Hashtable();
            RegisterAllPages();

            if (entryPage != PageType.None)
            {
                OpenFullPage(entryPage);
            }
        }
    }

    #endregion

    #region Private Functions

    private void TransitionToPage(PageType _targetPage)
    {
        Page _offPage = GetPage(currentPage);

        if (_offPage.gameObject.activeSelf)
        {
            _offPage.Animate(false);
        }

        TurnPageOn(_targetPage);
    }

    private void TurnPageOn(PageType _type)
    {
        if (!CheckPage(_type, "Turn Page On")) return;

        Page _page = GetPage(_type);
        _page.gameObject.SetActive(true);
        _page.Animate(true);

        // Select the entry Button
        GameObject _button = _page.entryButton;
        if (!_button) 
        {
            LogWarning("There is no entry Button - Check the Inspecter of Page [" + _type + "].");
            return;
        }
        _button.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_button);

        currentPage = _type;
    }

    private void TurnPageOff(PageType _off, PageType _on = PageType.None, bool _waitForExit = false)
    {
        if (!CheckPage(_off, "Turn Page Off")) return;

        Page _offPage = GetPage(_off);
        if (_offPage.gameObject.activeSelf)
        {
            _offPage.Animate(false);
        }

        if (_waitForExit)
        {
            Page _onPage = GetPage(_on);
            StartCoroutine(WaitForPageExit(_onPage, _offPage));
        }
        else
        {
            TurnPageOn(_on);
        }
    }

    private void RevertToLastPage()
    {
        if (pageHistory.Count == 0) return;

        var _page = pageHistory.Pop();
        Log("Popped: " + _page);
        TransitionToPage(pageHistory.Peek());
        Log("PEEK: " + pageHistory.Peek());
    }

    private IEnumerator WaitForPageExit(Page _on, Page _off)
    {
        while (_off.targetState != Page.FlagNone)
        {
            yield return null;
        }

        TurnPageOn(_on.type);
    }

    private void RegisterAllPages()
    {
        foreach (Page _page in pages)
        {
            RegisterPage(_page);
        }
    }

    private void RegisterPage(Page _page)
    {
        if (PageExists(_page.type))
        {
            LogWarning("You're trying to register a page that has already been registered [" + _page.gameObject.name + "]");
            return;
        }

        m_Pages.Add(_page.type, _page);
        Log("Registered new Page [" + _page.type + "]");

    }

    private Page GetPage(PageType _type)
    {
        if (!PageExists(_type))
        {
            LogWarning("You're trying to get a Page that hasn't been registered [" + _type + "]");
            return null;
        }

        return (Page)m_Pages[_type];
    }

    private bool PageExists(PageType _type)
    {
        return m_Pages.ContainsKey(_type);
    }

    private bool CheckPage(PageType _type, string _ctx = "N/A")
    {
        if (_type == PageType.None) return false;
        if (!PageExists(_type))
        {
            LogWarning("You're trying to access a page [" + _type + "] that has not been registered. Reason [" + _ctx + "]");
            return false;
        }
        return true;
    }

    private void Log(string _msg)
    {
        if (!debug) return;
        Debug.Log("[Page Controller]: " + _msg);
    }
    private void LogWarning(string _msg)
    {
        if (!debug) return;
        Debug.LogWarning("[Page Controller]: " + _msg);
    }

    #endregion
}
}
