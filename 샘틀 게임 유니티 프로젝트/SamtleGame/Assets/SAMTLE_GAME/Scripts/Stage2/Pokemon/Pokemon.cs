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
        public int _count;
        [HideInInspector]
        public int _currentCount;
        [Tooltip("스킬 이벤트")]
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
        private float _health;

        public enum StatusEffect { None, AttackDown }
        public StatusEffect _status = StatusEffect.None;
        public int _effectCount = 0;

        public PokemonInfo Info { get => _info; }

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
                if (_health > MaxHealth) _health = MaxHealth;
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

            for (int i = 0; i < newInfo._skills.Length; i++)
                _info._skills[i]._currentCount = newInfo._skills[i]._count;
        }
    }
}

