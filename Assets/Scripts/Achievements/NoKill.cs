using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu("Hitman GO/Achievements/No Kill")]
public class NoKill : Achievement
{
    Enemy[] startingEnemiesInScene;

    void Start()
    {
        startingEnemiesInScene = FindObjectsOfType<Enemy>();
    }

    public override bool CheckAchievement(bool win)
    {
        //check if there are all enemies in scene
        return GameManager.instance.LevelManager.enemiesInScene.Count >= startingEnemiesInScene.Length;
    }
}
