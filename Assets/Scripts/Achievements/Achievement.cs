using UnityEngine;

public abstract class Achievement : MonoBehaviour
{
    [SerializeField] protected string achievementName = string.Empty;

    public bool CheckAchievement(bool win)
    {
        //check if achievement succeeded
        bool succeeded = CheckSucceeded(win);

        //load achievement
        bool achievementLoaded = LoadAchievement();

        //if achievement was not already completed and player succeeded, save it
        if (achievementLoaded == false && succeeded)
            SaveAchievement();

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

    void SaveAchievement()
    {
        //do only if there is an achievement name
        if (achievementName == string.Empty)
            return;

        //save it succeeded
        PlayerPrefs.SetInt(achievementName, 1);
    }
}
