using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Tilemaps;

using System.Linq;
using System.Text;

namespace MIT.SamtleGame.Tools
{
    public class GameProfiler : MonoBehaviour
    {
        private void Start() {
            this.DoProfile(typeof(Sprite));
            this.DoProfile(typeof(Collider2D));
            this.DoProfile(typeof(TilemapCollider2D));
        }

        public void DoProfile(System.Type type)
        {
            var sortedAll = Resources.FindObjectsOfTypeAll(type).OrderBy(go=>Profiler.GetRuntimeMemorySizeLong(go)).ToList();

            StringBuilder sb = new StringBuilder("");
            long memTexture = 0;

            for( int i = sortedAll.Count-1; i >= 0; i-- )
            {
                if(!sortedAll[i].name.StartsWith("d_"))
                {
                    memTexture += Profiler.GetRuntimeMemorySizeLong(sortedAll[i]);
                    sb.Append(type.ToString());
                    sb.Append("Size#");
                    sb.Append(sortedAll.Count-i);
                    sb.Append(":");
                    sb.Append(sortedAll[i].name);
                    sb.Append("/InstanceID:");
                    sb.Append(sortedAll[i].GetInstanceID());
                    sb.Append("/Mem:");
                    sb.Append(Profiler.GetRuntimeMemorySizeLong(sortedAll[i]).ToString());
                    sb.Append("B/Total:");
                    sb.Append(memTexture/1024);
                    sb.Append("KB");
                    sb.Append("\n");
                }
            }

            Debug.Log("Inspect: "+sb.ToString());
        }
    }
}
