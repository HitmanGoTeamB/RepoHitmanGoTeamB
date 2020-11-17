﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

[AddComponentMenu("Hitman GO/Managers/Level Manager")]
public class LevelManager : StateMachine
{
    #region variables

    [Tooltip("Minimum time duration for enemy turn (when there is no enemy, or every enemy is in idle)")] 
    [SerializeField] float minimumEnemyTurnDuration = 0f;
    public float MinimumEnemyTurnDuration => minimumEnemyTurnDuration;

    [SerializeField] int rockAreaEffect = 1;
    public int RockAreaEffect => rockAreaEffect;

    //every enemies in scene
    public List<Enemy> enemiesInScene { get; set; }

    //every enemy that must to end turn
    List<Enemy> enemiesInMovement = new List<Enemy>();

    bool isGameEnded;

    #endregion

    void Start()
    {
        //find every enemy and start prelevel state
        enemiesInScene = FindObjectsOfType<Enemy>().ToList();
        SetState(new PrelevelState(this));
    }

    IEnumerator EndEnemyTurnAfterFewSeconds()
    {
        //called when there is no enemy in scene

        //wait
        yield return new WaitForSeconds(MinimumEnemyTurnDuration);

        //end enemy turn
        EndEnemyTurn(null);
    }

    #region public API

    /// <summary>
    /// Called by player
    /// </summary>
    public void EndPlayerTurn()
    {
        SetState(new EnemyTurnState(this));
    }

    /// <summary>
    /// Called by every enemy (or by level manager if there is no enemy)
    /// </summary>
    public void EndEnemyTurn(Enemy enemy)
    {
        //remove this enemy from the list
        if (enemiesInMovement.Contains(enemy))
        {
            enemiesInMovement.Remove(enemy);
        }
        
        //if there is no enemy in the list, end enemy turn
        if(enemiesInMovement.Count <= 0)
        {
            SetState(new PlayerTurnState(this));
        }
    }

    /// <summary>
    /// Called by PlayerTurnState
    /// </summary>
    public void StartPlayerTurn()
    {
        //start player turn
        GameManager.instance.player.ActivePlayer();       
    }

    /// <summary>
    /// Called by EnemyTurnState
    /// </summary>
    public void StartEnemyTurn()
    {
        //start every enemy turn
        foreach(Enemy enemy in enemiesInScene)
        {
            //add to the list, to know when everybody has finished
            enemiesInMovement.Add(enemy);
            enemy.ActiveEnemy();
        }

        //if there is no enemy, end turn after few seconds
        if(enemiesInMovement == null || enemiesInMovement.Count <= 0)
        {
            StartCoroutine(EndEnemyTurnAfterFewSeconds());
        }
    }

    public void SetEnemiesPathFinding(Waypoint waypointToReach)
    {
        //get every waypoint in rock area
        foreach(Waypoint waypoint in GameManager.instance.map.GetWaypointsInArea(waypointToReach, rockAreaEffect))
        {
            //foreach enemy on waypoint, set path finding
            foreach (Enemy enemy in waypoint.GetObjectsOnWaypoint<Enemy>())
                enemy.SetPathFinding(waypointToReach);
        }
    }

    /// <summary>
    /// Called by player when is killed or reached final waypoint
    /// </summary>
    public void EndGame(bool win)
    {
        //do only one time
        if (isGameEnded)
            return;

        isGameEnded = true;

        if(win)
        {
            //show achievement and menu to change level
            //TODO

            //check every achievement
            foreach (Achievement achievement in GetComponents<Achievement>())
                achievement.CheckAchievement(win);

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        else
        {
            //restart scene
            Invoke("EndGame", 2);
        }
    }

    void EndGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion
}
