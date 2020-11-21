using UnityEngine;

[System.Serializable]
public struct HintStruct
{
    public Waypoint waypointToReach;
    public GameObject hintToActivate;
}

public abstract class Achievement : MonoBehaviour
{
    public static Achievement hintActive { get; private set; }

    [Header("Important")]
    [SerializeField] string achievementName = string.Empty;
    [SerializeField] GameObject[] stamps = default;

    [Header("Hint")]
    [SerializeField] GameObject hintMode = default;
    [SerializeField] HintStruct[] hints = default;

    protected virtual void Awake()
    {
        //check if activate stamps
        ActiveStamp(LoadAchievement());

        //deactive every hint
        hintMode.SetActive(false);
        foreach (HintStruct hint in hints)
            hint.hintToActivate.SetActive(false);
    }

    public bool CheckAchievement(bool win)
    {
        //check if achievement succeeded
        bool succeeded = CheckSucceeded(win);

        //load achievement
        bool achievementLoaded = LoadAchievement();

        //if achievement was not already completed and player succeeded, save it
        if (achievementLoaded == false && succeeded)
            SaveAchievement();

        //active stamp (graphics)
        ActiveStamp(succeeded || achievementLoaded);

        //return completed if succeeded or achievement was already completed
        return succeeded || achievementLoaded;
    }

    protected abstract bool CheckSucceeded(bool win);

    public bool LoadAchievement()
    {
        //do only if there is an achievement name
        if (achievementName == string.Empty)
            return false;

        //return achievement, completed or not
        return PlayerPrefs.GetInt(achievementName, 0) >= 1 ? true : false;
    }

    public void ActiveStamp(bool achievementCompleted)
    {
        //active or deactive
        foreach (GameObject stamp in stamps)
            stamp.SetActive(achievementCompleted);
    }

    void SaveAchievement()
    {
        //do only if there is an achievement name
        if (achievementName == string.Empty)
            return;

        //save it succeeded
        PlayerPrefs.SetInt(achievementName, 1);
    }

    void ActivateHintsAfterFewSeconds()
    {
        //active hint mode and first hint
        hintMode.SetActive(true);
        hints[0].hintToActivate.SetActive(true);
    }

    #region hints

    public void StartHints()
    {
        //save reference on game manager and restart
        GameManager.instance.StartHints(this);
    }

    public void ActivateHints()
    {
        //set reference
        hintActive = this;

        Invoke("ActivateHintsAfterFewSeconds", 0.1f);
    }

    public void CheckHint(Waypoint waypoint)
    { 
        //check what hint is active
        for(int i = 0; i < hints.Length; i++)
        {
            if(hints[i].hintToActivate.activeInHierarchy)
            {
                //if correct waypoint
                if(hints[i].waypointToReach == waypoint)
                {
                    //deactivate this hint
                    hints[i].hintToActivate.SetActive(false);

                    //and activate next (or just remove reference when is the last one)
                    if (i < hints.Length - 1)
                        hints[i + 1].hintToActivate.SetActive(true);
                    else
                        hintActive = null;

                    return;
                }
            }
        }

        //else deactivate hints
        DeactivateHints();
    }

    public void DeactivateHints()
    {
        //remove hint mode
        hintMode.SetActive(false);

        //remove reference
        hintActive = null;
    }

    #endregion
}
