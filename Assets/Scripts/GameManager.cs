using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public Map map { get; private set; }

    void Awake()
    {
        //singleton
        if(instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        //set references
        instance.SetReferences();
    }

    void SetReferences()
    {
        map = FindObjectOfType<Map>();
    }
}
