using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage2.Tools
{
    [System.Serializable]
    public struct Tag
    {
        public enum EnumFlag { Player = 0, Npc = 1, Obstacle = 2 }
        public EnumFlag _flag;
        
        private static string[] tags = { "Player", "Npc", "Obstacle" };
        public string GetTag() { return tags[(int)_flag]; }
    }

    public class ColliderChecker : MonoBehaviour
    {
        public static bool CheckColliders(Vector2 origin, Vector2 dest, string tag, bool isDebug = false)
        {
            RaycastHit2D[] hitted = Physics2D.LinecastAll(origin, dest);
            
            foreach(var obj in hitted)
            {
                if(obj.collider.tag == tag)
                {
                    if(isDebug)
                        Debug.Log(obj.collider.tag);
                    return true;
                }
            }
            return false;
        }
        public static bool CheckColliders(Vector2 origin, Vector2 dest, string tag, RaycastHit2D[] result)
        {
            Physics2D.LinecastNonAlloc(origin, dest, result);
            
            foreach(var obj in result)
            {
                if(obj.collider.tag == tag)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckColliders(Vector2 origin, Vector2 dest, string[] tags)
        {
            foreach(var tag in tags)
            {
                if(CheckColliders(origin, dest, tag))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckColliders(Vector2 origin, Vector2 dest, Tag[] tags)
        {
            foreach(var tag in tags)
            {
                if(CheckColliders(origin, dest, tag.GetTag()))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<Collider2D> GetColliders(Vector2 origin, Vector2 dest, string tag)
        {
            RaycastHit2D[] hitted = Physics2D.LinecastAll(origin, dest);
            List<Collider2D> checkedColliders = new List<Collider2D>();
            
            foreach(var obj in hitted)
            {
                if(obj.collider.tag == tag)
                {
                    checkedColliders.Add(obj.collider);
                }
            }

            return checkedColliders;
        }
    }
}
