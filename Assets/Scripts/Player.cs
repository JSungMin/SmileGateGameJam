using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine.Unity.Modules;
using XInputDotNetPure;

using Com.LuisPedroFonseca.ProCamera2D;

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
		"Player_bet_attack1",
		"Player_bet_attack0"
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

	public virtual void Damaged(float val, Vector3 dir)
	{
		base.Damaged (val, dir);
		Camera.main.GetComponent<ProCamera2DShake> ().Shake (0);
	}

	public void NormalAttack ()
	{
		if (acInfo.isDashing || acInfo.isAttacking)
			return;
		skel.state.ClearTrack (0);
		rigid.velocity = Vector3.zero;
		switch (nowWeaponInfo.weaponType)
		{
		case WeaponType.BetWeapon:
			SetAnimation (0, batAnim[animationIndex], false, 1.5f);
			break;
		case WeaponType.KeyBoardWeapon:
			SetAnimation (0, keyboardAnim[animationIndex], false, 1.5f);
			break;
		case WeaponType.MouseWeapon:
			SetAnimation (0, mouseAnim[animationIndex], false, 1.5f);
			break;
		}

		Vector3 center = transform.position + (int)lookDir* Vector3.right * nowWeaponInfo.reach * 0.5f;
		var hittedObjs = Physics.OverlapBox (center, Vector3.right* nowWeaponInfo.reach*0.5f + Vector3.up * bodyCollider.bounds.size.y* 0.5f + Vector3.forward * 2f, Quaternion.identity, 1 << LayerMask.NameToLayer("Enemy"));

		int mCount = 0;
		for (int i = 0; i < hittedObjs.Length; i++)
		{
			var obj = hittedObjs [i];
			Debug.Log (obj.name);
			var enemy = obj.GetComponent<Enemy> ();
			if (null != enemy)
			{
				++mCount;
				switch (nowWeaponInfo.weaponType)
				{
				case WeaponType.BetWeapon:
					var pos = enemy.bodyCollider.bounds.center + new Vector3 (Random.Range (-1, 1), Random.Range (-1, 1), -1f) + Vector3.up * 2;
					var newEffect = Instantiate (betHitEffects [Random.Range (0, betHitEffects.Length)], pos, Quaternion.identity);
					newEffect.transform.localScale = Vector3.one * 2;
					break;
				case WeaponType.KeyBoardWeapon:

					break;
				case WeaponType.MouseWeapon:

					break;
				}

				enemy.Damaged (nowWeaponInfo.damage, (enemy.transform.position - transform.position).normalized);
			}
		}
		if (mCount != 0) {
			Camera.main.GetComponent<ProCamera2DShake> ().Shake (0);
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

		Physics.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer("Enemy"),true);
	
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
		Physics.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer("Enemy"),false);
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
		if (acInfo.mp < 10)
			return;
		acInfo.mp = 0;
		Vector3 center = transform.position + (int)lookDir* Vector3.right * nowWeaponInfo.reach * 0.5f;
		var hittedObjs = Physics.OverlapBox (center, Vector3.right* nowWeaponInfo.reach*0.5f + Vector3.up * bodyCollider.bounds.size.y* 0.5f + Vector3.forward * 2f, Quaternion.identity, 1 << LayerMask.NameToLayer("Enemy"));
		Debug.Log ("Use SkillA");
		for (int i = 0; i < hittedObjs.Length; i++)
		{
			var enemy = hittedObjs [i].GetComponent<Enemy>();
			var dir = (enemy.transform.position - transform.position).normalized;
			enemy.Damaged (nowWeaponInfo.damage, (enemy.transform.position - transform.position).normalized);
			enemy.Knockback (dir * 100);
		}
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
	public GameObject[] betHitEffects;
	public GameObject[] keyboardHitEffects;
	public GameObject[] mouseHitEffects;
	void HandleHitEvent (Spine.TrackEntry entry, Spine.Event e)
	{
		if (e.Data.Name == "Hit") {

		}
	}
}
