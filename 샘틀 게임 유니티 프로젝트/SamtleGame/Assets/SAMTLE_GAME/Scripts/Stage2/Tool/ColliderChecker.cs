using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage2.Tool
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
        public static bool CheckColliders(Vector3 centerPoint, Vector2 size, string tag)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(centerPoint, size, 0);
            
            foreach(var col in colliders)
            {
                if(col.tag == tag)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckColliders(Vector3 centerPoint, Vector2 size, string[] tags)
        {
            foreach(var tag in tags)
            {
                if(CheckColliders(centerPoint, size, tag))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckColliders(Vector3 centerPoint, Vector2 size, Tag[] tags)
        {
            foreach(var tag in tags)
            {
                if(CheckColliders(centerPoint, size, tag.GetTag()))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<Collider2D> GetColliders(Vector3 centerPoint, Vector2 size, string tag)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(centerPoint, size, 0);
            List<Collider2D> checkedColliders = new List<Collider2D>();
            
            foreach(var col in colliders)
            {
                if(col.tag == tag)
                {
                    checkedColliders.Add(col);
                }
            }

            return checkedColliders;
        }
    }
}
