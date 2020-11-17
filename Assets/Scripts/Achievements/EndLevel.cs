using UnityEngine;

[AddComponentMenu("Hitman GO/Achievements/End Level")]
public class EndLevel : Achievement
{
    protected override bool CheckSucceeded(bool win)
    {
        //check if won the level
        return win;
    }
}
