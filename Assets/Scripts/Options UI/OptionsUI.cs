using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Hitman GO/UI/Options UI")]
public class OptionsUI : MonoBehaviour
{
    [Header("PC - Text")]
    [SerializeField] Text resolutionText = default;
    [SerializeField] Text fullScreenText = default;
    [SerializeField] Text vSyncText = default;
    [SerializeField] Text soundText = default;
    [SerializeField] Text musicText = default;

    [Header("Mobile")]
    [SerializeField] Text qualityText = default;
    [SerializeField] Text soundTextMobile = default;
    [SerializeField] Text musicTextMobile = default;

    [Header("String")]
    [SerializeField] string resolutionString = "RESOLUTION - ";
    [SerializeField] string fullScreenString = "FULLSCREEN";
    [SerializeField] string windowScreenString = "WINDOW MODE";
    [SerializeField] string vSyncString = "V SYNC ";
    [SerializeField] string soundString = "SOUND - ";
    [SerializeField] string musicString = "MUSIC - ";

    void Awake()
    {
        UpdateResolutionText(Screen.currentResolution);
        UpdateFullScreenText();
        UpdateVSyncText();
        UpdateQualityText();
        UpdateSoundText(true);
        UpdateMusicText(true);
    }

    public void UpdateResolutionText(Resolution resolution)
    {
        if (resolutionText == null)
            return;

        resolutionText.text = resolutionString + resolution.width + " x " + resolution.height + ", " + resolution.refreshRate + "Hz";
    }

    public void UpdateFullScreenText()
    {
        if (fullScreenText == null)
            return;

        fullScreenText.text = Screen.fullScreen ? fullScreenString : windowScreenString;
    }

    public void UpdateVSyncText()
    {
        if (vSyncText == null)
            return;

        vSyncText.text = QualitySettings.vSyncCount <= 0 ? vSyncString + "OFF" : vSyncString + "ON";
    }

    public void UpdateQualityText()
    {
        qualityText.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    public void UpdateSoundText(bool isOn)
    {
        soundText.text = soundString + (isOn ? "ON" : "OFF");
        soundTextMobile.text = soundString + (isOn ? "ON" : "OFF");
    }

    public void UpdateMusicText(bool isOn)
    {
        musicText.text = musicString + (isOn ? "ON" : "OFF");
        musicTextMobile.text = musicString + (isOn ? "ON" : "OFF");
    }
}
