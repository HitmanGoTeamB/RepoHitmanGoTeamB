﻿using UnityEngine;

[AddComponentMenu("Hitman GO/Achievements/No Kill")]
public class NoKill : Achievement
{
    Enemy[] startingEnemiesInScene;

    void Start()
    {
        startingEnemiesInScene = FindObjectsOfType<Enemy>();
    }

    protected override bool CheckSucceeded(bool win)
    {
        //check if there are all enemies in scene
        return GameManager.instance.LevelManager.enemiesInScene.Count >= startingEnemiesInScene.Length;
    }
}
