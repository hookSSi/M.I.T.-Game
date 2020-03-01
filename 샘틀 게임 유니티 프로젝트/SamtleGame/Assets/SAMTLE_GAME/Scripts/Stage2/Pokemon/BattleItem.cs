using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace MIT.SamtleGame.Stage2.Pokemon
{
    [System.Serializable]
    public class BattleItem : MonoBehaviour
    {
        public string _itemName;
        public string _itemCaption;
        public int _itemCount;
        public ItemType _itemType;
        public BattleEvent _itemEvent;

        public enum ItemType
        {
            Consume, Pokeball, Etc
        }

        [SerializeField] private Text _itemNameText;
        [SerializeField] private Text _itemCountText;

        public BattleItem(string itemName, string itemCaption, int itemCount, ItemType itemType, BattleEvent itemEvent)
        {
            SetValues(itemName, itemCaption, itemCount, itemType, itemEvent);
            UpdateText();
        }

        public void SetValues(string itemName, string itemCaption, int itemCount, ItemType itemType, BattleEvent itemEvent)
        {
            _itemName = itemName;
            _itemCaption = itemCaption;
            _itemCount = itemCount;
            _itemType = itemType;
            _itemEvent = itemEvent;
        }

        public void SetCount(int newCount)
        {
            _itemCount += newCount;
        }

        public void UpdateText()
        {
            _itemNameText.text = _itemName;
            _itemCountText.text = _itemCount.ToString();
        }

        public void AddEvents()
        {
            // Submit 이벤트(아이템 사용), Cancel 이벤트(뒤로 가기)
            PokemonBattleManager manager = PokemonBattleManager.Instance;

            EventTrigger trigger = GetComponent<EventTrigger>();

            EventTrigger.Entry entrySubmit = new EventTrigger.Entry();
            entrySubmit.eventID = EventTriggerType.Submit;
            entrySubmit.callback.AddListener((data) => manager._itemManager.UseItem(gameObject));
            trigger.triggers.Add(entrySubmit);

            EventTrigger.Entry entryCancel = new EventTrigger.Entry();
            entryCancel.eventID = EventTriggerType.Cancel;
            entryCancel.callback.AddListener((data) => manager.SelectAction());
            trigger.triggers.Add(entryCancel);
        }
    }
}