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

    public string State;

    private new void OnEnable()
    {
        base.OnEnable();
        isMoving = false;
    }

    void Start()
    {
        StartCoroutine(miniPattern());
        
    }

    public void getState()
    {
        if (disToPlayer > farDisance || (disToPlayer >= nearDistance && disToPlayer < normalDistance))
        {
            State = "Far";
        }
        else if(disToPlayer < nearDistance)
        {
            State = "Near";
        }
        else
        {
            State = "Normal";
        }
    }
    
    IEnumerator miniPattern()
    {
        float timer = 0f;
        Vector3 dir = new Vector3(0, 0);

        while(true)
        {
            getState();
            if(State == "Near" || State == "Normal")
            {
                timer = 0f;
                dir = Player.GetInstance.bodyCollider.bounds.center - bodyCollider.bounds.center;
                dir = dir.normalized;
                if(State == "Near")
                    acInfo.speed = fastSpeed;
                else
                    acInfo.speed = slowSpeed;
                Move(dir);
            }
            else if (!isMoving)
            {
                acInfo.speed = normalSpeed;
                Vector3 dir1 = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
                Vector3 dir2 = (Player.GetInstance.bodyCollider.bounds.center - bodyCollider.bounds.center).normalized;
                dir = (dir1 + dir2).normalized;
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
                rigid.velocity = Vector3.zero;
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
