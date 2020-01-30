using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
   public float _lifeTime = 0.33f;

   private void Awake() 
   {
       StartCoroutine(Destroy());
   }

   private IEnumerator Destroy()
   {
        yield return new WaitForSeconds(_lifeTime);
        Destroy(this.gameObject);
        yield break;
   }
}
