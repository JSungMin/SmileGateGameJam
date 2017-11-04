using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor {
	public EnemySpawn spanwer;
    public int score;
    public float disToPlayer;
	public Material[] mats;
	public GameObject deathEffect;
	public Transform deathEffectPivot;
	public new virtual void OnEnable ()
	{
		base.OnEnable ();
		mats = skel.GetComponent<MeshRenderer> ().materials;
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

	public IEnumerator DeathRoutine (float duration)
	{
		acInfo.isDead = true;
		float timer = 0f;
		while (timer <= 0.5f * duration)
		{
			timer += Time.deltaTime;
			for (int i = 0; i < mats.Length; i++)
			{
				var mat = mats [i];
				mat.SetColor ("_Black",Color.Lerp (mat.GetColor ("_Black"), Color.white, timer / (duration * 0.5f)));
			}
			skel.GetComponent<MeshRenderer> ().materials = mats;
			yield return null;
		}
		GameObject newEffect = Instantiate (deathEffect, deathEffectPivot.position, Quaternion.identity);
		while (timer <= duration)
		{
			timer += Time.deltaTime;
			for (int i = 0; i < mats.Length; i++)
			{
				var mat = mats [i];
				mat.SetColor ("_Color",Color.Lerp (Color.white, new Color (1f,1f,1f,0f), timer / duration));
			}
			skel.GetComponent<MeshRenderer> ().materials = mats;
			yield return null;
		}
		spanwer.SetPooledObject (gameObject);
	}

	public void Idle ()
	{
		if (acInfo.isAttacking || acInfo.isDead)
			return;
		SetAnimation (0, "_idle", true, 1);
		rigid.velocity = Vector3.Lerp (rigid.velocity, Vector3.zero, 25 * Time.deltaTime);
	}

	public void Move (Vector3 dir)
	{
		if (acInfo.isAttacking || acInfo.isKnockbacking || acInfo.isDead)
			return;
		if (dir.x < 0)
		{
			lookDir = LookDirection.LookLeft;
			skel.transform.localScale = Vector3.one * 0.15f;
		}
		else if (dir.x > 0)
		{
			lookDir = LookDirection.LookRight;
			skel.transform.localScale = new Vector3 (-0.15f,0.15f,0.15f);
		}
		rigid.velocity =  (dir * acInfo.speed);
		SetAnimation (0, "_run", true, 1);
	}

	public void ReleaseKnockback ()
	{
		acInfo.isKnockbacking = false;
	}

	public void Damaged(float val, Vector3 dir)
	{
		if (acInfo.isDead)
			return;
		if (acInfo.hp -1 <= 0)
		{
			skel.state.ClearTrack (0);
			SetAnimation (0, "_death", false, 1f);
			StartCoroutine (DeathRoutine (1f));
			return;
		}
		base.Damaged (val, dir);
		acInfo.isAttacking = false;
		skel.state.ClearTrack (0);
		acInfo.isKnockbacking = true;
		Knockback (dir * 25f);
		SetAnimation (0, "_hit", false, 1f);
		Invoke ("ReleaseKnockback", 0.1f);
	}
}
