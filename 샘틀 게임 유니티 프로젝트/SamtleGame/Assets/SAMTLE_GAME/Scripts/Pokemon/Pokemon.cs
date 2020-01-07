using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon
{
    [System.Serializable]
    public class Skill
    {
        [Tooltip("기술명")]
        public string _name;
        [Tooltip("남은 갯수")]
        public string _count;
    }

    public class Pokemon : MonoBehaviour
    {
        [Header("기술")]
        [SerializeField]
        private Skill[] _skills;

        [Header("체력")]
        [SerializeField]
        private float _health = 100f;

        public Skill UseSkill(int n)
        {
            return _skills[n];
        }

        public virtual void GetHitSkill(Skill skill)
        {
            Debug.Log(skill._name + "에 당했다!");
        }
    }
}

