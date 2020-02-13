using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MIT.SamtleGame.Stage2.Pokemon
{
    [System.Serializable]
    public class Skill
    {
        [Tooltip("기술명")]
        public string _name;
        [Tooltip("남은 갯수")]
        public string _count;
        public BattleEvent _battleEvent;
    }
    [System.Serializable]
    public class PokemonInfo
    {
        [Header("일반 정보")]
        [Tooltip("전면 이미지")]
        public Sprite _frontImage;
        [Tooltip("후면 이미지")]
        public Sprite _backImage;
        [Tooltip("울음 소리")]
        public AudioClip _cryingSound;
        [Tooltip("포켓몬 이름")]
        public string _name;
        [Tooltip("체력")]
        public float _health = 100f;

        [Header("기술")]
        [SerializeField]
        public Skill[] _skills;

    }

    public class Pokemon : MonoBehaviour
    {
        [SerializeField]
        private PokemonInfo _info;
        public PokemonInfo Info { get => _info; }

        private float _health;
        public float Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = value;
                if (_health < 0f) _health = 0f;
            }
        }
        
        public float MaxHealth
        {
            get { return _info._health; }
        }

        public Skill UseSkill(int n)
        {
            return _info._skills[n];
        }

        public virtual void GetHitSkill(Skill skill)
        {
            Debug.Log(skill._name + "에 당했다!");
        }

        public AudioClip Cry()
        {
            return _info._cryingSound;
        }

        public void SetInfo(PokemonInfo newInfo)
        {
            if (newInfo == null)
                return;

            _info = newInfo;
            Health = MaxHealth;
        }
    }
}

