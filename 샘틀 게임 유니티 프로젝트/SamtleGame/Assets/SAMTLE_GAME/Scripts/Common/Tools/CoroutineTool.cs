using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Tools
{
    public static class CoroutineTool
    {
        public static IEnumerator WaitForRealSeconds(float time)
        {
            float start = Time.realtimeSinceStartup;
            while(Time.realtimeSinceStartup < start + time)
            {
                yield return null;
            }
        }
    }
}