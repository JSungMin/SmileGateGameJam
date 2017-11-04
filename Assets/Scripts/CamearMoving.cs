using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class CamearMoving : MonoBehaviour {
	private Camera cam;
	public Actor followTarget;
	private RoomInfo nowFocusedRoom;
	[Header("Camera Setting")]
	public Vector3 offset;
	public float followSpeed;
	public float bobAmount;
	public float maxDistance = 2;

	private float xRatio;
	private float yRatio;
	private float zRatio;
	private float disToPlayer;
	void OnEnable()
	{
		cam = GetComponent<Camera> ();
	}

	// Use this for initialization
	void Start ()
	{
		if (null == followTarget)
			followTarget = Player.GetInstance;
		nowFocusedRoom = followTarget.nowRoomInfo;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (null == followTarget ||
			null == followTarget.nowRoomInfo.cameraRect ||
			null == followTarget.nowRoomInfo.roomRect)
			return;
		var pPos = new Vector3 (followTarget.transform.position.x,
			followTarget.transform.position.y,
			0
		);
		var cPos = new Vector3 (transform.position.x,
			transform.position.y,
			0
		);
		disToPlayer = Vector3.Distance (pPos, cPos);
		LockToCameraRect ();
	}

	void LockToCameraRect ()
	{
		xRatio = (Mathf.Abs (followTarget.bodyCollider.bounds.center.x + offset.x * (int)followTarget.lookDir - nowFocusedRoom.roomRect.min.x)) / nowFocusedRoom.roomRect.size.x;
		yRatio = (Mathf.Abs (followTarget.bodyCollider.bounds.center.y + offset.y - nowFocusedRoom.roomRect.min.y)) / nowFocusedRoom.roomRect.size.y;
		zRatio = (Mathf.Abs (followTarget.bodyCollider.bounds.center.z + offset.z - nowFocusedRoom.roomRect.min.z)) / nowFocusedRoom.roomRect.size.z;

		var tmpPos = transform.position;

		tmpPos.x = Mathf.Lerp (nowFocusedRoom.cameraRect.min.x, nowFocusedRoom.cameraRect.max.x, xRatio);
		tmpPos.y = Mathf.Lerp (nowFocusedRoom.cameraRect.min.y, nowFocusedRoom.cameraRect.max.y, yRatio);
		tmpPos.z = Mathf.Lerp (nowFocusedRoom.cameraRect.min.z, nowFocusedRoom.cameraRect.max.z, zRatio);

		transform.position = Vector3.Lerp (transform.position, tmpPos, followSpeed * Time.deltaTime);

	}
}
