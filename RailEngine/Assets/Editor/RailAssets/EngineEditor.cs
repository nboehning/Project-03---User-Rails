using UnityEngine;
using System.Collections;
using UnityEditor;

/*
 * @author Mike Dobson
 * */

[CustomEditor(typeof(ScriptEngine))]
public class EngineEditor :  Editor
{
	
	ScriptEngine engineScript;

	void Awake()
	{
		//engineScript = (ScriptEngine)engineScript;
	}

	public override void OnInspectorGUI()
	{

		if(GUILayout.Button("Create Waypoints"))
		{
			ScriptWaypointWindow.Init();
		}
	}
}
