using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MIT.SamtleGame.Stage2.Pokemon
{
    public class PokemonBattleSkill : MonoBehaviour
    {
        public GameObject[] _skills = new GameObject[4];
        public Text _skillPpText;

        private Text[] _skillTexts;
        private Selectable[] _selectable;
        private Pokemon _playerPokemon;

        public void Init()
        {
            _skillTexts = new Text[4];
            _selectable = new Selectable[4];

            for (int i = 0; i < _skillTexts.Length; i++)
            {
                if (_skills[i])
                {
                    if (!_skillTexts[i])
                        _skillTexts[i] = _skills[i].GetComponentInChildren<Text>();

                    _selectable[i] = _skills[i].GetComponent<Selectable>();

                    EventTrigger trigger = _skills[i].GetComponent<EventTrigger>();
                    int index = i;

                    EventTrigger.Entry entrySubmit = new EventTrigger.Entry();
                    entrySubmit.eventID = EventTriggerType.Submit;
                    entrySubmit.callback.AddListener((data) => { PokemonBattleManager.Instance.UseSkill(index);});
                    trigger.triggers.Add(entrySubmit);

                    EventTrigger.Entry entrySelect = new EventTrigger.Entry();
                    entrySelect.eventID = EventTriggerType.Select;
                    entrySelect.callback.AddListener((data) => { UpdateSkill(index); });
                    trigger.triggers.Add(entrySelect);

                    EventTrigger.Entry entryCancel = new EventTrigger.Entry();
                    entryCancel.eventID = EventTriggerType.Cancel;
                    entryCancel.callback.AddListener((data) => { PokemonBattleManager.Instance.SelectAction(); });
                    trigger.triggers.Add(entryCancel);
                }
                
            }
        }

        public void SetPokemon(Pokemon playerPokemon)
        {
            if (playerPokemon == null)
                return;

            _playerPokemon = playerPokemon;
        }

        public void UpdateText()
        {
            for (int i = 0; i < _skillTexts.Length; i++)
            {
                if (_skills[i])
                {
                    // 해당하는 스킬이 존재할 때
                    if (i < _playerPokemon.Info._skills.Count)
                    {
                        _selectable[i].interactable = true;
                        _skillTexts[i].text = _playerPokemon.Info._skills[i]._name;
                    }
                    else
                    {
                        _selectable[i].interactable = false;
                        _skillTexts[i].text = "";
                    }
                }
            }
        }

        public void UpdateSkill(int indexOfSkill)
        {
            if (_playerPokemon == null)
                return;

            if (indexOfSkill >= _playerPokemon.Info._skills.Count)
                return;

            if (_skillPpText != null)
            {
                int count = _playerPokemon.Info._skills[indexOfSkill]._currentCount;
                int maxCount = _playerPokemon.Info._skills[indexOfSkill]._count;

                _skillPpText.text = count + "/ " + maxCount;
            }
        }

        public GameObject GetFirstObject()
        {
            return _skills[0];
        }
    }
}