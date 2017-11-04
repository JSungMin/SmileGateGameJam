using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObserver : MonoBehaviour {

    public static GameObserver instance;
    public static GameObserver GetInstance
    {
        get
        {
            if (null == instance)
            {
                instance = GameObject.FindObjectOfType<GameObserver>();
                if (null == instance)
                    Debug.LogError("Can't Find GameObserver");
            }
            return instance;
        }
    }

    public List<Enemy> enemiesList = new List<Enemy>();

    void Update()
    {
        for(int i = 0; i < enemiesList.Count; i++)
        {
            enemiesList[i].UpdateDistanceToPlayer();
        }
    }

}
