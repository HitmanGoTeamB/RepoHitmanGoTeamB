using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu("Hitman GO/Achievements/Kill Everyone")]
public class KillEveryone : Achievement
{
    public override bool CheckAchievement(bool win)
    {
        //check if there are no enemies
        return GameManager.instance.LevelManager.enemiesInScene.Count <= 0;
    }
}
