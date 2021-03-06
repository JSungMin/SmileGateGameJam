﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
	public static PlayerInput instance;
	public static PlayerInput GetInstance
	{
		get{
			if (null == instance) 
			{
				instance = GameObject.FindObjectOfType<PlayerInput> ();
				if (null == instance) {
					Debug.LogError ("Can't Find PlayerInput");
				}
			}
			return instance;
		}
	}
	public Vector3 dir;
	public Player pActor;
	public bool inputA = false;
	public bool inputS = false;
	public bool inputD = false;
	public bool inputQ = false;


	public void OnEnable ()
	{
		instance = this;
	}

	// Update is called once per frame
	void Update () {
		dir.x = Input.GetAxis ("Horizontal");
		dir.y = Input.GetAxis ("Vertical");
		dir = dir.normalized;		
		if (dir != Vector3.zero)
			pActor.Move (dir);
		else
		{
			pActor.Idle ();
		}
		inputA = (Input.GetKeyDown ("a")||Input.GetKeyDown("joystick button 0")) ? true : false;
		inputS = (Input.GetKeyDown ("s")||Input.GetKeyDown("joystick button 1")) ? true : false;
		inputD = (Input.GetKey ("d")||Input.GetKey("joystick button 2")) ? true : false;
		inputQ = (Input.GetKeyDown ("q")||Input.GetKeyDown("joystick button 3")) ? true : false;

		if (Input.GetKeyDown(KeyCode.Space))
		{
			pActor.acInfo.hp = pActor.acInfo.InitHp;
			pActor.acInfo.mp = 10;
		}

		if (inputA)
		{
			pActor.NormalAttack ();
		}
		if (inputD)
		{
			pActor.SkillA ();
		}
		if (inputQ)
		{
			pActor.ChangeWeapon ();
		}
		if (inputS)
		{
			pActor.Dash ();
		}
	}
}
