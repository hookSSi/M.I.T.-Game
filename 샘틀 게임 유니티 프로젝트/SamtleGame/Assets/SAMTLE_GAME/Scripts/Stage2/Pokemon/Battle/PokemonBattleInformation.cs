using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MIT.SamtleGame.Stage2
{
    public class PokemonBattleInformation : MonoBehaviour
    {
        private void Awake()
        {
            EventTrigger trigger = GetComponent<EventTrigger>();
            EventTrigger.Entry entryCancel = new EventTrigger.Entry();
            entryCancel.eventID = EventTriggerType.Cancel;
            entryCancel.callback.AddListener((data) => { Pokemon.PokemonBattleManager.Instance.SelectAction(); });
            trigger.triggers.Add(entryCancel);
        }
    }
}