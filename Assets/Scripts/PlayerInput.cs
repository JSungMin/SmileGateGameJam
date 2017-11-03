using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
	public Vector3 dir;
	public Player pActor;
	public bool inputA = false;
	public bool inputS = false;
	public bool inputD = false;
	public bool inputQ = false;

	// Update is called once per frame
	void Update () {
		dir.x = Input.GetAxis ("Horizontal");
		dir.y = Input.GetAxis ("Vertical");
		dir = dir.normalized;		
		if (dir != Vector3.zero)
			pActor.Move (dir);
		inputA = (Input.GetKey ("a")||Input.GetKey("joystick button 0")) ? true : false;
		inputS = (Input.GetKey ("s")||Input.GetKey("joystick button 1")) ? true : false;
		inputD = (Input.GetKey ("d")||Input.GetKey("joystick button 3")) ? true : false;
		inputQ = (Input.GetKey ("q")||Input.GetKey("joystick button 2")) ? true : false;
	}
}
