using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

[System.Serializable]
public class ActorInfo
{
	public string name;
	public float hp;
	public float mp;
	public float speed;
	public float dashAmount;
	public bool isDashing = false;
	public bool isBeatable = true;
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
		rigid = GetComponent<Rigidbody> ();
		skel = GetComponentInChildren<SkeletonAnimation> ();
		bodyCollider = GetComponent<BoxCollider> ();
	}
	public event InteractFunc OnDamaged;

	public virtual void Damaged(float val, Vector3 dir)
	{
		if (!acInfo.isBeatable)
			return;
		acInfo.hp -= val;
		if (null != OnDamaged)
		{
			OnDamaged.Invoke();
		}
	}

	public virtual void Idle ()
	{
		if (lookDir == LookDirection.LookLeft) {
			SetAnimation (0, "Left_Idle", true, 1);
		}
		else {
			SetAnimation (0, "Right_Idle", true, 1);
		}
		rigid.velocity = Vector3.zero;
	}
	// dir 넣기전에 normalize 시킬 것
	public virtual void Move (Vector3 dir)
	{
		if (dir.x > 0)
		{
			lookDir = LookDirection.LookRight;
			SetAnimation (0, "Right_Run", true, 1);
		}
		else if (dir.x < 0)
		{
			lookDir = LookDirection.LookLeft;
			SetAnimation (0, "Left_Run", true, 1);
		}
		if (lookDir == LookDirection.LookLeft) {
			SetAnimation (0, "Left_Run", true, 1);
		} 
		else {
			SetAnimation (0, "Right_Run", true, 1);
		}
		dir.z = dir.y;
		dir.y = 0;
		rigid.velocity =  (dir * acInfo.speed);
		// SetAnimation (0, acInfo.name + "_Run", false, 1); 본 애니메이션 들어오면 사용
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
}
