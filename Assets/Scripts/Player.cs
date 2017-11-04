using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity.Modules;

public class Player : Actor {
	public static Player instance;
	public static Player GetInstance {
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

	public int animationIndex = 0;
	private string[] batAnim = {
		"Player_Bat_Attack01",
		"Player_Bat_Attack02",
		"Player_Bat_Attack03"
	};
	private string[] keyboardAnim = {
		"Player_Keyboard_Attack01",
		"Player_Keyboard_Attack02",
		"Player_Keyboard_Attack03"
	};
	private string[] mouseAnim = {
		"Player_Mouse_Attack01",
		"Player_Mouse_Attack02",
		"Player_Mouse_Attack03"
	};

	void OnEnable ()
	{
		base.OnEnable ();
		instance = this;
		// Equipt Default Weapon
		ChangeWeapon (0);
	}

	public void NormalAttack ()
	{
		if (acInfo.isDashing)
			return;

		if (lookDir == LookDirection.LookLeft) {
			skel.AnimationState.ClearTrack (0);
			SetAnimation (0, "Left_Attack", true, 1);
		}
		else {
			skel.AnimationState.ClearTrack (0);
			SetAnimation (0, "Right_Attack", true, 1);
		}
		Vector3 center = transform.position + (int)lookDir* Vector3.right * nowWeaponInfo.reach * 0.5f;
		var hittedObjs = Physics.OverlapBox (center,
			Vector3.right* nowWeaponInfo.reach*0.5f + Vector3.up * bodyCollider.bounds.size.y* 0.5f + Vector3.forward * 2f,
			Quaternion.identity, 1<<attackableMask);

		int mCount = 0;
		for (int i = 0; i < hittedObjs.Length; i++)
		{
			var obj = hittedObjs [i];
			var enemy = obj.GetComponent<Enemy> ();
			if (null != enemy)
			{
				++mCount;
				// TODO:Enemy damaged;
				enemy.Damaged (nowWeaponInfo.damage, (enemy.transform.position - transform.position).normalized);
				Debug.Log ("Damaged");
			}
		}
		ComboTimer.GetInstance.AddCombo (mCount);
	}
	private IEnumerator IDashing (float duration)
	{
		acInfo.isDashing = true;
		GetComponentInChildren<SkeletonGhost> ().ghostingEnabled = true;
		var dir = Vector3.right * ((int)lookDir);
		var timer = 0f;
		var curveVal = 0f;
		while (timer / duration <= 1)
		{
			timer += Time.deltaTime;
			curveVal = dashCurve.Evaluate (timer / duration);
			rigid.velocity = dir * curveVal * acInfo.dashAmount;
			yield return null;
		}
		GetComponentInChildren<SkeletonGhost> ().ghostingEnabled = false;
		var delay = 1f;
		while (delay > 0f)
		{
			delay -= Time.deltaTime;
			yield return null;
		}
		acInfo.isDashing = false;
	}
	public void Dash ()
	{
		if (!acInfo.isDashing) {
			StartCoroutine ("IDashing", 0.15f);
		}
	}
	public void SkillA()
	{

	}
	public void ChangeWeapon()
	{
		ChangeWeapon ((nowWeaponIndex + 1) % haveWeaponsInfo.Count);
	}
}
