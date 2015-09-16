using UnityEngine;
using System.Collections;
using System.IO;

/// <summary>
/// @Author Marshall Mason
/// ScriptModSupport handles the I/O tasks of reading and outputting text files for modding.
/// </summary>
public class ScriptModSupport : MonoBehaviour
{

    public GameObject movementWaypoint;
    public GameObject facingWaypoint;
    public GameObject effectWaypoint;
    public ScriptEngine player;


    string defaultModFileText = "Edit this file for modding timelines and delete this line";

    FileInfo modFile = null;
    StreamReader reader = null;

    void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<ScriptEngine>();
            if (player == null)
            {
                ScriptErrorLogging.logError("No Player Object found, please add a player to the scene and check the tag.");
                Application.Quit();
            }
        }
        if (movementWaypoint == null)
        {
            movementWaypoint = (GameObject)Resources.Load("movementWaypoint.prefab", typeof(GameObject));
            if (movementWaypoint == null)
            {
                ScriptErrorLogging.logError("No movementWaypoint prefab found, please place one in the Resources folder");
                Application.Quit();
            }
        }
        if (facingWaypoint == null)
        {
            facingWaypoint = (GameObject)Resources.Load("facingWaypoint.prefab", typeof(GameObject));
            if (facingWaypoint == null)
            {
                ScriptErrorLogging.logError("No facingWaypoint prefab found, please place one in the Resources folder");
                Application.Quit();
            }
        }
        if (effectWaypoint == null)
        {
            effectWaypoint = (GameObject)Resources.Load("effectWaypoint.prefab", typeof(GameObject));
            if (effectWaypoint == null)
            {
                ScriptErrorLogging.logError("No effectWaypoint prefab found, please place one in the Resources folder");
                Application.Quit();
            }
        }


        //Reads in a file like this
        /*
            M_move 5 0,0,17
            M_wait 7
            M_move 3 17,0,17
            M_bezier 4 0,0,0 17,0,0
        */
        modFile = new FileInfo(Application.dataPath + "/waypoints.txt");
        if (!modFile.Exists)
        {
            File.WriteAllText(Application.dataPath + "/waypoints.txt", defaultModFileText);
        }
        else
        {

            reader = modFile.OpenText();
            if (reader.ReadLine() != defaultModFileText)
            {
                reader.Close();
                System.Collections.Generic.List<ScriptMovements> tempMovements = new System.Collections.Generic.List<ScriptMovements>(0);
                System.Collections.Generic.List<ScriptEffects> tempEffects = new System.Collections.Generic.List<ScriptEffects>(0);
                System.Collections.Generic.List<ScriptFacings> tempFacings = new System.Collections.Generic.List<ScriptFacings>(0);

                GameObject[] waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
                foreach (GameObject go in waypoints)
                {
                    Destroy(go);
                }
                reader = modFile.OpenText();
                string inputLine = reader.ReadLine();
                while (inputLine != null)
                {
                    ScriptMovements tempMove;
                    string[] coords;
                    Vector3 target;
                    string[] keywords = inputLine.Split('_');
                    if (keywords[0].ToUpper() == "M")
                    {
                        string[] words = keywords[1].Split(' ');
                        switch ((MovementTypes)System.Enum.Parse(typeof(MovementTypes), words[0].ToUpper()))
                        {
                            case MovementTypes.MOVE:
                                tempMove = new ScriptMovements();
                                tempMove.moveType = MovementTypes.MOVE;
                                tempMove.movementTime = (float)System.Convert.ToDouble(words[1]);
                                coords = words[2].Split(',');
                                target = new Vector3(System.Convert.ToSingle(coords[0]),
                                    System.Convert.ToSingle(coords[1]), System.Convert.ToSingle(coords[2]));
                                tempMove.endWaypoint = (GameObject)Instantiate(movementWaypoint, target, Quaternion.identity);
                                tempMovements.Add(tempMove);
                                break;
                            case MovementTypes.WAIT:
                                tempMove = new ScriptMovements();
                                tempMove.moveType = MovementTypes.WAIT;
                                tempMove.movementTime = System.Convert.ToSingle(words[1]);
								tempMove.movementTime = (float)System.Convert.ToDouble(words[1]);
                                tempMovements.Add(tempMove);
                                break;
                            case MovementTypes.BEZIER:
                                tempMove = new ScriptMovements();
                                tempMove.moveType = MovementTypes.BEZIER;
                                tempMove.movementTime = System.Convert.ToSingle(words[1]);
								tempMove.movementTime = (float)System.Convert.ToDouble(words[1]);
                                coords = words[2].Split(',');
                                target = new Vector3(System.Convert.ToSingle(coords[0]),
                                    System.Convert.ToSingle(coords[1]), System.Convert.ToSingle(coords[2]));
                                tempMove.endWaypoint = (GameObject)Instantiate(movementWaypoint, target, Quaternion.identity);
                                coords = words[3].Split(',');
                                target = new Vector3(System.Convert.ToSingle(coords[0]),
                                    System.Convert.ToSingle(coords[1]), System.Convert.ToSingle(coords[2]));
                                tempMove.curveWaypoint = (GameObject)Instantiate(movementWaypoint, target, Quaternion.identity);
                                tempMovements.Add(tempMove);
                                break;
                        }
                    }
                    else if (keywords[0].ToUpper() == "E")
                    {
                        ScriptScreenFade tempFade;
                        ScriptSplatter tempSplatter;
                        ScriptLookAtTarget tempLookAt;
                        ScriptCameraShake tempShake;
                        ScriptEffects tempEffect;
                        string[] words = keywords[1].Split(' ');
                        switch ((EffectTypes)System.Enum.Parse(typeof(EffectTypes), words[0].ToUpper()))
                        {
                            //@ mike @ reference Marshall
                            case EffectTypes.FADE:

                                tempFade = new ScriptScreenFade();
                                tempFade.fadeInLength = (float)System.Convert.ToSingle(words[1]);

                                //Fade waypoint spawning Code
                                tempEffect = new ScriptEffects();
                                tempEffect.effectType = EffectTypes.FADE;
                                tempEffect.effectTime = System.Convert.ToSingle(words[1]);
                                tempEffect.fadeInTime = System.Convert.ToSingle(words[2]);
                                tempEffect.fadeOutTime = System.Convert.ToSingle(words[3]);
                                tempEffects.Add(tempEffect);

                                break;
                            case EffectTypes.SHAKE:
                                //Shake waypoint spawning Code
                                tempEffect = new ScriptEffects();
                                tempEffect.effectType = EffectTypes.SHAKE;
                                tempEffect.effectTime = System.Convert.ToSingle(words[1]);
                                tempEffect.magnitude = System.Convert.ToSingle(words[2]);
                                tempEffects.Add(tempEffect);
                                break;
                            case EffectTypes.SPLATTER:
                                //Splatter waypoint spawning Code
                                tempEffect = new ScriptEffects();
                                tempEffect.effectType = EffectTypes.SPLATTER;
                                tempEffect.effectTime = System.Convert.ToSingle(words[1]);
                                tempEffect.fadeInTime = System.Convert.ToSingle(words[2]);
                                tempEffect.fadeOutTime = System.Convert.ToSingle(words[3]);
                                tempEffect.imageScale = System.Convert.ToSingle(words[4]);
                                break;
                            case EffectTypes.WAIT:
                                //Effect Wait waypoint spawning Code
                                tempEffect = new ScriptEffects();
                                tempEffect.effectType = EffectTypes.WAIT;
                                tempEffect.effectTime = System.Convert.ToSingle(words[1]);
                                tempEffects.Add(tempEffect);
                                break;
                                //end @ mike
                        }
                    }
                    else if (keywords[0].ToUpper() == "F")
                    {
                        ScriptFacings tempFacing; 

                        string[] words = keywords[1].Split(' ');
                        switch ((FacingTypes)System.Enum.Parse(typeof(FacingTypes), words[0].ToUpper()))
                        {
                            case FacingTypes.LOOKAT:
                                //Look At waypoint spawning Code
                                
                                break;
                            case FacingTypes.LOOKCHAIN:
                                //Look Chain waypoint spawning Code
                                break;
                            case FacingTypes.WAIT:
                                //Facing Wait waypoint spawning Code
                                break;
                        }
                    }
                    inputLine = reader.ReadLine();
                }
                player.movements = new ScriptMovements[tempMovements.Count];
                for (int i = 0; i < tempMovements.Count; i++)
                {
                    player.movements[i] = tempMovements[i];
                }
                player.effects = new ScriptEffects[tempEffects.Count];
                for (int i = 0; i < tempEffects.Count; i++)
                {
                    player.effects[i] = tempEffects[i];
                }
            }
        }
    }
}
