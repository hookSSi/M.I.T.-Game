using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Pokemon
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
    }
}