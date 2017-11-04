using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

[System.Serializable]
public class ActorInfo
{
	public string name;
	public float hp;
	public float InitHp;
	public float mp;
	public float speed;
	public float dashAmount;
	public bool isAttacking = false;
	public bool isDashing = false;
	public bool isKnockbacking = false;
	public bool isDead = false;
	public bool isBeatable = true;

	public void ResetFlags()
	{
		isAttacking = false;
		isDashing = false;
		isKnockbacking = false;
		isDead = false;
		isBeatable = true;
		hp = InitHp;
		mp = 0;
	}
}
public delegate void InteractFunc ();
[RequireComponent(typeof (Rigidbody))]
public class Actor : MonoBehaviour {
	public ActorInfo acInfo;
	public RoomInfo nowRoomInfo;
	public Rigidbody rigid;
	public BoxCollider bodyCollider;
    public WeaponInfo nowWeaponInfo;
	public List<WeaponInfo> haveWeaponsInfo = new List<WeaponInfo>();
	public int nowWeaponIndex = 0;
	public SkeletonAnimation skel;
	public LookDirection lookDir;
	public string curAnimation;
	public AnimationCurve dashCurve;

	protected void OnEnable ()
	{
		acInfo.InitHp = acInfo.hp;
		rigid = GetComponent<Rigidbody> ();
		skel = GetComponentInChildren<SkeletonAnimation> ();
		bodyCollider = GetComponent<BoxCollider> ();
	}
	public event InteractFunc OnDamaged;
	public GameObject damagedEffect;
	public virtual void Damaged(float val, Vector3 dir)
	{
		if (!acInfo.isBeatable)
			return;
		acInfo.hp -= val;
		for (int i = 0; i < 3; i++)
		{
			var newEffect = Instantiate (damagedEffect, bodyCollider.bounds.center + new Vector3(Random.Range(-1f, 1f),Random.Range(-1f,1f)) + Vector3.up * 2, Quaternion.identity);
			newEffect.transform.localScale = Vector3.one * 2;
		}
		if (null != OnDamaged)
		{
			OnDamaged.Invoke();
		}
	}

	public virtual void Idle ()
	{
		if (acInfo.isAttacking||acInfo.isDashing)
			return;
		if (nowWeaponInfo.weaponType == WeaponType.BetWeapon) {
			SetAnimation (0, acInfo.name+"_bet_idle", true, 1);
		}
		else if (nowWeaponInfo.weaponType == WeaponType.KeyBoardWeapon){
			SetAnimation (0, acInfo.name+"_keyboard_idle", true, 1);
		}

		rigid.velocity = Vector3.zero;
	}
	// dir 넣기전에 normalize 시킬 것
	public virtual void Move (Vector3 dir)
	{
		if (acInfo.isAttacking || acInfo.isDashing)
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
		if (nowWeaponInfo.weaponType == WeaponType.BetWeapon) {
			SetAnimation (0, acInfo.name+"_bet_run", true, 1);
		}
		else if (nowWeaponInfo.weaponType == WeaponType.KeyBoardWeapon){
			SetAnimation (0, acInfo.name+"_keyboard_run", true, 1);
		}
	}

	public void ChangeWeapon (int index)
	{
		if (index < 0 || index >= haveWeaponsInfo.Count)
			return;
		nowWeaponIndex = index;
		nowWeaponInfo = haveWeaponsInfo [index];
	}
	public void SetAnimation(int index, string name, bool loop, float time)
	{
		if(curAnimation == name)
		{
			return;
		}
		else
		{
			skel.state.SetAnimation(index, name, loop).TimeScale = time;
			curAnimation = name;
		}
	}
	public void Update ()
	{
		skel.GetComponent<MeshRenderer> ().sortingOrder = -(int)transform.position.y;
	}
}
