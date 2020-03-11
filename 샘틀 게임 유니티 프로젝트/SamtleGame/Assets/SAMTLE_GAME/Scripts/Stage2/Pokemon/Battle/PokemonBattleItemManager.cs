using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace MIT.SamtleGame.Stage2.Pokemon
{
    public class PokemonBattleItemManager : MonoBehaviour
    {
        public Items _items;
        public Transform _contentObject;

        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private BattleItem _dummyItem;
        [SerializeField] private List<BattleItem> _itemList = new List<BattleItem>();

        [HideInInspector] public string _previousItemName = "";

        private void Awake()
        {
            _itemList = new List<BattleItem>();
            Init();

            // 아이템 추가
            AddItem("전 부회장의 3신기", "기묘한 '옛것'의 기운이 느껴지는 물건이다...", 5, 
                BattleItem.ItemType.Etc, _items.TheTrinity);
            AddItem("노사람즈스카이 확장팩", "디스 이즈 낫 게임...", 1,
                BattleItem.ItemType.Consume, _items.NoHumansSky);
            AddItem("마시면 큰일나요 보드카",
                "위험한 꼬꼬마 새내기 여러분이 좋아할 만한 사악한 물건. 마시면 미래로의 시간여행을 경험할 수 있다.", 44,
                BattleItem.ItemType.Consume, _items.DangerousVodka);
            AddItem("MIT산 특제 보드게임", "대체 왜 이런 걸 준거지?", 7,
                BattleItem.ItemType.Etc, _items.MITsBoardGame);
            AddItem("몬스터볼", "야생의 포켓몬을 잡을 수 있는 몬스터볼이다.", 99,
                BattleItem.ItemType.Pokeball, _items.Pokeball);
        }

        private void Init()
        {
            // _dummyItem Setting
            EventTrigger trigger = _dummyItem.GetComponent<EventTrigger>();

            EventTrigger.Entry entryCancel = new EventTrigger.Entry();
            entryCancel.eventID = EventTriggerType.Cancel;
            entryCancel.callback.AddListener((data) => { PokemonBattleManager.Instance.SelectAction(); });
            trigger.triggers.Add(entryCancel);
        }

        private void OnEnable()
        {
            SetFirstItem();
        }

        public void SetFirstItem()
        {
            // event system의 first select로 선택될 아이템을 정한다.
            var eventSystem = FindObjectOfType<Tool.PokemonBattleEventSystem>();

            if (_itemList.Count != 0)
            {
                eventSystem.SetIndexObject(firstItemObject: _itemList[0].gameObject);

                _dummyItem.gameObject.SetActive(false);
            }
            else
            {
                eventSystem.SetIndexObject(firstItemObject: _dummyItem.gameObject);

                _dummyItem.gameObject.SetActive(true);
            }
        }

        public void UseItem(GameObject usingObject)
        {
            try
            {
                BattleItem usingItem = usingObject.GetComponent<BattleItem>();

                int index =
                    _itemList.FindIndex(item => { return item._itemName == usingItem._itemName; });
                _previousItemName = usingItem._itemName;

                PokemonBattleManager.Instance.UseItem(usingItem._itemEvent);

                _itemList[index].SetCount(-1);
                _itemList[index].UpdateText();

                if (usingItem._itemCount <= 0)
                {
                    RemoveItem(index);
                }
            }
            catch (System.ArgumentNullException error)
            {
                Debug.Log("오류가 발생했습니다 : " + error.Message);
            }
        }

        public void AddItem(string itemName, string itemCaption, int itemCount, BattleItem.ItemType itemType, UnityAction<Pokemon, Pokemon> itemEvent)
        {
            if (_itemPrefab == null)
            {
                Debug.Log("경고 : 아이템 프리팹이 존재하지 않습니다.");
                return;
            }

            GameObject newObject = Instantiate(_itemPrefab, _contentObject);
            BattleItem newItem = newObject.GetComponent<BattleItem>();

            BattleEvent newItemEvent = new BattleEvent();
            newItemEvent.AddListener(itemEvent);

            newItem.SetValues(itemName, itemCaption, itemCount, itemType, newItemEvent);
            newItem.UpdateText();
            newItem.AddEvents();

            SetFirstItem();

            _itemList.Add(newItem);
        }

        // 아이템 삭제(아이템 개체를 통해)
        public void RemoveItem(BattleItem removedItem)
        {
            try
            {
                int index =
                    _itemList.FindIndex(item => { return item._itemName == removedItem._itemName; });
                RemoveItem(index);
            }
            catch (System.ArgumentNullException error)
            {
                Debug.Log("오류가 발생했습니다 : " + error.Message);
            }
        }

        public void RemoveItem(int index)
        {
            var item = _itemList[index];

            _itemList.RemoveAt(index);
            Destroy(item.gameObject);

            SetFirstItem();
        }
    }
}