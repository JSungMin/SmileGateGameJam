using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamearMoving : MonoBehaviour {
	public Rect3D targetArea;
	public Vector3 offset;
	public Transform followTarget;

	// Use this for initialization
	void Start () {
		if (null == followTarget)
			followTarget = Player.GetInstance.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
