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
}
