using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.Tools;

public class Pad : MonoBehaviour
{
    private bool isPressed = false;
    public Color _color = Color.yellow;
    
    public void Press()
    {
        if(!isPressed)
            StartCoroutine(LightOn(0.1f));
    }

    private IEnumerator LightOn(float delay)
    {
        isPressed = true;
        GetComponent<Renderer>().material.SetFloat("_factor", 0f);
        GetComponent<Renderer>().material.SetColor("_targetColor", _color);
        yield return new WaitForSeconds(delay);

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
