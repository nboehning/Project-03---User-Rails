using UnityEngine;
using System.Collections;
using UnityEditor;

public class ScriptWaypointWindow : EditorWindow
{
    private Vector2 scrollPos1;
    private Vector2 scrollPos2;
    private Vector2 scrollPos3;

    GameObject player;

    private ScriptEngine engineScript;
    private Color oldColor;
    private bool [] dataFoldouts;


    public static void Init()
    {
        ScriptWaypointWindow window = (ScriptWaypointWindow) EditorWindow.GetWindow(typeof (ScriptWaypointWindow));
        window.position = new Rect(500, 500, 931, 510);
        window.maxSize = new Vector2(931, 510);
        window.minSize = window.maxSize;
        window.Show();
    }

    void OnFocus()
    {

        player = GameObject.Find("Player");
        engineScript = player.GetComponent<ScriptEngine>();
        dataFoldouts = new bool[engineScript.movements.Length];
        
    }

    void OnLostFocus()
    {

    }

    void OnGUI()
    {

        // Begin area for all of the waypoint types
        EditorGUILayout.BeginHorizontal();

        // Create a vertical area for movement waypoints
        EditorGUILayout.BeginVertical("box");

        // Title of the vertical area
        int oldFontSize = GUI.skin.label.fontSize;
        GUI.skin.label.fontSize = 20;
        EditorGUILayout.LabelField("Movement Waypoints", EditorStyles.boldLabel);
        GUI.skin.label.fontSize = oldFontSize;
        // Create a scroll area for the movement waypoints array
        scrollPos1 = EditorGUILayout.BeginScrollView(scrollPos1, GUILayout.Width(300), GUILayout.Height(430));

        // Loop through the array.
        for (int i = 0; i < engineScript.movements.Length; i++)
        {
            // Create a label showing which waypoint it is
            EditorGUILayout.LabelField("Waypoint " + (i + 1), EditorStyles.boldLabel);

            // Create an enum for the movement types
            engineScript.movements[i].moveType = (MovementTypes)EditorGUILayout.EnumPopup(engineScript.movements[i].moveType);

            EditorGUI.indentLevel++;
            dataFoldouts[i] = EditorGUILayout.Foldout(dataFoldouts[i],
                string.Format("Show Movement Waypoint {0} Data", i + 1));

            if (dataFoldouts[i])
            {
                EditorGUI.indentLevel++;
                switch (engineScript.movements[i].moveType)
                {
                    case MovementTypes.BEZIER:
                        engineScript.movements[i].movementTime = EditorGUILayout.FloatField("Movement Time: ",
                            engineScript.movements[i].movementTime);

                        if (engineScript.movements[i].movementTime < 0)
                            engineScript.movements[i].movementTime = 0;

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("End Point: ", GUILayout.Width(131));

                        engineScript.movements[i].endWaypoint = (GameObject)
                            EditorGUILayout.ObjectField(engineScript.movements[i].endWaypoint, typeof (GameObject), true,
                                GUILayout.Width(141));

                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("Curve Point: ", GUILayout.Width(131));

                        engineScript.movements[i].curveWaypoint = (GameObject)
                            EditorGUILayout.ObjectField(engineScript.movements[i].endWaypoint, typeof (GameObject), true,
                                GUILayout.Width(141));

                        EditorGUILayout.EndHorizontal();
                        break;
                    case MovementTypes.MOVE:
                        engineScript.movements[i].movementTime = EditorGUILayout.FloatField("Movement Time: ",
                            engineScript.movements[i].movementTime);

                        if (engineScript.movements[i].movementTime < 0)
                            engineScript.movements[i].movementTime = 0;

                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("End Point: ", GUILayout.Width(131));

                        engineScript.movements[i].endWaypoint = (GameObject)
                            EditorGUILayout.ObjectField(engineScript.movements[i].endWaypoint, typeof (GameObject), true,
                                GUILayout.Width(141));

                        EditorGUILayout.EndHorizontal();
                        break;
                    case MovementTypes.WAIT:
                        engineScript.movements[i].movementTime = EditorGUILayout.FloatField("Wait Time: ",
                            engineScript.movements[i].movementTime);

                        if (engineScript.movements[i].movementTime < 0)
                            engineScript.movements[i].movementTime = 0;
                        break;
                }
                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;









            /*
            EditorGUI.indentLevel++;
            // Create a button that'll open another window to show the specific data
            if (GUILayout.Button(engineScript.movements[i].moveType + " Components", GUILayout.Width(150), GUILayout.Height(20)))
            {
                Debug.Log("haha");
            }
            EditorGUI.indentLevel--;
            */
        }

        // End the scroll view
        EditorGUILayout.EndScrollView();

        // Create a button that adds a new waypoint to the movement array
        if (GUILayout.Button("Add New Movement Waypoint"))
        {
            Debug.Log("Creates a new movement waypoint");
        }

        // End of the movement waypoints vertical area
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Facing Waypoints", EditorStyles.boldLabel);
        scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2, GUILayout.Width(300), GUILayout.Height(430));

        EditorGUILayout.LabelField("Facing");

        EditorGUILayout.EndScrollView();

        // Create a button that adds a new waypoint to the movement array
        if (GUILayout.Button("Add New Facing Waypoint"))
        {
            Debug.Log("Creates a new facing waypoint");
        }

        EditorGUILayout.EndVertical();

        // Create area for the effects
        EditorGUILayout.BeginVertical("box");
        // Create a title showing the type of waypoints that are being listed in this area
        EditorGUILayout.LabelField("Effect Waypoints", EditorStyles.boldLabel);

        // Create a scroll view that allows designer to have an infinitely long list of waypoints
        scrollPos3 = EditorGUILayout.BeginScrollView(scrollPos3, GUILayout.Width(300), GUILayout.Height(430));
        EditorGUILayout.LabelField("Effects");

        // End of scroll view
        EditorGUILayout.EndScrollView();

        // Create a button that adds a new waypoint to the movement array
        if (GUILayout.Button("Add New Effect Waypoint"))
        {
            Debug.Log("Creates a new effect waypoint");
        }

        // End the area for effects
        EditorGUILayout.EndVertical();

        // End of waypoints area.
        EditorGUILayout.EndHorizontal();

        // Begin area for the export data button.
        GUILayout.BeginHorizontal();
        GUILayout.BeginArea(new Rect((Screen.width / 3) * 2 + 200, position.height - 27, 150, 50));
            oldColor = GUI.color;
            GUI.color = Color.green;
            if (GUILayout.Button("Export Data", GUILayout.Width(100), GUILayout.Height(20)))
            {
                Debug.Log("Export data button pushed");
            }
            GUI.color = oldColor;
        GUILayout.EndArea();
        GUILayout.EndHorizontal();

        /*
        EditorGUILayout.BeginHorizontal();
        oldColor = GUI.color;
        GUI.color = Color.green;
        if (GUILayout.Button("Export Data", GUILayout.Width(100), GUILayout.Height(20)))
        {
            Debug.Log("Export data button pushed");
        }
        GUI.color = oldColor;
        EditorGUILayout.EndHorizontal();

        Rect FacingAreaRect = new Rect(position.width / 3, 50, position.width / 3, position.height - 50);
        Rect MovementAreaRect = new Rect(0, 50, position.width / 3, position.height - 50);
        Rect EffectAreaRect = new Rect((position.width / 3) * 2, 50, position.width / 3, position.height - 50);

        EditorGUI.BeginProperty(MovementAreaRect, GUIContent.none, SerializedProperty moveScript);

        GUILayout.Label("Movement Waypoints", EditorStyles.boldLabel);

        EditorGUI.EndProperty();

        EditorGUI.BeginProperty(FacingAreaRect, GUIContent.none);

        GUILayout.Label("Facing Waypoints", EditorStyles.boldLabel);

        EditorGUI.EndProperty();

        EditorGUI.BeginProperty(EffectAreaRect, GUIContent.none);

        GUILayout.Label("Effect Waypoints", EditorStyles.boldLabel);

        EditorGUI.EndProperty();
        */
    }

}