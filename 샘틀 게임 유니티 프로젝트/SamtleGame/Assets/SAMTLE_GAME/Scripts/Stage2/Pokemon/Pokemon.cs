using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MIT.SamtleGame.Stage2.Pokemon
{
    [System.Serializable]
    // public delegate void BattleEvent(Pokemon myPokemon, Pokemon enemyPokemon);
    public class BattleEvent : UnityEvent<Pokemon, Pokemon> { }

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
        public BattleEvent _event;
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
        public List<Skill> _skills;
    }

    public class Pokemon : MonoBehaviour
    {
        [SerializeField]
        private PokemonInfo _info;
        [SerializeField]
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

            for (int i = 0; i < newInfo._skills.Count; i++)
            {
                _info._skills[i]._currentCount = newInfo._skills[i]._count;

                /// 스킬 이벤트 할당 과정
                /// 버그로 인해서인지, 유니티 이벤트의 스킬이 할당되지 않을 때가 있다. 이를 해결하기 위한 코드
                Type tp = typeof(SkillClass);
                string methodName = newInfo._skills[i]._event.GetPersistentMethodName(0);
                MethodInfo method = tp.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
                Delegate d = Delegate.CreateDelegate(typeof(UnityAction<Pokemon, Pokemon>), PokemonBattleManager.Instance._uiManager._skillClass, method);
                _info._skills[i]._event.AddListener((UnityAction<Pokemon, Pokemon>) d);
            }
        }
    }
}

