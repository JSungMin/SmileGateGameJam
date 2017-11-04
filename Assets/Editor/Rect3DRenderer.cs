using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CustomEditor (typeof (Rect3D))]
public class Rect3DRenderer : Editor {

	private Rect3D selectedRectInfo;
	private static float buttonSize = 0.5f;

	private void OnSceneGUI ()
	{
		selectedRectInfo = target as Rect3D;

		if (null == selectedRectInfo) {
			return;
		}

		DrawRect3D (selectedRectInfo);
		selectedRectInfo.ReloadRectInfo ();
	}

	public static List <Vector3[]> DrawRectangularWithOutline (Rect3D targetRect)
	{
		List<Vector3[]> faceList = new List<Vector3[]> ();
		Vector3[] vertices = new Vector3[8];
		vertices [0] = targetRect.worldCenterPosition + Vector3.forward * targetRect.extents.z + Vector3.down * targetRect.extents.y + Vector3.left * targetRect.extents.x; // 왼쪽 아래 앞
		vertices [1] = targetRect.worldCenterPosition + Vector3.forward * targetRect.extents.z + Vector3.down * targetRect.extents.y + Vector3.right * targetRect.extents.x; // 오른쪽 아래 앞
		vertices [2] = targetRect.worldCenterPosition + Vector3.forward * targetRect.extents.z + Vector3.up * targetRect.extents.y + Vector3.right * targetRect.extents.x;
		vertices [3] = targetRect.worldCenterPosition + Vector3.forward * targetRect.extents.z + Vector3.up * targetRect.extents.y + Vector3.left * targetRect.extents.x;
		vertices [4] = targetRect.worldCenterPosition + Vector3.back * targetRect.extents.z + Vector3.up * targetRect.extents.y + Vector3.left * targetRect.extents.x;
		vertices [5] = targetRect.worldCenterPosition + Vector3.back * targetRect.extents.z + Vector3.up * targetRect.extents.y + Vector3.right * targetRect.extents.x;
		vertices [6] = targetRect.worldCenterPosition + Vector3.back * targetRect.extents.z + Vector3.down * targetRect.extents.y + Vector3.right * targetRect.extents.x;
		vertices [7] = targetRect.worldCenterPosition + Vector3.back * targetRect.extents.z + Vector3.down * targetRect.extents.y + Vector3.left * targetRect.extents.x;
	
		Vector3[] front = new Vector3[4];
		front [0] = vertices [0];
		front [1] = vertices [1];
		front [2] = vertices [2];
		front [3] = vertices [3];

		Vector3[] back = new Vector3[4];
		back [0] = vertices [4];
		back [1] = vertices [5];
		back [2] = vertices [6];
		back [3] = vertices [7];

		Vector3[] leftSide = new Vector3[4];
		leftSide [0] = vertices [0];
		leftSide [1] = vertices [3];
		leftSide [2] = vertices [4];
		leftSide [3] = vertices [7];

		Vector3[] rightSide = new Vector3[4];
		rightSide [0] = vertices [1];
		rightSide [1] = vertices [2];
		rightSide [2] = vertices [5];
		rightSide [3] = vertices [6];

		Vector3[] top = new Vector3[4];
		top [0] = vertices [2];
		top [1] = vertices [3];
		top [2] = vertices [4];
		top [3] = vertices [5];

		Vector3[] down = new Vector3[4];
		down [0] = vertices [0];
		down [1] = vertices [1];
		down [2] = vertices [6];
		down [3] = vertices [7];

		Handles.DrawSolidRectangleWithOutline (
			front,
			targetRect.renderColor,
			new Color (0.91f, 0.2901f, 0.3725f, 0.05f)
		);

		Handles.DrawSolidRectangleWithOutline (
			back,
			targetRect.renderColor,
			new Color (0.91f, 0.2901f, 0.3725f, 0.05f)
		);

		Handles.DrawSolidRectangleWithOutline (
			leftSide,
			targetRect.renderColor,
			new Color (0.91f, 0.2901f, 0.3725f, 0.05f)
		);

		Handles.DrawSolidRectangleWithOutline (
			rightSide,
			targetRect.renderColor,
			new Color (0.91f, 0.2901f, 0.3725f, 0.05f)
		);

		Handles.DrawSolidRectangleWithOutline (
			top,
			targetRect.renderColor,
			new Color (0.91f, 0.2901f, 0.3725f, 0.05f)
		);

		Handles.DrawSolidRectangleWithOutline (
			down,
			targetRect.renderColor,
			new Color (0.91f, 0.2901f, 0.3725f, 0.05f)
		);

		faceList.Add (front);
		faceList.Add (back);
		faceList.Add (rightSide);
		faceList.Add (leftSide);
		faceList.Add (top);
		faceList.Add (down);

		return faceList;
	}

	public static int selectedIndex = -1;

	private static void MakeButton (int index, Vector3 point, float size)
	{
		if (Handles.Button (point, Quaternion.identity, size * 0.04f, 0.04f, Handles.DotHandleCap)) {
			selectedIndex = index;
		}
	}

	public static void DrawRect3D (Rect3D targetRect)
	{
		var faces = DrawRectangularWithOutline (targetRect);
		Handles.color = Color.cyan;
		var colCenter = targetRect.transform.position;
		MakeButton (10, colCenter, buttonSize * 2.5f);

		if (selectedIndex == 10)
		{
			EditorGUI.BeginChangeCheck ();

			colCenter = Handles.DoPositionHandle (colCenter, Quaternion.identity);

			if (EditorGUI.EndChangeCheck ()) {
				Undo.RecordObject (targetRect, "Change Center");
				EditorUtility.SetDirty (targetRect);
				targetRect.transform.position = colCenter;
			}
		}

		for (int i = 0; i < 6; i++)
		{
			var midPoint = Vector3.zero;
			for (int j = 0; j < 4; j++)
			{
				midPoint += faces [i] [j];
			}
			midPoint /= 4f;

			Handles.color = Color.red;
			MakeButton (i, midPoint, buttonSize);

			if (selectedIndex == i) {
				EditorGUI.BeginChangeCheck ();
				midPoint = Handles.DoPositionHandle (midPoint, Quaternion.identity);

				if (EditorGUI.EndChangeCheck ())
				{
					Undo.RecordObject (targetRect, "Change Camera Rect");
					EditorUtility.SetDirty (targetRect);
					if (i == 0 || i == 1) {// Front or Back
						var tmpVec = targetRect.extents;
						tmpVec.z = Mathf.Abs (midPoint.z - (targetRect.transform.position.z + targetRect.centerPosition.z));
						targetRect.extents = tmpVec;
					} 
					else if (i == 2 || i == 3) { // right side or left side
						var tmpVec = targetRect.extents;
						tmpVec.x = Mathf.Abs (midPoint.x - (targetRect.transform.position.x + targetRect.centerPosition.x));
						targetRect.extents = tmpVec;
					} 
					else if (i == 4 || i == 5) { // top or down
						var tmpVec = targetRect.extents;
						tmpVec.y = Mathf.Abs (midPoint.y - (targetRect.transform.position.y + targetRect.centerPosition.y));
						targetRect.extents = tmpVec;
					}
				}
			}
		}
	}
}
