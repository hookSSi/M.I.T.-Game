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
            entrySubmit.callback.AddListener((data) => { manager._itemManager.UseItem(gameObject); });
            trigger.triggers.Add(entrySubmit);

            EventTrigger.Entry entryCancel = new EventTrigger.Entry();
            entryCancel.eventID = EventTriggerType.Cancel;
            entryCancel.callback.AddListener((data) => { manager.SelectAction(); });
            trigger.triggers.Add(entryCancel);
        }
    }

    // 인게임 아이템 이벤트들
    public class Items
    {
        private static void FirstScript(out string firstDialog)
        {
            firstDialog =  "새내기는 " 
                + PokemonBattleManager.Instance._itemManager._previousItemName + "를(을) 사용했다!";
        }

        // 전 부회장의 3신기
        public void TheTrinity(Pokemon myPokemon, Pokemon enemyPokemon)
        {
            string dialog;
            // 확률에 따라 플레이어의 체력이 1/3이 되거나 적의 체력이 1/3이 되거나 아무 일도 일어나지 않는다.
            int nextEffect = Random.Range(1, 4);
            float damage;

            FirstScript(out dialog);
            PokemonBattleManager.AddNextText(dialog);

            switch (nextEffect)
            {
                case 1:
                    damage = enemyPokemon.MaxHealth / 3f + 0.1f;
                    enemyPokemon.Health -= damage;

                    PokemonBattleManager.AddNextText("제 1신기 폭풍을 부르는 선풍기로 인해 " + enemyPokemon.Info._name + "이(가) 피해를 입었다!");
                    break;
                case 2:
                    damage = myPokemon.MaxHealth / 3f + 0.1f;
                    myPokemon.Health -= damage;

                    PokemonBattleManager.AddNextText("제 2신기 음성 인식 되는 플라즈마볼이 잘못 작동해 " + myPokemon.Info._name + "이(가) 반동을 입었다...");
                    break;
                case 3:
                    PokemonBattleManager.AddNextText("제 3신기 워터 컨트리뷰터로 인해 물바다가 되었다...");
                    break;
            }
        }
    }
}