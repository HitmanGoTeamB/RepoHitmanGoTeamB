using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[AddComponentMenu("Hitman GO/Managers/Functions UI")]
public class FunctionsUI : MonoBehaviour
{
    public static FunctionsUI instance { get; private set; }

    void Awake()
    {
        //singleton
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadURL(string url)
    {
        Application.OpenURL(url);
    }

    public void ChangeQuality()
    {

    }
}
