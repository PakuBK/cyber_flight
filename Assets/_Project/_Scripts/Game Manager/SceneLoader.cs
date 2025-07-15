using UnityEngine.SceneManagement;

namespace CF {

public static class SceneLoader 
{
    public static void LoadNextScene()
    {
        var buildIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (SceneManager.sceneCountInBuildSettings <= buildIndex) buildIndex = 0;

        SceneManager.LoadScene(buildIndex);
    }

    public static void LoadPreviousScene()
    {
        var buildIndex = SceneManager.GetActiveScene().buildIndex - 1;
        if (0 > buildIndex) buildIndex = 0;
        SceneManager.LoadScene(buildIndex);
    }

    public static void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void LoadIndexScene(int index)
    {
        SceneManager.LoadScene(index);
    }
   
    public static string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    public static void LoadMenuScene()
    {
        SceneManager.LoadScene(0);
    }

    public static void LoadUIBackground()
    {
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
    }

    public static void LoadMainGame()
    {
        SceneManager.LoadScene(2);
    }
}
}