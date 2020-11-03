using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu("Hitman GO/Achievements/End Level")]
public class EndLevel : Achievement
{
    public override bool CheckAchievement(bool win)
    {
        //check if won the level
        return win;
    }
}
