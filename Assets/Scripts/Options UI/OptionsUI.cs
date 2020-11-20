using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Hitman GO/UI/Options UI")]
public class OptionsUI : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] GameObject[] soundOn = default;
    [SerializeField] GameObject[] soundOff = default;

    [Header("Music")]
    [SerializeField] GameObject[] musicOn = default;
    [SerializeField] GameObject[] musicOff = default;

    [Header("Quality")]
    [SerializeField] Text[] quality = default;
    [SerializeField] string beforeQuality = "QUALITY - ";

    [Header("Resolution")]
    [SerializeField] Text[] resolution = default;

    [Header("Full Screen")]
    [SerializeField] GameObject[] fullScreenOn = default;
    [SerializeField] GameObject[] fullScreenOff = default;

    [Header("VSync")]
    [SerializeField] GameObject[] vSyncOn = default;
    [SerializeField] GameObject[] vSyncOff = default;

    void Start()
    {
        UpdateSoundText(FunctionsUI.instance.Sound);
        UpdateMusicText(FunctionsUI.instance.Music);
        UpdateQualityText();
        UpdateResolutionText(Screen.currentResolution);
        UpdateFullScreenText();
        UpdateVSyncText();
    }

    public void UpdateSoundText(bool isOn)
    {
        //set on and off
        foreach (GameObject on in soundOn)
            on.SetActive(isOn);

        foreach (GameObject off in soundOff)
            off.SetActive(!isOn);
    }

    public void UpdateMusicText(bool isOn)
    {
        //set on and off
        foreach (GameObject on in musicOn)
            on.SetActive(isOn);

        foreach (GameObject off in musicOff)
            off.SetActive(!isOn);
    }

    public void UpdateQualityText()
    {
        //set text
        foreach (Text qualityText in quality)
            qualityText.text = beforeQuality + QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    public void UpdateResolutionText(Resolution newResolution)
    {
        foreach (Text resolutionText in resolution)
            resolutionText.text = newResolution.width + " x " + newResolution.height + ", " + newResolution.refreshRate + "hz";
    }

    public void UpdateFullScreenText()
    {
        bool isOn = Screen.fullScreen;

        //set on and off
        foreach (GameObject on in fullScreenOn)
            on.SetActive(isOn);

        foreach (GameObject off in fullScreenOff)
            off.SetActive(!isOn);
    }

    public void UpdateVSyncText()
    {
        bool isOn = QualitySettings.vSyncCount > 0;

        //set on and off
        foreach (GameObject on in vSyncOn)
            on.SetActive(isOn);

        foreach (GameObject off in vSyncOff)
            off.SetActive(!isOn);
    }
}
