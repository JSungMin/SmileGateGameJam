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
}
[RequireComponent(typeof (Rigidbody))]
public class Actor : MonoBehaviour {
	public ActorInfo acInfo;
	public Rigidbody rigid;
    public WeaponInfo weInfo;
	public SkeletonAnimation skel;
	public string curAnimation;

	protected void OnEnable ()
	{
		rigid = GetComponent<Rigidbody> ();
		skel = GetComponentInChildren<SkeletonAnimation> ();
	}
	public virtual void Move (Vector3 dir)
	{
		transform.Translate (dir * acInfo.speed * Time.deltaTime);
	}
	void SetAnimation(int index, string name, bool loop, float time)
	{
		if(curAnimation == name)
		{
			return;
		}else
		{
			skel.state.SetAnimation(index, name, loop).TimeScale = time;
			curAnimation = name;
		}
	}
}
