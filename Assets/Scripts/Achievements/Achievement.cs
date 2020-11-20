using UnityEngine;

public abstract class Achievement : MonoBehaviour
{
    [SerializeField] string achievementName = string.Empty;
    [SerializeField] GameObject[] stamps = default;

    protected virtual void Awake()
    {
        //check if activate stamps
        ActiveStamp(LoadAchievement());
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
}
