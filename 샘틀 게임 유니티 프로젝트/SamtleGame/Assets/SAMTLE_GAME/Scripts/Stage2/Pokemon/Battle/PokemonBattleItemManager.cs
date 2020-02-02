using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon
{
    public delegate void BattleDelegate(Pokemon myPokemon, Pokemon enemyPokemon);

    public struct BattleEvent
    {
        public int _priority;
        public BattleDelegate _event;
    }

    public class PokemonBattleItemManager : MonoBehaviour
    {
        private PokemonBattleManager _manager;

        [SerializeField] private GameObject _itemPrefab;

        [SerializeField] private List<BattleItem> _itemList = new List<BattleItem>();


        private void Start()
        {
            // 게임 오브젝트 초기화

        }


        private void Update()
        {
            
        }

        public void Initialize()
        {
            // event system의 first select로 선택될 아이템을 정한다.
            if (_itemList.Count != 0)
            {
                var eventSystem = FindObjectOfType<PokemonBattleEventSystem>();

                var firstItem = _itemList[0].gameObject;

                eventSystem.SetIndexObject(firstItemObject: firstItem);
            }
        }

        public void UseItem(GameObject usingObject)
        {
            BattleItem usingItem = usingObject.GetComponent<BattleItem>();

            int index = _itemList.FindIndex(item => { return item._itemName == usingItem._itemName; });

            // 아이템 사용 효과 ...

            _itemList[index]._itemEvent._event(_manager._myPokemon, _manager._enemyPokemon);

            // 아이템 개수 감소

            Debug.Log("이전 아이템 개수 : " + usingItem._itemCount);

            _itemList[index].SetCount(-1);

            Debug.Log("이전 아이템 개수 : " + usingItem._itemCount);

            // 아이템 삭제
            if (usingItem._itemCount <= 0)
            {
                RemoveItem(index);
            }
        }

        public void AddItem(string itemName, string itemCaption, int itemCount, BattleItem.ItemType itemType, BattleEvent itemEvent)
        {
            if (_itemPrefab == null)
            {
                Debug.Log("경고 : 아이템 프리팹이 존재하지 않습니다.");
                return;
            }

            var newObject = Instantiate(_itemPrefab, transform);
            var newItem = newObject.GetComponent<BattleItem>();

            newItem.SetValues(itemName, itemCaption, itemCount, itemType, itemEvent);
            newItem.UpdateText();

            _itemList.Add(newItem);
        }

        // 아이템 삭제(아이템 개체를 통해)
        public void RemoveItem(BattleItem removedItem)
        {
            try
            {
                int index = _itemList.FindIndex(item => { return item._itemName == removedItem._itemName; });
                RemoveItem(index);
            }
            catch (System.ArgumentNullException error)
            {
                Debug.Log("오류가 발생했습니다 : " + error.Message);
            }
        }

        // 아이템 삭제(리스트 상의 인덱스를 이용해)
        public void RemoveItem(int index)
        {
            /*  사소한 문제
             *  아이템을 모두 사용하고 나면 아이템 오브젝트를 완전히 삭제해버린다.
             *  => 새로 아이템을 추가하려면 오브젝트를 새로 생성해야 하는 문제점이 존재한다.
             *  => 유니티 상에 성능 저하가 심각함.
             *  (개선 필요)
             */
            if (_itemList.Count <= index)
            {
                Debug.Log("경고 : 조회할 수 없는 인덱스의 아이템을 삭제하려 했습니다.(PokemonBattleItemManager)");

                var item = _itemList[index];

                _itemList.RemoveAt(index);
                Destroy(item.gameObject);
            }
        }
    }
}