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
		//required things for arrays
		serializedObject.Update ();

		//-------------------------------
		//Place your custom editor stuffs
		//serializedObject.waypoints
		SerializedProperty waypointsArray = serializedObject.FindProperty ("movements");

        EditorGUILayout.PropertyField(waypointsArray);

        if (waypointsArray.isExpanded)
        {
            //EditorGUILayout.PropertyField(waypointsArray.arraySize)
            EditorGUILayout.PropertyField(waypointsArray.FindPropertyRelative("Array.size"));
            EditorGUI.indentLevel++;
            for (int i = 0; i < waypointsArray.arraySize; i++)
            {
                SerializedProperty showInEditor = waypointsArray.GetArrayElementAtIndex(i).FindPropertyRelative("showInEditor");

                showInEditor.boolValue = EditorGUILayout.Foldout(showInEditor.boolValue, "Movement " + ( i + 1 ));

                if (showInEditor.boolValue)
                {
                    //EditorGUILayout.LabelField("Movement " + (i + 1));
                    EditorGUILayout.PropertyField(waypointsArray.GetArrayElementAtIndex(i));
                }
            }
            EditorGUI.indentLevel--;
        }
        //--------------------------------


        //required things for arrays
        serializedObject.ApplyModifiedProperties();
	}
}
