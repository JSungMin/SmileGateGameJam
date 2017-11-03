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
	public bool isBeatable = false;
}
[RequireComponent(typeof (Rigidbody))]
public class Actor : MonoBehaviour {
	public ActorInfo acInfo;
	public Rigidbody rigid;
    public WeaponInfo weInfo;
	public SkeletonAnimation skel;
	public LookDirection lookDir;
	public string curAnimation;

	protected void OnEnable ()
	{
		rigid = GetComponent<Rigidbody> ();
		skel = GetComponentInChildren<SkeletonAnimation> ();
	}
	public virtual void Idle ()
	{
		if (lookDir == LookDirection.LookLeft) {
			SetAnimation (0, "Left_Idle", true, 1);
		}
		else {
			SetAnimation (0, "Right_Idle", true, 1);
		}
	}
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
		transform.Translate (dir * acInfo.speed * Time.deltaTime);
		// SetAnimation (0, acInfo.name + "_Run", false, 1); 본 애니메이션 들어오면 사용
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
