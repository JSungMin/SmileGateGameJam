using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor {

    public int score;
    public float disToPlayer;
    public void UpdateDistanceToPlayer()
    {
        float disX = Player.instance.transform.position.x - transform.position.x;
        float disY = Player.instance.transform.position.y - transform.position.y;
        
        disToPlayer = Mathf.Sqrt(Mathf.Pow(disX, 2) + Mathf.Pow(disY, 2));
    }

    public delegate void InteractFunc ();
    public event InteractFunc OnDamaged;

    public void Damaged(float val)
    {
        acInfo.hp -= val;
        if (null != OnDamaged.Method)
        {
            OnDamaged.Invoke();
        }
    }

}
