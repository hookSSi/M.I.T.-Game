using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MIT.SamtleGame.Stage2.Pokemon
{
    public class PokemonBattleAction : MonoBehaviour
    {
        public GameObject[] _actions = new GameObject[4];
        public string[] _actionStrings = new string[4];

        private Text[] _actionTexts = new Text[4];

        private void Awake()
        {
            Init();
            UpdateText();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            UpdateText();
        }
#endif

        private void Init()
        {
            for (int i = 0; i < 4; i++)
            {
                if (_actions[i] != null)
                {
                    if (_actionTexts[i] == null)
                        _actionTexts[i] = _actions[i].GetComponent<Text>();

                    // Button Event 추가
                    EventTrigger trigger = _actions[i].GetComponent<EventTrigger>();
                    EventTrigger.Entry entrySubmit = new EventTrigger.Entry();
                    entrySubmit.eventID = EventTriggerType.Submit;

                    switch (i)
                    {
                        case 0:
                            entrySubmit.callback.AddListener((data) => { PokemonBattleManager.Instance.SelectSkill(); });
                            break;
                        case 1:
                            entrySubmit.callback.AddListener((data) => { PokemonBattleManager.Instance.SelectItem(); });
                            break;
                        case 2:
                            entrySubmit.callback.AddListener((data) => { PokemonBattleManager.Instance.SelectPokemonInformation(); });
                            break;
                        case 3:
                            entrySubmit.callback.AddListener((data) => { PokemonBattleManager.Instance.Escape(); });
                            break;
                    }
                    trigger.triggers.Add(entrySubmit);

                    EventTrigger.Entry entryCancel = new EventTrigger.Entry();
                    entryCancel.eventID = EventTriggerType.Cancel;
                    entryCancel.callback.AddListener((data) => { PokemonBattleManager.Instance.SelectAction(); });
                    trigger.triggers.Add(entryCancel);
                }
            }
        }

        private void UpdateText()
        {
            for (int i = 0; i < 4; i++)
            {
                if (_actions[i] != null && _actionTexts[i] != null)
                {
                    _actionTexts[i].text = _actionStrings[i];
                }
            }
        }

        public GameObject GetFirstObject()
        {
            return _actions[0];
        }
    }
}