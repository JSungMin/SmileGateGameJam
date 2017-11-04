using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
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

	void OnEnable ()
	{
		base.OnEnable ();
		instance = this;
		// Equipt Default Weapon
		skel.state.Event += HandleHitEvent;
		skel.state.Event += HandleStartEvent;
		skel.state.Event += HandleEndEvent;
		ChangeWeapon (0);
	}

	public void NormalAttack ()
	{
		if (acInfo.isDashing || acInfo.isAttacking)
			return;
		skel.state.ClearTrack (0);
		switch (nowWeaponInfo.weaponType)
		{
		case WeaponType.BetWeapon:
			SetAnimation (0, batAnim[animationIndex], false, 1f);
			break;
		case WeaponType.KeyBoardWeapon:
			SetAnimation (0, keyboardAnim[animationIndex], false, 1f);
			break;
		case WeaponType.MouseWeapon:
			SetAnimation (0, mouseAnim[animationIndex], false, 1f);
			break;
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
				enemy.Damaged (nowWeaponInfo.damage, (enemy.transform.position - transform.position).normalized);
			}
		}
		if (mCount != 0) {
			if (animationIndex + 1 >= 2) {
				// 2타 콤보 쳤을때
				acInfo.mp = Mathf.Min (acInfo.mp + 1, 10);
				animationIndex = 0;
			} else {
				++animationIndex;
			}
			ComboTimer.GetInstance.AddCombo (mCount);
		} else {
			animationIndex = 0;
		}
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

	void HandleStartEvent (Spine.TrackEntry entry, Spine.Event e)
	{
		if (e.Data.Name == "Start") {
			acInfo.isAttacking = true;
			Debug.Log ("SS");
		}
	}
	void HandleEndEvent (Spine.TrackEntry entry, Spine.Event e)
	{
		if (e.Data.Name == "End") {
			Debug.Log ("EE");
			acInfo.isAttacking = false;
		}
	}
	void HandleHitEvent (Spine.TrackEntry entry, Spine.Event e)
	{
		if (e.Data.Name == "hit") {
			Debug.Log ("EE");
		}
	}
}
