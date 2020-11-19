using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[AddComponentMenu("Hitman GO/Managers/Functions UI")]
public class FunctionsUI : MonoBehaviour
{
    public static FunctionsUI instance { get; private set; }

    Resolution newResolution;

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
        int currentQuality = QualitySettings.GetQualityLevel();

        //set quality settings
        QualitySettings.SetQualityLevel(currentQuality + 1, true);
        int newQuality = currentQuality + 1;

        //if reached limit (no change in quality), set quality to 0
        if (QualitySettings.GetQualityLevel() == currentQuality)
        {
            QualitySettings.SetQualityLevel(0, true);
            newQuality = 0;
        }

        //UI
        string s = QualitySettings.names[newQuality];
        FindObjectOfType<OptionsUI>().UpdateQualityText(s);
    }

    public void ChangeResolution()
    {
        Resolution currentResolution = Screen.currentResolution;

        for(int i = 0; i < Screen.resolutions.Length; i++)
        {
            //find current resolution in the list
            if(Screen.resolutions[i].width == currentResolution.width && Screen.resolutions[i].height == currentResolution.height)
            {
                //get next resolution or the back to 0
                int nextResolution = i < Screen.resolutions.Length - 1 ? i + 1 : 0;
                newResolution = Screen.resolutions[nextResolution];
            }
        }

        //UI
        string s = newResolution.width + " x " + newResolution.height + ", " + newResolution.refreshRate + "Hz";
        FindObjectOfType<OptionsUI>().UpdateResolutionText(s);
    }

    public void SetResolution()
    {
        Screen.SetResolution(newResolution.width, newResolution.height, Screen.fullScreen, newResolution.refreshRate);
    }

    public void SetFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;

        //UI
        string s = Screen.fullScreen ? "FULLSCREEN" : "WINDOW MODE";
        FindObjectOfType<OptionsUI>().UpdateFullScreenText(s);
    }

    public void SetVSync()
    {
        QualitySettings.vSyncCount = QualitySettings.vSyncCount <= 0 ? 1 : 0;

        //UI
        string s = QualitySettings.vSyncCount <= 0 ? "V SYNC OFF" : "V SYNC ON";
        FindObjectOfType<OptionsUI>().UpdateVSyncText(s);
    }
}
