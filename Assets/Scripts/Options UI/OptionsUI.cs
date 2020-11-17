using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [Header("PC")]
    [SerializeField] Text resolutionText = default;
    [SerializeField] Text fullScreenText = default;
    [SerializeField] Text vSyncText = default;

    [Header("Mobile")]
    [SerializeField] Text qualityText = default;

    public void UpdateResolutionText(string text)
    {
        resolutionText.text = text;
    }

    public void UpdateFullScreenText(string text)
    {
        fullScreenText.text = text;
    }

    public void UpdateVSyncText(string text)
    {
        vSyncText.text = text;
    }

    public void UpdateQualityText(string text)
    {
        qualityText.text = text;
    }
}
