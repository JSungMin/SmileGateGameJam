using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor {
	public static Player instance;
	public Player GetInstance {
		get
		{
			if (null == instance) 
			{
				instance = GameObject.FindObjectOfType<Player> ();
				if (null == instance)
					Debug.LogError ("Can't Find Player");
			}
			return instance;
		}
	}
	private PlayerInput input;
	public LayerMask attackableMask;
	void OnEnable ()
	{
		instance = this;
	}

	public void NormalAttack ()
	{
		Vector3 center = transform.position + transform.localScale.x * Vector3.right * weInfo.reach * 0.5f;
		var hittedObjs = Physics.OverlapBox (center, Vector3.one * weInfo.reach * 0.5f,Quaternion.identity, 1<<attackableMask);
		for (int i = 0; i < hittedObjs.Length; i++)
		{
			var obj = hittedObjs [i];
			var enemy = obj.GetComponent<Enemy> ();
			if (null != enemy)
			{
				// TODO:Enemy damaged;
				Debug.Log ("Damaged");
			}
		}
	}
	private IEnumerator IDashing ()
	{
		acInfo.isDashing = true;
		var prevPos = transform.position;
		var targetPos = prevPos + Vector3.right * ((int)lookDir) * acInfo.dashAmount;
		var timer = 0f;
		while (timer <= 1)
		{
			timer += Time.deltaTime;
			transform.position = Vector3.Lerp (prevPos, targetPos, timer);
			yield return null;
		}
		transform.position = targetPos;
		acInfo.isDashing = false;
	}
	public void Dash ()
	{
		if (!acInfo.isDashing) {
			StartCoroutine ("IDashing");
		}
	}
	public void SkillA()
	{

	}
	public void ChangeWeapon()
	{

	}
}
