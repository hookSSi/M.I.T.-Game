using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Tools;

public class Pad : MonoBehaviour
{
    private bool isPressed = false;
    [ColorUsageAttribute(true,true)] public Color _color;
    
    public void Press()
    {
        if(!isPressed)
            StartCoroutine(LightOn(0.33f));
    }

    private IEnumerator LightOn(float delay, float startFactor = 0.375f, float endFactor = 1f)
    {
        isPressed = true;
        GetComponent<Renderer>().material.SetFloat("_factor", startFactor);
        GetComponent<Renderer>().material.SetColor("_targetColor", _color);
        
        float lapse = 0f;
        float result = 0f;
        while ( lapse < delay )
        {
            result = startFactor + (endFactor - startFactor) * ( lapse / delay );
            GetComponent<Renderer>().material.SetFloat("_factor", result);
            lapse += Time.deltaTime;
            yield return null;
        }

        GetComponent<Renderer>().material.SetFloat("_factor", 1f);
        isPressed = false;
        yield break;
    }

    public void SetRandomColor()
    {
        _color = new Color(
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f)
            );
    }
}
