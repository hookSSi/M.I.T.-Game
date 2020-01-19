using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon
{
    public class PokemonBattleAction : MonoBehaviour
    {
        [SerializeField] private GameObject[] _actions;
        [SerializeField] private string[] _actionStrings;

        private Text[] _actionTexts;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _actionTexts = new Text[_actions.Length];

            if (_actions != null && _actions.Length != 0)
            {
                for(int i = 0; i < _actions.Length; i++)
                {
                    if (_actions[i])
                    {
                        _actionTexts[i] = _actions[i].GetComponentInChildren<Text>();
                        _actionTexts[i].text = _actionStrings[i];
                    }
                    
                }
            }
        }
#endif
    }
}