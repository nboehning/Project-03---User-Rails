using UnityEngine;
using System.Collections;

/*
*   @author Mike Dobson
* */

[System.Serializable]
public class ScriptFacings
{
    public FacingTypes facingType;

    //Loot at target variables
    public GameObject[] targets;
	public float[] rotationSpeed;
	public float[] lockTimes;

    public float facingTime;

    // Boolean for look chain foldout in window @author: Nathan
    public bool isFoldedOut;
    public bool dataFoldout;
    // int for number of elements in the look chain @author: Nathan
    public int chainCount;
}
