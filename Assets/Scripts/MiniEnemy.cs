using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniEnemy : Enemy {
   
    public bool isMoving;

    public float farDisance;
    public float normalDistance;
    public float nearDistance;

    public float fastSpeed;
    public float normalSpeed;
    public float slowSpeed;

    new void OnEnable()
    {
        isMoving = false;
    }

    void Start()
    {
        StartCoroutine(miniPattern());

    }

    public Vector3 getMove()
    {
        Vector3 dir;
        if (disToPlayer > farDisance || (disToPlayer >= nearDistance && disToPlayer < normalDistance))
        {
            acInfo.speed = normalSpeed;
            Vector3 dir1 = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            Vector3 dir2 = (Player.GetInstance.transform.position - transform.position).normalized;
            dir = (dir1 + dir2).normalized;
        }
        else if (disToPlayer < nearDistance)
        {
            acInfo.speed = fastSpeed;
            dir = Player.GetInstance.transform.position - transform.position;
            dir = dir.normalized;
        }
        else
        {
            acInfo.speed = slowSpeed;
            dir = Player.GetInstance.transform.position - transform.position;
            dir = dir.normalized;
        }

        return dir;
    }
    
    IEnumerator miniPattern()
    {
        float timer = 0f;
        Vector3 dir = new Vector3(0, 0);

        while(true)
        {
            
            if (!isMoving)
            {
                dir = getMove();
                isMoving = true;
            }
            else if (timer < 1f)
            {
                timer += Time.deltaTime;
                Move(dir);
            }
            else if(timer >= 1f && timer < 2f)
            {
                acInfo.speed = 0;
                timer += Time.deltaTime;
            }
            else
            {
                isMoving = false;
                timer = 0f;
            }

            yield return null;
        }
    }

}
