using UnityEngine;
using UnityEditor;

/// <summary>
/// Create a custom window for designer to input waypoints.
/// @author Nathan Boehning
/// </summary>
public class ScriptWaypointWindow : EditorWindow
{
    private Vector2 scrollPos1;
    private Vector2 scrollPos2;
    private Vector2 scrollPos3;

    GameObject player;

    private ScriptEngine engineScript;
    private Color oldColor;

    public static void Init()
    {
        // Set window size and show the window.
        ScriptWaypointWindow window = (ScriptWaypointWindow) GetWindow(typeof (ScriptWaypointWindow));
        window.position = new Rect(100, 100, 931, 510);
        window.maxSize = new Vector2(931, 510);
        window.minSize = window.maxSize;
        window.Show();
    }

    void OnFocus()
    {
        // Get the information needed
        player = GameObject.Find("Player");
        engineScript = player.GetComponent<ScriptEngine>();
        
        // Set the length of the boolean foldouts based on list length
    }

    void OnGUI()
    {
        #region Waypoints

        // Begin area for all of the waypoint types
        EditorGUILayout.BeginHorizontal();

        #region Movement
        // Create a vertical area for movement waypoints
        EditorGUILayout.BeginVertical("box");

        // Title of the vertical area
        EditorGUILayout.LabelField("Movement Waypoints", EditorStyles.boldLabel);

        // Create a scroll area for the movement waypoints array
        scrollPos1 = EditorGUILayout.BeginScrollView(scrollPos1, GUILayout.Width(300), GUILayout.Height(430));

        // Loop through the array.
        for (int i = 0; i < engineScript.movements.Count; i++)
        {
            // Create some space for ease of reading
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            // Create a section showing label information along with move up and down
            EditorGUILayout.BeginHorizontal();

            // Formatting for if point is at beginning or end of list
            if (!CanMoveUp(i) || !CanMovementMoveDown(i))
            {
                EditorGUILayout.LabelField("Waypoint " + (i + 1), EditorStyles.boldLabel, GUILayout.Width(218));
            }
            else
            {
                EditorGUILayout.LabelField("Waypoint " + (i + 1), EditorStyles.boldLabel, GUILayout.Width(190));
            }

            // Check to see if element can move up in the array
            if (CanMoveUp(i))
            {
                // If it can, create a button that allows designer to move the element upwards
                if (GUILayout.Button("/\\", GUILayout.Width(25f)))
                {
                    SwapMovements(i, i - 1);
                }
            }

            // Check to see if element can move down in the array
            if (CanMovementMoveDown(i))
            {
                // If it can, create a button that allows designer to move element downwards
                if (GUILayout.Button("\\/", GUILayout.Width(25f)))
                {
                    SwapMovements(i, i + 1);
                }
            }

            oldColor = GUI.color;
            GUI.color = Color.red;

            // Create a red button that removes the element from the array of waypoints
            if (GUILayout.Button("X", GUILayout.Width(25f)))
            {
                
                engineScript.movements.RemoveAt(i);
                return;

            }

            // Revert the GUI color to previous color before red button
            GUI.color = oldColor;

            EditorGUILayout.EndHorizontal();

            // Create an enum for the movement types
            engineScript.movements[i].moveType = (MovementTypes)EditorGUILayout.EnumPopup(engineScript.movements[i].moveType);

            EditorGUI.indentLevel++;
            // Create a foldout that shows all of the waypoint specific variables
            engineScript.movements[i].dataFoldout = EditorGUILayout.Foldout(engineScript.movements[i].dataFoldout,
                string.Format("Show Movement Waypoint {0} Data", i + 1));

            // If the foldout is open
            if (engineScript.movements[i].dataFoldout)
            {
                EditorGUI.indentLevel++;
                // Switch between the different move types
                switch (engineScript.movements[i].moveType)
                {
                    // It's a bezier curve
                    case MovementTypes.BEZIER:
                        // Get the movement time of the waypoint
                        engineScript.movements[i].movementTime = EditorGUILayout.FloatField(new GUIContent("Move Time: ", "Time spent moving in bezier curve"),
                            engineScript.movements[i].movementTime);

                        if (engineScript.movements[i].movementTime < 0)
                            engineScript.movements[i].movementTime = 0;

                        // Section to get the end point of the bezier curve
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(new GUIContent("End Point: ", "Position the player ends their movement"), GUILayout.Width(117));

                        engineScript.movements[i].endWaypoint = (GameObject)
                            EditorGUILayout.ObjectField(engineScript.movements[i].endWaypoint, typeof (GameObject), true,
                                GUILayout.Width(156));

                        EditorGUILayout.EndHorizontal();

                        // Section to get the curve point of the bezier curve
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(new GUIContent("Curver Point: ", "Point that the curve is based off of"), GUILayout.Width(117));

                        engineScript.movements[i].curveWaypoint = (GameObject)
                            EditorGUILayout.ObjectField(engineScript.movements[i].endWaypoint, typeof (GameObject), true,
                                GUILayout.Width(156));

                        // End of "property drawer" section
                        EditorGUILayout.EndHorizontal();
                        break;

                    // It's a move (straight line)
                    case MovementTypes.MOVE:
                        // Get the move time
                        engineScript.movements[i].movementTime = EditorGUILayout.FloatField(new GUIContent("Move Time: ", "Time spent moving in straight line"),
                            engineScript.movements[i].movementTime);

                        // make sure there can't be a negative time
                        if (engineScript.movements[i].movementTime < 0)
                            engineScript.movements[i].movementTime = 0;

                        // Section to get the end point of the straight line
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(new GUIContent("End Point: ", "Position the player ends their movement"), GUILayout.Width(117));

                        engineScript.movements[i].endWaypoint = (GameObject)
                            EditorGUILayout.ObjectField(engineScript.movements[i].endWaypoint, typeof (GameObject), true,
                                GUILayout.Width(156));

                        // End of "property drawer"
                        EditorGUILayout.EndHorizontal();                      
                        break;
                    // It's a wait
                    case MovementTypes.WAIT:
                        // Get the amount of time the player will wait here as "movement" time.
                        engineScript.movements[i].movementTime = EditorGUILayout.FloatField(new GUIContent("Wait Time: ", "Time spent waiting"),
                            engineScript.movements[i].movementTime);

                        if (engineScript.movements[i].movementTime < 0)
                            engineScript.movements[i].movementTime = 0;
                        break;
                }
                // Decrement the indent level
                EditorGUI.indentLevel--;
            }
            // Decrement the indent level
            EditorGUI.indentLevel--;
        }

        // End the scroll view
        EditorGUILayout.EndScrollView();

        // Create a button that adds a new waypoint to the movement array
        if (GUILayout.Button("Add New Movement Waypoint"))
        {
            ScriptMovements tempType = new ScriptMovements
            {
                moveType = MovementTypes.WAIT,
                movementTime = 0.1f
            };
            engineScript.movements.Add(tempType);
        }

        // End of the movement waypoints vertical area
        EditorGUILayout.EndVertical();
        #endregion

        #region Facings
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Facing Waypoints", EditorStyles.boldLabel);
        scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2, GUILayout.Width(300), GUILayout.Height(430));
        
        // Loop through the array.
        for (int i = 0; i < engineScript.facings.Count; i++)
        {
            // Create some space for ease of reading
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            // Create a section showing label information along with move up and down
            EditorGUILayout.BeginHorizontal();

            // Formatting for if point is at beginning or end of list
            if (!CanMoveUp(i) || !CanFacingMoveDown(i))
            {
                EditorGUILayout.LabelField("Waypoint " + (i + 1), EditorStyles.boldLabel, GUILayout.Width(218));
            }
            else
            {
                EditorGUILayout.LabelField("Waypoint " + (i + 1), EditorStyles.boldLabel, GUILayout.Width(190));
            }

            // Check to see if element can move up in the array
            if (CanMoveUp(i))
            {
                // If it can, create a button that allows designer to move the element upwards
                if (GUILayout.Button("/\\", GUILayout.Width(25f)))
                {
                    SwapFacings(i, i - 1);
                }
            }

            // Check to see if element can move down in the array
            if (CanFacingMoveDown(i))
            {
                // If it can, create a button that allows designer to move element downwards
                if (GUILayout.Button("\\/", GUILayout.Width(25f)))
                {
                    SwapFacings(i, i + 1);
                }
            }

            oldColor = GUI.color;
            GUI.color = Color.red;

            // Create a red button that removes the element from the array of waypoints
            if (GUILayout.Button("X", GUILayout.Width(25f)))
            {

                engineScript.facings.RemoveAt(i);
                return;

            }

            // Revert the GUI color to previous color before red button
            GUI.color = oldColor;

            EditorGUILayout.EndHorizontal();

            // Create an enum for the movement types
            engineScript.facings[i].facingType = (FacingTypes)EditorGUILayout.EnumPopup(engineScript.facings[i].facingType);

            EditorGUI.indentLevel++;
            // Create a foldout that shows all of the waypoint specific variables
            engineScript.facings[i].dataFoldout = EditorGUILayout.Foldout(engineScript.facings[i].dataFoldout,
                string.Format("Show Facing Waypoint {0} Data", i + 1));

            // If the foldout is open
            if (engineScript.facings[i].dataFoldout)
            {
                EditorGUI.indentLevel++;
                // Switch between the different move types
                switch (engineScript.facings[i].facingType)
                {
                    // It's a free look
                    case FacingTypes.FREELOOK:

                        // Get the facing time of the waypoint
                        engineScript.facings[i].facingTime = EditorGUILayout.FloatField(new GUIContent("Facing Time: ", "Time spent in free look"),
                            engineScript.facings[i].facingTime);

                        // Facing time can't go less than zero
                        if (engineScript.facings[i].facingTime < 0)
                            engineScript.facings[i].facingTime = 0;

                        // End of "property drawer"
                        break;

                    // It's a look at
                    case FacingTypes.LOOKAT:

                        engineScript.facings[i].lockTimes = new float[1];
                        engineScript.facings[i].targets = new GameObject[1];

                        // Get the rotate time of the waypoint
                        engineScript.facings[i].lockTimes[0] = EditorGUILayout.FloatField(new GUIContent("Focus Time: ", "Time to rotate to look at point"),
                            engineScript.facings[i].lockTimes[0]);

                        // Rotate time can't go less than zero
                        if (engineScript.facings[i].lockTimes[0] < 0)
                            engineScript.facings[i].lockTimes[0] = 0;


                        // Look point game object setter
                        EditorGUILayout.BeginHorizontal();

                        EditorGUILayout.LabelField(new GUIContent("Focus Point: ", "Location that the camera will look at"), GUILayout.Width(100));

                        engineScript.facings[i].targets[0] = (GameObject)
                            EditorGUILayout.ObjectField(engineScript.facings[i].targets[0], typeof(GameObject), true,
                                GUILayout.Width(173));

                        EditorGUILayout.EndHorizontal();

                        // Get the facing time of the waypoint
                        engineScript.facings[i].facingTime = EditorGUILayout.FloatField(new GUIContent("Facing Time: ", "Time spent looking at point"),
                            engineScript.facings[i].facingTime);

                        // Facing time can't go less than zero
                        if (engineScript.facings[i].facingTime < 0)
                            engineScript.facings[i].facingTime = 0;

                        // End of "property drawer"
                        break;

                    // It's a look chain
                    case FacingTypes.LOOKCHAIN:

                        // Get and set the length of the look chain, make sure it can't go below zero
                        engineScript.facings[i].chainCount = EditorGUILayout.IntField(new GUIContent("Chain Length: ", "Number of points the camera will look at"),
                            engineScript.facings[i].chainCount);

                        if (engineScript.facings[i].chainCount < 0)
                            engineScript.facings[i].chainCount = 0;

                        // Set the lock times and targets arrays with the new lengths
                        GameObject[] tempTargets = engineScript.facings[i].targets;
                        float[] tempLockTimes = engineScript.facings[i].lockTimes;
                        float[] tempRotationSpeed = engineScript.facings[i].rotationSpeed;

                        // Set the lock times and targets arrays with the new lengths
                        engineScript.facings[i].targets = new GameObject[engineScript.facings[i].chainCount];
                        engineScript.facings[i].lockTimes = new float[engineScript.facings[i].chainCount];
                        engineScript.facings[i].rotationSpeed = new float[engineScript.facings[i].chainCount];

                        for (int k = 0; k < engineScript.facings[i].chainCount; k++)
                        {
                            engineScript.facings[i].targets[k] = tempTargets[k];
                            engineScript.facings[i].lockTimes[k] = tempLockTimes[k];
                            engineScript.facings[i].rotationSpeed[k] = tempRotationSpeed[k];

                        }
                        
                        EditorGUI.indentLevel++;

                        // Create a foldout to show all of the elements
                        engineScript.facings[i].isFoldedOut =
                            EditorGUILayout.Foldout(engineScript.facings[i].isFoldedOut, "Show Chain Elements");

                        // If they have the foldout open
                        if (engineScript.facings[i].isFoldedOut)
                        {

                            // Loop through and show all of the elements of the look chain
                            for (int j = 0; j < engineScript.facings[i].targets.Length; j++)
                            {

                                // Look point game object setter
                                EditorGUILayout.BeginHorizontal();

                                EditorGUILayout.LabelField("Point " + (j + 1), GUILayout.Width(100));

                                engineScript.facings[i].targets[j] = (GameObject)
                                    EditorGUILayout.ObjectField(engineScript.facings[i].targets[j], typeof(GameObject), true,
                                        GUILayout.Width(173));

                                EditorGUILayout.EndHorizontal();

                                EditorGUI.indentLevel++;
                                // Show the array elements
                                engineScript.facings[i].rotationSpeed[j] = EditorGUILayout.FloatField(new GUIContent("Rotation Time", "Time to rotate to point"),
                                    engineScript.facings[i].rotationSpeed[j]);

                                // Facing time can't go less than zero
                                if (engineScript.facings[i].rotationSpeed[j] < 0)
                                    engineScript.facings[i].rotationSpeed[j] = 0;

                                // Show the array elements
                                engineScript.facings[i].lockTimes[j] = EditorGUILayout.FloatField(new GUIContent("Look Time ", "Time to look at point"),
                                    engineScript.facings[i].lockTimes[j]);

                                // Facing time can't go less than zero
                                if (engineScript.facings[i].lockTimes[j] < 0)
                                    engineScript.facings[i].lockTimes[j] = 0;

                                EditorGUI.indentLevel--;
                                // Create spaces for ease of reading
                                EditorGUILayout.Space();
                                EditorGUILayout.Space();
                            }
                        }

                        // Decrement the indent level
                        EditorGUI.indentLevel--;

                        // End of "property drawer"
                        break;

                    case FacingTypes.LOOKANDRETURN:

                        // Set the lock times and targets arrays with the new lengths
                        tempTargets = engineScript.facings[i].targets;
                        tempLockTimes = engineScript.facings[i].lockTimes;
                        tempRotationSpeed = engineScript.facings[i].rotationSpeed;

                        // Set the lock times and targets arrays with the new lengths
                        engineScript.facings[i].targets = new GameObject[1];
                        engineScript.facings[i].lockTimes = new float[1];
                        engineScript.facings[i].rotationSpeed = new float[2];

                        for (int k = 0; k < engineScript.facings[i].rotationSpeed.Length; k++)
                        {
                            if (k < 1)
                            {
                                engineScript.facings[i].targets[k] = tempTargets[k];
                                engineScript.facings[i].lockTimes[k] = tempLockTimes[k];
                            }
                            engineScript.facings[i].rotationSpeed[k] = tempRotationSpeed[k];

                        }

                        // Get the facing time of the waypoint
                        engineScript.facings[i].rotationSpeed[0] = EditorGUILayout.FloatField(new GUIContent("Reach Time: ", "Time spent rotating to look at target point"),
                            engineScript.facings[i].rotationSpeed[0]);

                        // Facing time can't go less than zero
                        if (engineScript.facings[i].rotationSpeed[0] < 0)
                            engineScript.facings[i].rotationSpeed[0] = 0;

                        // Look point game object setter
                        EditorGUILayout.BeginHorizontal();

                        EditorGUILayout.LabelField(new GUIContent("Focus Point: ", "Point camera will look at."), GUILayout.Width(117));

                        engineScript.facings[i].targets[0] = (GameObject)
                            EditorGUILayout.ObjectField(engineScript.facings[i].targets[0], typeof(GameObject), true,
                                GUILayout.Width(156));

                        EditorGUILayout.EndHorizontal();

                        // Get the facing time of the waypoint
                        
                        engineScript.facings[i].lockTimes[0] = EditorGUILayout.FloatField(new GUIContent("Time At: ", "Time spent looking at point"),
                            engineScript.facings[i].lockTimes[0]);

                        // Facing time can't go less than zero
                        if (engineScript.facings[i].lockTimes[0] < 0)
                            engineScript.facings[i].lockTimes[0] = 0;

                        // Get the facing time of the waypoint
                        engineScript.facings[i].rotationSpeed[1] = EditorGUILayout.FloatField(new GUIContent("Return Time: ", "Time spent returning to original rotation"),
                            engineScript.facings[i].rotationSpeed[1]);

                        // Facing time can't go less than zero
                        if (engineScript.facings[i].rotationSpeed[1] < 0)
                            engineScript.facings[i].rotationSpeed[1] = 0;

                        // End of "property drawer"
                        break;

                    // It's a wait
                    case FacingTypes.WAIT:

                        // Get the facing time of the waypoint
                        engineScript.facings[i].facingTime = EditorGUILayout.FloatField(new GUIContent("Wait Time: ", "Time spent waiting"),
                            engineScript.facings[i].facingTime);

                        // Facing time can't go less than zero
                        if (engineScript.facings[i].facingTime < 0)
                            engineScript.facings[i].facingTime = 0;

                        // End of "property drawer"
                        break;
                }
                // Decrement the indent level
                EditorGUI.indentLevel--;
            }
            // Decrement the indent level
            EditorGUI.indentLevel--;
        }

        // End the scroll view
        EditorGUILayout.EndScrollView();

        // Create a button that adds a new waypoint to the movement array
        if (GUILayout.Button("Add New Facing Waypoint"))
        {
            ScriptFacings tempType = new ScriptFacings
            {
                facingType = FacingTypes.WAIT,
                facingTime = 0.1f
            };
            engineScript.facings.Add(tempType);
        }

        EditorGUILayout.EndVertical();
        #endregion

        #region Effects
        // Create area for the effects
        EditorGUILayout.BeginVertical("box");

        // Create a title showing the type of waypoints that are being listed in this area
        EditorGUILayout.LabelField("Effect Waypoints", EditorStyles.boldLabel);

        // Create a scroll view that allows designer to have an infinitely long list of waypoints
        scrollPos3 = EditorGUILayout.BeginScrollView(scrollPos3, GUILayout.Width(300), GUILayout.Height(430));
        for (int i = 0; i < engineScript.effects.Count; i++)
        {
            // Create some space for ease of reading
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            // Create a section showing label information along with move up and down
            EditorGUILayout.BeginHorizontal();

            // Formatting for if point is at beginning or end of list
            if (!CanMoveUp(i) || !CanEffectsMoveDown(i))
            {
                EditorGUILayout.LabelField("Waypoint " + (i + 1), EditorStyles.boldLabel, GUILayout.Width(218));
            }
            else
            {
                EditorGUILayout.LabelField("Waypoint " + (i + 1), EditorStyles.boldLabel, GUILayout.Width(190));
            }

            // Check to see if element can move up in the array
            if (CanMoveUp(i))
            {
                // If it can, create a button that allows designer to move the element upwards
                if (GUILayout.Button("/\\", GUILayout.Width(25f)))
                {
                    SwapEffects(i, i - 1);
                }
            }

            // Check to see if element can move down in the array
            if (CanEffectsMoveDown(i))
            {
                // If it can, create a button that allows designer to move element downwards
                if (GUILayout.Button("\\/", GUILayout.Width(25f)))
                {
                    SwapEffects(i, i + 1);
                }
            }

            oldColor = GUI.color;
            GUI.color = Color.red;

            // Create a red button that removes the element from the array of waypoints
            if (GUILayout.Button("X", GUILayout.Width(25f)))
            {

                engineScript.effects.RemoveAt(i);
                return;

            }

            // Revert the GUI color to previous color before red button
            GUI.color = oldColor;

            EditorGUILayout.EndHorizontal();

            // Create an enum for the movement types
            engineScript.effects[i].effectType = (EffectTypes)EditorGUILayout.EnumPopup(engineScript.effects[i].effectType);

            EditorGUI.indentLevel++;
            // Create a foldout that shows all of the waypoint specific variables
            engineScript.effects[i].dataFoldout = EditorGUILayout.Foldout(engineScript.effects[i].dataFoldout,
                string.Format("Show Effect Waypoint {0} Data", i + 1));

            // If the foldout is open
            if (engineScript.effects[i].dataFoldout)
            {
                EditorGUI.indentLevel++;
                // Switch between the different move types
                switch (engineScript.effects[i].effectType)
                {
                    // It's a free look
                    case EffectTypes.FADE:

                        // Get the fade out time of the waypoint
                        engineScript.effects[i].fadeOutTime = EditorGUILayout.FloatField(new GUIContent("Fade Out Time: ", "Time to fade to black"),
                            engineScript.effects[i].fadeOutTime);

                        // fade in time can't go less than zero
                        if (engineScript.effects[i].fadeOutTime < 0)
                            engineScript.effects[i].fadeOutTime = 0;

                        // Get the facing time of the waypoint
                        engineScript.effects[i].effectTime = EditorGUILayout.FloatField(new GUIContent("Faded Time: ", "Time spent with black screen."),
                            engineScript.effects[i].effectTime);

                        // Facing time can't go less than zero
                        if (engineScript.effects[i].effectTime < 0)
                            engineScript.effects[i].effectTime = 0;

                        // Get the fade out time of the waypoint
                        engineScript.effects[i].fadeInTime = EditorGUILayout.FloatField(new GUIContent("Fade In Time: ", "Time to fade the camera in"),
                            engineScript.effects[i].fadeInTime);

                        // fade in time can't go less than zero
                        if (engineScript.effects[i].fadeInTime < 0)
                            engineScript.effects[i].fadeInTime = 0;

                        // End of "property drawer"
                        break;

                    // It's a look at
                    case EffectTypes.SHAKE:

                        // Get the facing time of the waypoint
                        engineScript.effects[i].effectTime = EditorGUILayout.FloatField(new GUIContent("Effect Time: ", "Time camera spends shaking"),
                            engineScript.effects[i].effectTime);

                        // Facing time can't go less than zero
                        if (engineScript.effects[i].effectTime < 0)
                            engineScript.effects[i].effectTime = 0;

                        EditorGUILayout.LabelField(new GUIContent("Magnitude: ", "Moves camera X units"));
                        engineScript.effects[i].magnitude = EditorGUILayout.Slider(engineScript.effects[i].magnitude, 0.1f,
                            3.0f);


                        // End of "property drawer"
                        break;

                    // It's a look chain
                    case EffectTypes.SPLATTER:

                        // Get the facing time of the waypoint
                        engineScript.effects[i].effectTime = EditorGUILayout.FloatField(new GUIContent("Effect Time: ", "Time camera spends splattered"),
                            engineScript.effects[i].effectTime);

                        // Facing time can't go less than zero
                        if (engineScript.effects[i].effectTime < 0)
                            engineScript.effects[i].effectTime = 0;

                        engineScript.effects[i].imageScale = EditorGUILayout.FloatField(new GUIContent("Image Scale: ", "16:9 aspect ratio"),
                            engineScript.effects[i].imageScale);

                        if (engineScript.effects[i].imageScale <= 0)
                            engineScript.effects[i].imageScale = 0.1f;

                        // End of "property drawer"
                        break;

                    case EffectTypes.WAIT:

                        // Get the facing time of the waypoint
                        engineScript.effects[i].effectTime = EditorGUILayout.FloatField(new GUIContent("Wait Time: ", "Time spent waiting"),
                            engineScript.effects[i].effectTime);

                        // Facing time can't go less than zero
                        if (engineScript.effects[i].effectTime < 0)
                            engineScript.effects[i].effectTime = 0;



                        // End of "property drawer"
                        break;
                }
                // Decrement the indent level
                EditorGUI.indentLevel--;
            }
            // Decrement the indent level
            EditorGUI.indentLevel--;
        }

        // End the scroll view
        EditorGUILayout.EndScrollView();

        // Create a button that adds a new waypoint to the movement array
        if (GUILayout.Button("Add New Effect Waypoint"))
        {
            ScriptEffects tempType = new ScriptEffects
            {
                effectType = EffectTypes.WAIT,
                effectTime = 0.1f
            };
            engineScript.effects.Add(tempType);

            ScriptExportLevelData.Init();
        }

        EditorGUILayout.EndVertical();
        #endregion

        // End of waypoints area.
        EditorGUILayout.EndHorizontal();

        #endregion

        #region Export Button
        // Begin area for the export data button.
        GUILayout.BeginHorizontal();
        GUILayout.BeginArea(new Rect((Screen.width / 3) * 2 + 200, position.height - 27, 150, 50));
            oldColor = GUI.color;
            GUI.color = Color.green;
            if (GUILayout.Button("Export Data", GUILayout.Width(100), GUILayout.Height(20)))
            {
                ScriptConfirmationWindow.Init();
            }
            GUI.color = oldColor;
        GUILayout.EndArea();
        GUILayout.EndHorizontal();

        #endregion

    }

    #region Helper Functions
    // Method to see whether element is at first position of array or not
    bool CanMoveUp(int i)
    {
        // Return whether the index is at first position of array
        return i >= 1;

    }

    // Method to see whether element is at last position of array or not
    bool CanMovementMoveDown(int i)
    {
        // Return whether the index is at end of the array
        return i < engineScript.movements.Count - 1;

    }

    // Method to see whether element is at last position of array or not
    bool CanFacingMoveDown(int i)
    {
        // Return whether the index is at end of the array
        return i < engineScript.facings.Count - 1;

    }

    // Method to see whether element is at last position of array or not
    bool CanEffectsMoveDown(int i)
    {
        // Return whether the index is at end of the array
        return i < engineScript.effects.Count - 1;

    }
    // Got swap code from: http://stackoverflow.com/questions/2094239/swap-two-items-in-listt
    public void SwapMovements(int i1, int i2)
    {
        ScriptMovements tmp = engineScript.movements[i1];
        engineScript.movements[i1] = engineScript.movements[i2];
        engineScript.movements[i2] = tmp;
    }

    public void SwapFacings(int i1, int i2)
    {
        ScriptFacings tmp = engineScript.facings[i1];
        engineScript.facings[i1] = engineScript.facings[i2];
        engineScript.facings[i2] = tmp;
    }

    public void SwapEffects(int i1, int i2)
    {
        ScriptEffects tmp = engineScript.effects[i1];
        engineScript.effects[i1] = engineScript.effects[i2];
        engineScript.effects[i2] = tmp;
    }
    #endregion
}