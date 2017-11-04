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
	};
	private string[] mouseAnim = {
		"Player_mouse_attack0",
		"Player_mouse_attack1"
	};

	private new void OnEnable ()
	{
		base.OnEnable ();
		instance = this;
		// Equipt Default Weapon
		skel.state.Event += HandleHitEvent;
		skel.state.Event += HandleStartEvent;
		skel.state.Event += HandleEndEvent;
		ChangeWeapon (0);
	}

	void StopVibration ()
	{
		GamePad.SetVibration (0, 0f, 0f);
	}
		
	public new virtual void Damaged(float val, Vector3 dir)
	{
		base.Damaged (val, dir);
		acInfo.isAttacking = false;
		Camera.main.GetComponent<ProCamera2DShake> ().Shake (0);
		//SetAnimation (0, acInfo.name + "_hit", true, 1f);
		GamePad.SetVibration (0, 0.5f, 0.5f);
		Invoke ("StopVibration", 0.5f);
	}

	public override void Idle ()
	{
		if (acInfo.isAttacking||acInfo.isDashing)
			return;
		if (nowWeaponInfo.weaponType == WeaponType.BetWeapon) {
			SetAnimation (0, acInfo.name+"_bet_idle", true, 1);
		}
		else if (nowWeaponInfo.weaponType == WeaponType.KeyBoardWeapon){
			SetAnimation (0, acInfo.name+"_keyboard_Idle", true, 1);
		}
		rigid.velocity = Vector3.zero;
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
			SetAnimation (0, keyboardAnim[0], false, 3f);
			break;
		case WeaponType.MouseWeapon:
			SetAnimation (0, mouseAnim[animationIndex], false, 1.5f);
			break;
		}
	}
	private IEnumerator IDashing (float duration)
	{
		acInfo.isDashing = true;
		acInfo.isAttacking = false;
		GetComponentInChildren<SkeletonGhost> ().ghostingEnabled = true;
		var dir = Vector3.right * ((int)lookDir);
		var timer = 0f;
		var curveVal = 0f;

		//Physics.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer("Enemy"),true);
	
		while (timer / duration <= 1)
		{
			timer += Time.deltaTime;
			curveVal = dashCurve.Evaluate (timer / duration);
			rigid.velocity = dir * curveVal * acInfo.dashAmount;
			yield return null;
		}
		GetComponentInChildren<SkeletonGhost> ().ghostingEnabled = false;
		acInfo.isDashing = false;

		var delay = 1f;
		while (delay > 0f)
		{
			delay -= Time.deltaTime;
			yield return null;
		}
		acInfo.isAttacking = false;
		Idle ();
		//Physics.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer("Enemy"),false);
	}
	public void Dash ()
	{
		if (!acInfo.isDashing) {
			SetAnimation (0, acInfo.name + "_dash", true, 1f);
			GetComponent<AudioSource> ().Play ();
			StartCoroutine ("IDashing", 0.15f);
		}
	}
	public IEnumerator RageTick()
	{
		float timer = 10f;
		while (timer >= 0f)
		{
			timer -= Time.deltaTime;
			yield return null;
		}
		ChangeWeapon (0);
	}

	public void SkillA()
	{
		if (acInfo.mp < 10)
			return;
		acInfo.mp = 0;
		ChangeWeapon (1);
		StartCoroutine (RageTick());
	}
	public void ChangeWeapon()
	{
		//ChangeWeapon ((nowWeaponIndex + 1) % haveWeaponsInfo.Count);
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
			GamePad.SetVibration (0, 0f, 0f);
		}
	}
	public GameObject[] betHitEffects;
	public GameObject[] keyboardHitEffects;
	public GameObject[] mouseHitEffects;
	void HandleHitEvent (Spine.TrackEntry entry, Spine.Event e)
	{
		if (e.Data.Name == "Hit") {
			Vector3 center = transform.position + (int)lookDir* Vector3.right * nowWeaponInfo.reach * 0.5f;
			var hittedObjs = Physics.OverlapBox (center, Vector3.right* nowWeaponInfo.reach*0.5f + Vector3.up * bodyCollider.bounds.size.y* 0.5f + Vector3.forward * 2f, Quaternion.identity, 1 << LayerMask.NameToLayer("Enemy"));

			int mCount = 0;
			for (int i = 0; i < hittedObjs.Length; i++)
			{
				var obj = hittedObjs [i];
				var enemy = obj.GetComponent<Enemy> ();
				if (null != enemy)
				{
					if (enemy.acInfo.isDead)
						continue;
					GamePad.SetVibration (0, 0.7f, 0.7f);

					++mCount;
					var pos = enemy.bodyCollider.bounds.center + new Vector3 (Random.Range (-1, 1), Random.Range (-1, 1), -1f) + Vector3.up * 2;
					switch (nowWeaponInfo.weaponType)
					{
					case WeaponType.BetWeapon:
						var newEffect01 = Instantiate (betHitEffects [Random.Range (0, betHitEffects.Length)], pos, Quaternion.identity);
						newEffect01.transform.localScale = Vector3.one * 2;
						enemy.Damaged (nowWeaponInfo.damage, (enemy.transform.position - transform.position).normalized);
						break;
					case WeaponType.KeyBoardWeapon:
						var newEffect02 = Instantiate (keyboardHitEffects[0], pos, Quaternion.identity);
						newEffect02.transform.localScale = Vector3.one;
						enemy.Damaged (nowWeaponInfo.damage, (enemy.transform.position - transform.position).normalized * 1.5f);
						break;
					case WeaponType.MouseWeapon:

						break;
					}
				}
			}
			if (mCount != 0) {
				Camera.main.GetComponent<ProCamera2DShake> ().Shake (0);
				GamePad.SetVibration (0, 0.5f, 0.5f);
				if (animationIndex + 1 >= 2) {
					// 2타 콤보 쳤을때
					if (nowWeaponInfo.weaponType != WeaponType.KeyBoardWeapon) {
						acInfo.mp = Mathf.Min (acInfo.mp + 1, 10);
						animationIndex = 0;
					}
				} else {
					++animationIndex;
				}
				ComboTimer.GetInstance.AddCombo (mCount);
			} else {
				animationIndex = 0;
			}
		}
	}
}
