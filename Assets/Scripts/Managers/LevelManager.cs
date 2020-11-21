using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

[AddComponentMenu("Hitman GO/Managers/Level Manager")]
public class LevelManager : StateMachine
{
    #region variables
    [Header("Prelevel")]
    [SerializeField] float timeCinemachine = 5;

    [Header("Gameplay")]
    [Tooltip("Minimum time duration for enemy turn (when there is no enemy, or every enemy is in idle)")] 
    [SerializeField] float minimumEnemyTurnDuration = 0f;
    [SerializeField] int rockAreaEffect = 1;

    [Header("Enemies")]
    [SerializeField] float timeDeathAnimation = 0.7f;
    [SerializeField] Transform[] positionsToDeath = default;
    int deathPositionIndex;

    [Header("Sounds")]
    [SerializeField] AudioClip soundWin = default;
    [SerializeField] AudioClip soundLose = default;

    public float TimeCinemachine => timeCinemachine;
    public float MinimumEnemyTurnDuration => minimumEnemyTurnDuration;
    public int RockAreaEffect => rockAreaEffect;

    //every enemies in scene
    public List<Enemy> enemiesInScene { get; set; }

    public bool isAgainSameLevel { get; set; }

    //every enemy that must to end turn
    List<Enemy> enemiesInMovement = new List<Enemy>();

    bool isGameEnded;

    AudioSource audio;

    #endregion

    void Start()
    {
        //find every enemy and start prelevel state
        enemiesInScene = FindObjectsOfType<Enemy>().ToList();
        SetState(new PrelevelState(this));

        audio = GetComponent<AudioSource>();
    }

    #region private API

    IEnumerator EndEnemyTurnAfterFewSeconds()
    {
        //called when there is no enemy in scene

        //wait
        yield return new WaitForSeconds(MinimumEnemyTurnDuration);

        //end enemy turn
        EndEnemyTurn(null);
    }

    IEnumerator PlayerDeath()
    {
        //wait player death animation
        yield return new WaitForSeconds(1.5f);

        //wait click
        while (true)
        {
            if (OnClick())
                break;

            yield return null;
        }

        //reload
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator EnemyDeathAnimation(Enemy enemy, Transform deathPosition)
    {
        Vector3 positionToDeath = new Vector3(deathPosition.position.x, enemy.transform.position.y, deathPosition.position.z);

        //wait
        yield return new WaitForSeconds(timeDeathAnimation);

        //move to position
        enemy.transform.position = positionToDeath;
    }

    bool OnClick()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        //if touch began
        if (Input.touchCount > 0)
        {
            if(Input.GetTouch(0).phase == TouchPhase.Began)
            {
                return true;
            }
        }
#else
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            return true;
        }
#endif

        return false;
    }

    #endregion

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

    public void EnemyDeath(Enemy enemy)
    {
        //remove from list
        enemiesInScene.Remove(enemy);

        //move to death position
        if (deathPositionIndex < positionsToDeath.Length)
            StartCoroutine(EnemyDeathAnimation(enemy, positionsToDeath[deathPositionIndex]));

        //increase index
        deathPositionIndex++;
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
            //check every achievement
            foreach (Achievement achievement in GetComponents<Achievement>())
                achievement.CheckAchievement(win);

            //deactive player and active end menu
            GameManager.instance.player.enabled = false;
            GameManager.instance.uiManager.EndMenu(true);
        }
        else
        {
            //restart scene
            StartCoroutine(PlayerDeath());
        }

        //play sound
        audio.clip = win ? soundWin : soundLose;
        audio.Play();
    }

    #endregion
}
