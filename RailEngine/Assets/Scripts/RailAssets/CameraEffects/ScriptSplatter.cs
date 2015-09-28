using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Author: Andew Seba
/// Desceiprion: Displays a designer specified sprite object on the screen
///     for a specified amount of time.
/// </summary>
public class ScriptSplatter : MonoBehaviour {


    public float effectTime = 1.0f;


    public float fadeInTime = 0;


    public float fadeOutTime = 0f;


    public GameObject splatImage;


    public float imageScale = 4;


    SpriteRenderer splatRenderer;
    Rect splatRect;
    Color splatColor;
    float smoothness = 0.02f;


    //void Update()
    //{
    //    if (Input.GetButtonDown("Jump"))
    //    {

    //        Activate(5, 1, 1, 1);

    //    }

    //}

#if UNITYEDITOR
    public void Awake()
    {
        if(splatImage != null)
        {
            splatRenderer = splatImage.GetComponent<SpriteRenderer>();
        }
        else
        {
            Debug.Log("No sprite image assigned to script. Drag it from the prefabs.");
        }
    }
#else
    void Awake()
    {
        splatImage = (GameObject)Resources.Load("splat_1");
        splatRenderer = splatImage.GetComponent<SpriteRenderer>();
    }
#endif

    public void Activate()
    {
        splatRect = new Rect(Random.Range(0, Screen.width / 2), Random.Range(0, Screen.height / 2), Screen.width / 16 * imageScale, Screen.height / 9 * imageScale);
        StartCoroutine("SplatFadeIn");
    }

    public void Activate(float pEffectTime, float pFadeInTime, float pFadeOutTime)
    {
        //@ Mike
        effectTime = pEffectTime;
        fadeInTime = pFadeInTime;
        fadeOutTime = pFadeOutTime;
        //end @ mike
        splatRect = new Rect(Random.Range(0, Screen.width / 2), Random.Range(0, Screen.height / 2), Screen.width / 16 * imageScale, Screen.height / 9 * imageScale);
        StartCoroutine("SplatFadeIn");
    }

    public void Activate(float pEffectTime, float pFadeInTime, float pFadeOutTime, float pImageScale)
    {
        //@ Mike
        effectTime = pEffectTime;
        fadeInTime = pFadeInTime;
        fadeOutTime = pFadeOutTime;
        imageScale = pImageScale;
        //end @ mike
        splatRect = new Rect(Random.Range(0, Screen.width /2), Random.Range(0, Screen.height /2), Screen.width / 16 * imageScale, Screen.height / 9 * imageScale);
        StartCoroutine("SplatFadeIn");
    }

    //Draws the splat
    void OnGUI()
    {
        GUI.color = splatColor;
        GUI.DrawTexture(splatRect, splatRenderer.sprite.texture, ScaleMode.StretchToFill);
    }

    IEnumerator SplatFadeIn()
    {
        float progress = 0;

        float increment = smoothness / fadeInTime;

        while (progress <= 1)
        {
            splatColor = Color.Lerp(Color.clear, splatRenderer.color, progress);
            progress += increment;
            yield return null;
        }

        StartCoroutine("SplatStay");
    }

    IEnumerator SplatStay()
    {
        float timePassed = 0.0f;
        

        while (timePassed <= effectTime)
        {
            timePassed += 1 * Time.deltaTime;
            yield return null;
        }
        StartCoroutine("SplatFadeOut");
    }


    IEnumerator SplatFadeOut()
    {
        float progress = 0;

        float increment = smoothness / fadeOutTime;

        while (progress < 1)
        {
            splatColor = Color.Lerp(splatRenderer.color, Color.clear, progress);
            progress += increment;
            yield return null;
        }
        splatColor = Color.clear;
    }
}
