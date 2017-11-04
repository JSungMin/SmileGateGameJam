using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor {

    public int score;
    public float disToPlayer;

	public new virtual void OnEnable ()
	{
		base.OnEnable ();
	}

    public void UpdateDistanceToPlayer()
    {
        float disX = Player.GetInstance.bodyCollider.bounds.center.x - bodyCollider.bounds.center.x;
        float disY = Player.GetInstance.bodyCollider.bounds.center.y - bodyCollider.bounds.center.y;
        
        disToPlayer = Mathf.Sqrt(Mathf.Pow(disX, 2) + Mathf.Pow(disY, 2));
    }
		
	public void Knockback (Vector3 speed)
	{
		if (!acInfo.isBeatable)
			return;
		rigid.AddForce (speed, ForceMode.Impulse);
	}

	public virtual void Damaged(float val, Vector3 dir)
	{
		Debug.Log ("DD");
		base.Damaged (val, dir);
		Knockback (dir * 5);
	}

}
