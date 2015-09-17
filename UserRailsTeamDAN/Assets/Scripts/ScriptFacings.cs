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
    public GameObject[] targets = new GameObject[100];
    public float[] rotationSpeed = new float[100];
    public float[] lockTimes = new float[100];
}
