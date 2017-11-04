using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rect3D : MonoBehaviour {
	
	public string rectName;

	public float widht = 1;
	public float height = 1;
	public float depth = 1;

	public Vector3 size = Vector3.one * 2;
	public Vector3 extents = Vector3.one;

	public Vector3 centerPosition;
	public Vector3 worldCenterPosition;

	public Vector3 max = Vector3.one; // always centerposition.xyz + extents.xyz
	public Vector3 min = -Vector3.one; // always centerposition.xyz - extents.xyz

	public Color renderColor = new Color (0.5529f, 0.9921f, 0.6313f, 0.0196f);

	public Rect[] objectFace = new Rect[6];

	public Rect3D ()
	{
		centerPosition = Vector3.zero;
		this.widht = 1f;
		this.height = 1f;
		this.depth = 1f;
	}

	public Rect3D (Vector3 center, float widht, float height, float depth)
	{
		this.centerPosition = center;
		this.widht = widht;
		this.height = height;
		this.depth = depth;

		this.extents = new Vector3 (widht * 0.5f, height * 0.5f, depth * 0.5f);
		this.max = new Vector3 (centerPosition.x + extents.x, centerPosition.y + extents.y + centerPosition.z + extents.z);
		this.min = new Vector3 (centerPosition.x - extents.x, centerPosition.y - extents.y, centerPosition.z - extents.z);

		/* Set Faces
		 * Face Direction based on Look Front State
		 */
		//UpFace
		var center2D = new Vector2 (center.x, max.y);
		var size2D = new Vector2 (widht, depth);
		objectFace [0] = new Rect (center2D, size2D);
		//FrontFace
		center2D = new Vector2 (center.x, center.y);
		size2D = new Vector2 (widht, height);
		objectFace [1] = new Rect (center2D, size2D);
		//DownFace
		center2D = new Vector2 (center.x, min.y);
		size2D = new Vector2 (widht, depth);
		objectFace [2] = new Rect (center2D, size2D);
		//BackFace
		center2D = new Vector2 (center.x, center.y);
		size2D = new Vector2 (widht, height);
		objectFace [3] = new Rect (center2D, size2D);
		//LeftFace
		center2D = new Vector2 (min.x, center.y);
		size2D = new Vector2 (depth, height);
		objectFace [4] = new Rect (center2D, size2D);
		//RightFace
		center2D = new Vector2 (max.x, center.y);
		size2D = new Vector2 (depth, height);
		objectFace [5] = new Rect (center2D, size2D);
		//End Set Faces

		this.renderColor = new Color (0.5529f, 0.9921f, 0.6313f, 0.0196f);
	}
	public Rect3D (Vector3 center, float widht, float height, float depth, Color color) : this(center, widht, height, depth)
	{
		this.renderColor = color;
	}
	public Rect3D (Vector3 center, Vector3 size) : this (center, size.x, size.y, size.z)
	{
		
	}
	public Rect3D (Vector3 center, Vector3 size, Color color) : this (center, size.x, size.y, size.z, color)
	{
	}

	public void Start ()
	{
		ReloadRectInfo ();
	}

	public void ReloadRectInfo ()
	{
		this.worldCenterPosition = transform.position + centerPosition;
		this.max = new Vector3 (worldCenterPosition.x + extents.x, worldCenterPosition.y + extents.y, worldCenterPosition.z + extents.z);
		this.min = new Vector3 (transform.position.x + centerPosition.x - extents.x, transform.position.y + centerPosition.y - extents.y, transform.position.z + centerPosition.z - extents.z);
		this.widht = extents.x * 2f;
		this.height = extents.y * 2f;
		this.depth = extents.z * 2f;
		this.size = extents * 2f;


		/* Set Faces
		 * Face Direction based on Look Front State
		 */
		//UpFace
		var center2D = new Vector2 (worldCenterPosition.x, max.y);
		var size2D = new Vector2 (widht, depth);
		objectFace [0] = new Rect (center2D, size2D);
		//FrontFace
		center2D = new Vector2 (worldCenterPosition.x, worldCenterPosition.y);
		size2D = new Vector2 (widht, height);
		objectFace [1] = new Rect (center2D, size2D);
		//DownFace
		center2D = new Vector2 (worldCenterPosition.x, min.y);
		size2D = new Vector2 (widht, depth);
		objectFace [2] = new Rect (center2D, size2D);
		//BackFace
		center2D = new Vector2 (worldCenterPosition.x, worldCenterPosition.y);
		size2D = new Vector2 (widht, height);
		objectFace [3] = new Rect (center2D, size2D);
		//LeftFace
		center2D = new Vector2 (min.x, worldCenterPosition.y);
		size2D = new Vector2 (depth, height);
		objectFace [4] = new Rect (center2D, size2D);
		//RightFace
		center2D = new Vector2 (max.x, worldCenterPosition.y);
		size2D = new Vector2 (depth, height);
		objectFace [5] = new Rect (center2D, size2D);
		//End Set Faces
	}

	public bool IsContainPoint (Vector3 point)
	{
		if (point.x > max.x || point.x < min.x)
		{
			return false;
		}
		if (point.y > max.y || point.y < min.y)
		{
			return false;
		}
		if (point.z > max.z || point.z < min.z)
		{
			return false;
		}
		return true;
	}

	public Rect GetFaceRect (FaceName faceName)
	{
		switch (faceName)
		{
		case FaceName.upFace:
			return objectFace [0];
		case FaceName.frontFace:
			return objectFace [1];
		case FaceName.downFace:
			return objectFace [2];
		case FaceName.backFace:
			return objectFace [3];
		case FaceName.leftFace:
			return objectFace [4];
		}
		return objectFace [5];
	}

	public Vector3 GetNearestPoint (Vector3 point)
	{
		if (IsContainPoint (point)) {
			Vector3[] boundVector = new Vector3 [6];
			boundVector [0] = new Vector3 (max.x, point.y, point.z);
			boundVector [1] = new Vector3 (min.x, point.y, point.z);
			boundVector [2] = new Vector3 (point.x, max.y, point.z);
			boundVector [3] = new Vector3 (point.x, min.y, point.z);
			boundVector [4] = new Vector3 (point.x, point.y, max.z);
			boundVector [5] = new Vector3 (point.x, point.y, min.z);

			int index = 0;
			float minDis = Vector3.Distance (point, boundVector [0]);
			for (int i = 1; i < 6; i++) {
				var tmpDis = Vector3.Distance (point, boundVector [i]);
				if (minDis > tmpDis) {
					index = i;
					minDis = tmpDis;
				}
			}
			return boundVector [index];
		}
			
		List<FaceName> checkableList = new List<FaceName> ();
		for (int i = 0; i < 6; i++)
		{
			if (GetFaceRect ((FaceName)i).Contains(point))
			{
				checkableList.Add ((FaceName)i);
			}
		}

		if (checkableList.Count != 0)
		{
			int index;
			var resultVec = Vector3.zero;
			float minDis = -1f;

			CalculateBoundPoint (checkableList [0], point, out resultVec, out minDis);

			for (int i = 0; i < checkableList.Count; i++)
			{
				Vector3 tmpVector = Vector3.zero;
				float tmpDis = 0f;

				CalculateBoundPoint (checkableList [i], point, out tmpVector, out tmpDis);

				if (tmpDis < minDis) {
					minDis = tmpDis;
					resultVec = tmpVector;
				}
			}
			return resultVec;
		}
		return Vector3.one * -10000;
	}

	private void CalculateBoundPoint (FaceName face, Vector3 point, out Vector3 boundVector, out float distance)
	{
		switch (face) {
		case FaceName.backFace:
			boundVector = GetFaceRect (FaceName.backFace).center;
			distance = Vector3.Distance (new Vector3 (point.x, point.y, boundVector.z), point);
			return;
		case FaceName.upFace: 
			boundVector = GetFaceRect (FaceName.upFace).center;
			distance = Vector3.Distance (new Vector3 (point.x, boundVector.y, point.z), point);
			return;
		case FaceName.frontFace:
			boundVector = GetFaceRect (FaceName.frontFace).center;
			distance = Vector3.Distance (new Vector3 (point.x, point.y, boundVector.z), point);
			return;
		case FaceName.downFace:
			boundVector = GetFaceRect (FaceName.downFace).center;
			distance = Vector3.Distance (new Vector3 (point.x, boundVector.y, point.z), point);
			return;
		case FaceName.leftFace:
			boundVector = GetFaceRect (FaceName.leftFace).center;
			distance = Vector3.Distance (new Vector3 (boundVector.x, point.y, point.z), point);
			return;
		}
		boundVector = GetFaceRect (FaceName.rightFace).center;
		distance = Vector3.Distance (new Vector3 (boundVector.x, point.y, point.z), point);
	}

}
