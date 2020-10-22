using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : StateMachine
{
    Enemy[] enemiesInScene;
    List<Enemy> EnemyReadyToFinish = new List<Enemy>();

    // Start is called before the first frame update
    void Start()
    {
        enemiesInScene = FindObjectsOfType<Enemy>();
        SetState(new PrelevelState(this));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndPlayerTurn()
    {
        SetState(new EnemyTurnState(this));
    }

    public void EndEnemyTurn(Enemy enemy)
    {
        if (EnemyReadyToFinish.Contains(enemy))
        {
            EnemyReadyToFinish.Remove(enemy);
        }
        
        if(EnemyReadyToFinish.Count <= 0)
            SetState(new PlayerTurnState(this));
    }

    public void StartPlayerTurn()
    {
        GameManager.instance.player.ActivePlayer();       
    }

    public void StartEnemyTurn()
    {
        foreach(Enemy enemy in enemiesInScene)
        {
            EnemyReadyToFinish.Add(enemy);
            enemy.ActiveEnemy();
        }
    }
}
