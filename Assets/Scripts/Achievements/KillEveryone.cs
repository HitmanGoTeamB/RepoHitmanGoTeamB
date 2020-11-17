using UnityEngine;

[AddComponentMenu("Hitman GO/Achievements/Kill Everyone")]
public class KillEveryone : Achievement
{
    protected override bool CheckSucceeded(bool win)
    {
        //check if there are no enemies
        return GameManager.instance.LevelManager.enemiesInScene.Count <= 0;
    }
}
