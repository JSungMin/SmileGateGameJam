using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor {
	public static Player instance;
	public Player GetInstance {
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
	void OnEnable ()
	{
		instance = this;
	}
	
	// Update is call ed once per frame
	void Update () 
	{
			
	}
	public void NormalAttack ()
	{
		
	}
	public void Dash ()
	{

	}
	public void SkillA()
	{

	}
	public void SkillB()
	{

	}
}
