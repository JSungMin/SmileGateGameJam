using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public ActorInfo info;
	public Rigidbody rigid;

	protected void OnEnable ()
	{
		rigid = GetComponent<Rigidbody> ();
	}
	public virtual void Move (Vector3 dir)
	{
		rigid.MovePosition (dir * info.speed);
	}
}
