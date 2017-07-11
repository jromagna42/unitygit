using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BotController))]
public class BotControllerEditor : Editor {

	bool			debug = true;
	BotController	bot;

	public void OnEnable()
	{
		bot = target as BotController;
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		
		EditorGUI.BeginChangeCheck();
		if (GUILayout.Button("Debug"))
			debug = !debug;
		if (EditorGUI.EndChangeCheck())
			SceneView.RepaintAll();
	}

	public void OnSceneGUI()
    {
		if (debug)
		{
			Handles.DrawWireDisc(bot.transform.position, Vector3.up, bot.limit);
			Handles.color = Color.blue;
			float size = HandleUtility.GetHandleSize(bot.transform.position) * 1f;
			Handles.ArrowCap(0, bot.transform.position, bot.transform.rotation, size);
		}
    }

}
