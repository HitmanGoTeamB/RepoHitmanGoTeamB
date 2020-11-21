using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[AddComponentMenu("Hitman GO/Managers/Functions UI")]
public class FunctionsUI : MonoBehaviour
{
    public static FunctionsUI instance { get; private set; }

    Resolution newResolution;
    public bool Music { get; private set; } = true;
    public bool Sound { get; private set; } = true;

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
            SetDefault();
        }
    }

    void SetDefault()
    {
        //load
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality", QualitySettings.GetQualityLevel()));      //get quality - default is current quality
        Screen.fullScreen = PlayerPrefs.GetInt("FullScreen", Screen.fullScreen ? 1 : 0) > 0 ? true : false;     //get full screen - default is current full screen (1 = true, 0 = false)
        newResolution = Screen.resolutions[PlayerPrefs.GetInt("Resolution", FindCurrentResolution(Screen.currentResolution))];          //get resolution - default is current resolution
        if (EqualResolution() == false) Screen.SetResolution(newResolution.width, newResolution.height, Screen.fullScreen, newResolution.refreshRate);  //if new resolution, set to screen
        QualitySettings.vSyncCount = PlayerPrefs.GetInt("VSync", QualitySettings.vSyncCount);                   //get vsync - default is current vsync
        Sound = PlayerPrefs.GetInt("Sound", 1) > 0 ? true : false;                                              //get sound - default is 1 (1 = true, 0 = false)
        Music = PlayerPrefs.GetInt("Music", 1) > 0 ? true : false;                                              //get sound - default is 1 (1 = true, 0 = false)
    }

    #region private API

    int FindCurrentResolution(Resolution resolutionToFind)
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            //find current resolution in the list
            if (Screen.resolutions[i].width == resolutionToFind.width 
                && Screen.resolutions[i].height == resolutionToFind.height 
                && Screen.resolutions[i].refreshRate == resolutionToFind.refreshRate)
            {
                return i;
            }
        }

        return 0;
    }

    bool EqualResolution()
    {
        if (newResolution.width == Screen.currentResolution.width
            && newResolution.height == Screen.currentResolution.height
            && newResolution.refreshRate == Screen.currentResolution.refreshRate)
        {
            return true;
        }

        return false;
    }

    #endregion

    #region scene

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadURL(string url)
    {
        Application.OpenURL(url);
    }

    public void Toggle(GameObject obj)
    {
        if (obj.activeInHierarchy)
            obj.SetActive(false);
        else
            obj.SetActive(true);
    }

    #endregion

    #region options

    public void ChangeQuality()
    {
        int currentQuality = QualitySettings.GetQualityLevel();
        int nextQuality = currentQuality + 1;

        //set quality settings
        QualitySettings.SetQualityLevel(currentQuality + 1, true);

        //if reached limit (no change in quality), set quality to 0
        if (QualitySettings.GetQualityLevel() == currentQuality)
        {
            QualitySettings.SetQualityLevel(0, true);
            nextQuality = 0;
        }

        //UI
        FindObjectOfType<OptionsUI>().UpdateQualityText(nextQuality);

        //Save
        PlayerPrefs.SetInt("Quality", nextQuality);
    }

    public void SetFullScreen(bool setOn)
    {
        Screen.fullScreen = setOn;

        //UI
        FindObjectOfType<OptionsUI>().UpdateFullScreenText(setOn);

        //Save
        PlayerPrefs.SetInt("FullScreen", setOn ? 1 : 0);
    }

    public void ChangeResolution(bool forward)
    {
        int currentResolution = FindCurrentResolution(newResolution);
        int nextResolution = 0;

        //get next resolution
        if (forward)
        {
            //forward or back to 0
            nextResolution = currentResolution < Screen.resolutions.Length - 1 ? currentResolution + 1 : 0;
        }
        else
        {
            //back or go to last index
            nextResolution = currentResolution > 0 ? currentResolution - 1 : Screen.resolutions.Length - 1;
        }

        //set new resolution
        newResolution = Screen.resolutions[nextResolution];

        //UI
        FindObjectOfType<OptionsUI>().UpdateResolutionText(newResolution);
    }

    public void SetResolution()
    {
        Screen.SetResolution(newResolution.width, newResolution.height, Screen.fullScreen, newResolution.refreshRate);

        //Save
        PlayerPrefs.SetInt("Resolution", FindCurrentResolution(newResolution));
    }

    public void SetVSync(bool setOn)
    {
        QualitySettings.vSyncCount = setOn ? 1 : 0;

        //UI
        FindObjectOfType<OptionsUI>().UpdateVSyncText(setOn);

        //Save
        PlayerPrefs.SetInt("VSync", setOn ? 1 : 0);
    }

    public void SetSound(bool setOn)
    {
        Sound = setOn;

        //UI
        FindObjectOfType<OptionsUI>().UpdateSoundText(setOn);

        //Save
        PlayerPrefs.SetInt("Sound", setOn ? 1 : 0);
    }

    public void SetMusic(bool setOn)
    {
        Music = setOn;

        //UI
        FindObjectOfType<OptionsUI>().UpdateMusicText(setOn);

        //Save
        PlayerPrefs.SetInt("Music", setOn ? 1 : 0);
    }

    public void ResetGame()
    {
        //delete all
        PlayerPrefs.DeleteAll();

        //save options
        PlayerPrefs.SetInt("Quality", QualitySettings.GetQualityLevel());
        PlayerPrefs.SetInt("FullScreen", Screen.fullScreen ? 1 : 0);
        PlayerPrefs.SetInt("Resolution", FindCurrentResolution(newResolution));
        PlayerPrefs.SetInt("VSync", QualitySettings.vSyncCount > 0 ? 1 : 0);
        PlayerPrefs.SetInt("Sound", Sound ? 1 : 0);
        PlayerPrefs.SetInt("Music", Music ? 1 : 0);
    }

    #endregion
}
