using Com.LuisPedroFonseca.ProCamera2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniEnemy : Enemy {
   
    public bool isMoving;
    public bool isMeeting;

    public float farDisance;
    public float normalDistance;
    public float nearDistance;

    public float fastSpeed;
    public float normalSpeed;
    public float slowSpeed;

    public string State;
    public int animationIndex = 0;
    private string[] batAnim = {
        "Player_bet_attack0",
        "Player_bet_attack1"
    };
    private string[] keyboardAnim = {
        "Player_keyboard_attack0",
        "Player_keyboard_attack1"
    };
    private string[] mouseAnim = {
        "Player_mouse_attack0",
        "Player_mouse_attack1"
    };

    private new void OnEnable()
    {
        base.OnEnable();
        
        isMoving = false;
        isMeeting = false;
    }

    public void InitChecker ()
    {
        StartCoroutine(miniPattern());
        StartCoroutine(hitCheck());
    }

    void Start()
    {
        // Equipt Default Weapon
        skel.state.Event += HandleHitEvent;
        skel.state.Event += HandleStartEvent;
        skel.state.Event += HandleEndEvent;
    
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

		if (acInfo.isKnockbacking) {
			rigid.velocity = Vector3.Lerp (rigid.velocity, Vector3.zero, Time.deltaTime * 10f);
		}

		while(!acInfo.isDead)
        {
            if(!isMeeting)
            {
                getState();
                if (State == "Near" || State == "Normal")
                {
                    timer = 0f;
                    dir = Player.GetInstance.bodyCollider.bounds.center - bodyCollider.bounds.center;
                    dir = dir.normalized;
                    if (State == "Near")
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
                else if (timer >= 1f && timer < 2f)
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
            }

            yield return null;
        }
    }
    
    IEnumerator hitCheck()
    {
		while(!acInfo.isDead)
        {
            //Debug.Log("Max: " + bodyCollider.bounds.max.y);
            //Debug.Log("Min: " + bodyCollider.bounds.min.y);
            //Debug.Log("Center: " + Player.GetInstance.bodyCollider.bounds.center.y);
            //Debug.Log("Dis: " + disToPlayer);
            //Debug.Log("Reach: " + nowWeaponInfo.reach);
            if((bodyCollider.bounds.max.y > Player.GetInstance.bodyCollider.bounds.center.y
                && bodyCollider.bounds.min.y < Player.GetInstance.bodyCollider.bounds.center.y)
                && disToPlayer < nowWeaponInfo.reach)
            {

                isMeeting = true;
                NormalAttack();
                
                yield return new WaitForSeconds(0.3f);
                Move(Vector3.zero);
                isMeeting = false;

                yield return new WaitForSeconds(0.5f);
                
            }
            else
            {
                isMeeting = false;
            }

            yield return null;
        }
    }

    public void NormalAttack()
    {
		if (acInfo.isDashing || acInfo.isAttacking || acInfo.isDead)
            return;
        skel.state.ClearTrack(0);
        rigid.velocity = Vector3.zero;
		SetAnimation (0, acInfo.name + "_attack0", false, 1f);        
    }

    void HandleStartEvent(Spine.TrackEntry entry, Spine.Event e)
    {
        if (e.Data.Name == "Start")
        {
            acInfo.isAttacking = true;
            Debug.Log("SS");
        }
    }
    void HandleEndEvent(Spine.TrackEntry entry, Spine.Event e)
    {
        if (e.Data.Name == "End")
        {
            Debug.Log("EE");
            acInfo.isAttacking = false;
        }
    }
	public ParticleSystem hitEffect;
    void HandleHitEvent(Spine.TrackEntry entry, Spine.Event e)
    {
        if (e.Data.Name == "Hit")
        {
			Vector3 center = transform.position + (int)lookDir * Vector3.right * nowWeaponInfo.reach * 0.5f;
			var hittedObjs = Physics.OverlapBox(center, Vector3.right * nowWeaponInfo.reach * 0.5f + Vector3.up * bodyCollider.bounds.size.y * 0.5f + Vector3.forward * 2f, Quaternion.identity, 1 << LayerMask.NameToLayer("Player"));

			for(int i = 0; i < hittedObjs.Length; i++)
			{
				var obj = hittedObjs[i];
				Debug.Log(obj.name);
				var player = obj.GetComponent<Player>();
				if (null != player)
				{
					Debug.Log("Hit!");
					player.Damaged(nowWeaponInfo.damage, (player.transform.position - transform.position).normalized);
				}

				animationIndex = 0;
			}
			if (null != hitEffect)
				hitEffect.Play ();
        }
    }

}
