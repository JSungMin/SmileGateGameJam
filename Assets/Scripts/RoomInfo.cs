using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RoomInfo : MonoBehaviour {
	public Rect3D roomRect;
	public Rect3D cameraRect;

	void OnEnable ()
	{
		if (null == roomRect) {
			var newRect = new GameObject ("RoomRect");
			newRect.transform.parent = transform;
			newRect.transform.localPosition = Vector3.zero;
			roomRect = newRect.AddComponent<Rect3D> ();
		}
		if (null == cameraRect)
		{
			var newRect = new GameObject ("CameraRect");
			newRect.transform.parent = transform;
			newRect.transform.localPosition = Vector3.zero;
			cameraRect = newRect.AddComponent<Rect3D> ();
		}
	}
	 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
