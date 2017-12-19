using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraController))]
public class CameraControllerEditor : Editor {

	CameraController		cameraController;

	void OnEnable()
	{
		cameraController = target as CameraController;
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
	}

	public void OnSceneGUI()
	{
		Rect r = cameraController.deadZone;
		Handles.color = Color.red;
		Handles.DrawAAPolyLine(new Vector3(r.xMin, 0, r.yMin), new Vector3(r.xMin, 0, r.yMax), new Vector3(r.xMax, 0, r.yMax), new Vector3(r.xMax, 0, r.yMin), new Vector3(r.xMin, 0, r.yMin));

		cameraController.deadZone.min = Handles.FreeMoveHandle(r.min, Quaternion.identity, 0.1f, Vector3.zero, Handles.DotHandleCap);
		cameraController.deadZone.max = Handles.FreeMoveHandle(r.max, Quaternion.identity, 0.1f, Vector3.zero, Handles.DotHandleCap);


		Rect wr = cameraController.worldLimit;
		 Handles.color = Color.blue;

		 Handles.DrawAAPolyLine(new Vector3(wr.xMin, 0, wr.yMin), new Vector3(wr.xMin, 0, wr.yMax), new Vector3(wr.xMax, 0, wr.yMax), new Vector3(wr.xMax, 0, wr.yMin), new Vector3(wr.xMin, 0, wr.yMin));

		// cameraController.worldLimit.min = Handles.FreeMoveHandle(wr.min, Quaternion.identity, 0.1f, Vector3.zero, Handles.DotHandleCap);
		// cameraController.worldLimit.max = Handles.FreeMoveHandle(wr.max, Quaternion.identity, 0.1f, Vector3.zero, Handles.DotHandleCap);
	}
}
