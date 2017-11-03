using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
	public Vector3 dir;
	public Player pActor;
	// Update is called once per frame
	void Update () {
		dir.x = Input.GetAxis ("Horizontal");
		dir.y = Input.GetAxis ("Vertical");
		dir = dir.normalized;		
		if (dir != Vector3.zero)
			pActor.Move (dir);
	}
}
