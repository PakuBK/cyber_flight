using UnityEngine;

namespace CF.UI {
public class IngameMenu : MonoBehaviour
{

    public void RestartLevel()
    {
        SceneLoader.ReloadScene();
    }

    public void GoToMenu()
    {
        SceneLoader.LoadMenuScene();
    }
}
}

