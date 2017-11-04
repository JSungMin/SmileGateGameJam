using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRotater : MonoBehaviour {
	public float rotateSpeed;
	public float moveSpeed = 10f;
	public float maxLifeTime = 5f;
	private float timer = 0f;
	public Vector3 dir;
	public float damage;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (timer >= maxLifeTime)
			DestroyProcess ();
		timer += Time.deltaTime;
		transform.Translate (dir * moveSpeed);
		transform.Rotate (Vector3.forward * rotateSpeed * Time.deltaTime);	
	}

	public void DestroyProcess ()
	{
		DestroyObject (gameObject);
	}

	public void OnCollisionEnter (Collision col)
	{
		Debug.Log (col.collider.name);
		var p = col.collider.GetComponent<Player> ();
		if (null != p) {
			p.Damaged (damage, dir);
		}
		DestroyProcess ();
	}
}
