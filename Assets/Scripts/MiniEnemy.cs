using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Seeker))]
[AddComponentMenu("Pathfinding/AI/AISimpleLerp (2D,3D generic)")]
[HelpURL("http://arongranberg.com/astar/docs/class_a_i_lerp.php")]
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
            dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            dir = dir.normalized;
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

        isMoving = true;

        return dir;
    }

    IEnumerator miniPattern()
    {
        float timer = 0f;
        Vector3 dir = new Vector3(0, 0);

        while(true)
        {
            timer += Time.deltaTime;

            if (!isMoving)
            {
                dir = getMove();
                isMoving = true;
            }

            if (timer < 1f)
            {
                Move(dir);
            }

            else if(timer > 2f)
            {
                isMoving = false;
                timer = 0f;
            }

            yield return null;
        }
    }

}
