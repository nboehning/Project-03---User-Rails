using UnityEngine;
using System.Collections;

/// <summary>
/// Author:Andrew Seba
/// Description: Controls the transform to look at a specified target and return
/// </summary>
public class ScriptLookAtTarget : MonoBehaviour {

    [Tooltip("How fast the camera will rotate to the target.")]
    public float[] rotateSpeed;

    [Tooltip("Place the target object for the camera to look at.")]
    public GameObject[] targets;

    [Tooltip("How long you will lock on target.")]
    public float[] lockTime;

    Quaternion startRotation;
    

    public void Activate()
    {
        startRotation = transform.rotation;
        StartCoroutine("LookAtTarget");
    }

    IEnumerator LookAtTarget()
    {
        for (int i = 0; i < targets.Length; i++ )
        {
            float timeElapsed = 0.0f;
            while (timeElapsed < lockTime[i])
            {
                timeElapsed += Time.deltaTime;
                Quaternion neededRotation = Quaternion.LookRotation(targets[i].transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, neededRotation, Time.deltaTime * rotateSpeed[i]);
                yield return null;
            }
        }

        StartCoroutine("ReturnLook");
    }

    IEnumerator ReturnLook()
    {
        StopCoroutine("LookAtTarget");
        while (transform.forward != Vector3.forward)
        {

            transform.rotation = Quaternion.RotateTowards(transform.rotation, startRotation, rotateSpeed[targets.Length]);

            yield return null;
        }
    }
}
