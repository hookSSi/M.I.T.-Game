using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MIT.SamtleGame.DesignPattern;

namespace MIT.SamtleGame.Stage3
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private bool _programmer = false;
        [SerializeField] private bool _artist = false;
        [SerializeField] private bool _musician = false;

        public Player _player;
        
        private void Start() 
        {
            _player = (Player)FindObjectOfType(typeof(Player));
        }

        public void Programmer()
        {
            Debug.Log("프로그래머 상호작용");
            _programmer = true;
        }
        public void Artist()
        {
            Debug.Log("아티스트 상호작용");
            _artist = true;
        }
        public void Musician()
        {
            Debug.Log("뮤지션 상호작용");
            _musician = true;
        }
    }
}
