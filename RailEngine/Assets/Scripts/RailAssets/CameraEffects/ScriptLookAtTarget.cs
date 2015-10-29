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
	

	public void Activate(float[] pRotateSpeed, GameObject[] pTargets, float[]pLockTimes)
	{
		rotateSpeed = pRotateSpeed;
		targets = pTargets;
		lockTime = pLockTimes;
		startRotation = transform.rotation;
		StopCoroutine ("ReturnLook");
		StartCoroutine("LookAtTarget");
	}

	IEnumerator LookAtTarget()
	{
		for (int i = 0; i < targets.Length; i++ )
		{
		    startRotation = transform.rotation;
            Quaternion neededRotation = Quaternion.LookRotation((targets[i].transform.position - transform.position).normalized);


            // Lerp code gotten from http://answers.unity3d.com/questions/672456/rotate-an-object-a-set-angle-over-time-c.html
            for (var t = 0f; t < 1; t += Time.deltaTime / rotateSpeed[i])
            {
                transform.rotation = Quaternion.Lerp(startRotation, neededRotation, t);
                yield return null;
            }

            yield return new WaitForSeconds(lockTime[i]);
		}
		StartCoroutine("ReturnLook");
	}



	IEnumerator ReturnLook()
	{
	    startRotation = transform.rotation;

        // Lerp code gotten from http://answers.unity3d.com/questions/672456/rotate-an-object-a-set-angle-over-time-c.html
        for (var t = 0f; t < 1; t += Time.deltaTime / rotateSpeed[rotateSpeed.Length - 1])
        {
            transform.rotation = Quaternion.Lerp(startRotation, Quaternion.Euler(Vector3.forward), t);
            yield return null;
        }

    }

    // Free Look code gotten from Project 2
    // Author: Nathan Boehning
    IEnumerator FreeLook(float facingTime)
    {
        float elapsedTime = 0f; // keeps track of elapsed time to continue facing
        float xRotation = 0f;
        float yRotation = 0f;
        float lookSensitivity = 3f;
        float curXRotation = transform.rotation.x;
        float curYRotation = transform.rotation.y;
        float lookSmoothDamp = 0.1f;
        float xRotationV = 0;
        float yRotationV = 0;

        // Make cursor invisible and locked in the middle of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        while (elapsedTime < facingTime) // iterate through the loop for faceSeconds seconds
        {

            // Increment the yRotation using mouse input mulitplied by the mouse sensitivity
            yRotation += Input.GetAxis("Mouse X") * lookSensitivity;

            // Decrement the xRotation using mouse input. (incrementing creates an inverted effect)
            xRotation -= Input.GetAxis("Mouse Y") * lookSensitivity;

            // Lock the rotation so camera can only look straight up or straight down without going circular
            xRotation = Mathf.Clamp(xRotation, -90, 90);

            // Smooth the rotation to prevent screen tearing
            curXRotation = Mathf.SmoothDamp(curXRotation, xRotation, ref xRotationV, lookSmoothDamp);
            curYRotation = Mathf.SmoothDamp(curYRotation, yRotation, ref yRotationV, lookSmoothDamp);

            // Set the rotation of the camera to new rotation
            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

            // Increment the elapsed time by the change in time
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return null;
    } // end method FaceFreeLook
}
