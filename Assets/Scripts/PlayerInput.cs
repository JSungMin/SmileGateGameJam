using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
	public Rigidbody pRigid;
	public Vector3 dir;

	void OnEnable ()
	{
		if (null == pRigid)
			Debug.LogError ("Rigidbody is Null");
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		dir.x = Input.GetAxis ("Horizontal");
		dir.y = Input.GetAxis ("Vertical");
		dir = dir.normalized;


	}
}
